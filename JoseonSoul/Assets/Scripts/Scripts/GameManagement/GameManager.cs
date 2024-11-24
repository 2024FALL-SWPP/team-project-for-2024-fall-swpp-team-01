using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
using System.IO;
using System;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Save Info")]
    [SerializeField] private int currentSceneIndex = 0;
    [SerializeField] private Vector3 lastVisitedFire;
    [SerializeField] private String filePath;

    [Header("Plyaer Status")]
    public GameObject playerPrefab;
    [SerializeField] private float currentHP;
    [SerializeField] private int potionRemained;
    [Header("Player Init Positions")]
    [SerializeField] private Vector3[] initPositions = new Vector3[5]{
        new Vector3(0,0,0),//Stage1Scene1
        new Vector3(0,0,0),//Stage1Scene2
        new Vector3(0,0,0),//Boss1
        new Vector3(0,0,0),//Stage2
        new Vector3(0,0,0)//Boss2
    };



    private void Awake()
    {
        // Singleton 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 새로 생성된 것을 제거
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
    }

    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            // 파일이 존재하면 JSON 데이터 읽기
            string jsonData = File.ReadAllText(filePath);

            GameData gameData = JsonUtility.FromJson<GameData>(jsonData);
            Debug.Log("SceneIndex: " + gameData.currentSceneIndex);
            Debug.Log("Last Visited Fire: " + gameData.lastVisitedFire);

            currentSceneIndex = gameData.currentSceneIndex;
            lastVisitedFire = gameData.lastVisitedFire;
            currentHP = PlayerHealthManager.maxHP;
            // potionRemained = PlayerPotionManager.maxPotion;

            LoadScene(true);
        }
        else
        {
            Debug.Log("No save file found at: " + filePath);
        }
    }

    public void NewGame()
    {
        currentHP = PlayerHealthManager.maxHP;
        currentSceneIndex = 0;
        //potionRemained = PlayerPotionManager.maxPotion;

        LoadScene(false);
    }

    public void SaveGame()
    {
        GameData gameData = new GameData(currentSceneIndex, lastVisitedFire);
        String jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Game data saved to: " + filePath);
    }

    public void NextScene()
    {
        currentSceneIndex += 1;

        LoadScene(false);
    }

    //저장된 Data Load면 True, 스토리 진행 상 다음 Scene으로 넘어가는거면 False
    public void LoadScene(Boolean load)
    {
        SceneManager.LoadScene(currentSceneIndex);
        GameObject player = Instantiate(playerPrefab);
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(load)
            playerController.InitPlayer(currentHP,potionRemained,lastVisitedFire);
        else
            playerController.InitPlayer(currentHP,potionRemained,initPositions[currentSceneIndex]);
    }

    // Player HP 변화 있을 때와 Potion 마실 때 호출해서 Sync 맞추기
    public void SetPlayerStatus(float currentHP, int potionRemained)
    {
        this.currentHP = currentHP;
        this.potionRemained = potionRemained;
    }
}
