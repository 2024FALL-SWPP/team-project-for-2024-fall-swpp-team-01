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
    [SerializeField] private int currentSceneIndex;
    [SerializeField] private Vector3 lastVisitedFire;
    [SerializeField] private bool[] wellPurified;
    [SerializeField] private String filePath;

    [Header("Plyaer Status")]
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

    [Header("Singleton Objects")]
    public GameObject player = PlayerController.Instance;
    public Stage_UIManager stage_UIManager = Stage_UIManager.Instance;
    public ThirdPersonCameraController mainCamera = ThirdPersonCameraController.Instance;
    

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        player.SetActive(true);
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

            this.currentSceneIndex = gameData.currentSceneIndex;
            this.lastVisitedFire = gameData.lastVisitedFire;
            this.wellPurified = gameData.wellPurified;

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
        wellPurified = new bool[4]{false,false,false,false};
        //potionRemained = PlayerPotionManager.maxPotion;
        LoadScene(false);
    }

    public void SaveGame()
    {
        GameData gameData = new GameData(currentSceneIndex, lastVisitedFire, wellPurified);
        String jsonData = JsonUtility.ToJson(gameData);
        Debug.Log(jsonData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Game data saved to: " + filePath);
    }

    public void NextScene()
    {
        currentSceneIndex += 1;
        LoadScene(false);
    }

//저장된 Data Load면 True, 스토리 진행 상 다음 Scene으로 넘어가는거면 False
    private void LoadScene(bool load)
    {
        SceneManager.LoadScene(currentSceneIndex);

        PlayerController playerController = player.GetComponent<PlayerController>();

        if(load)
            playerController.InitPlayer(currentHP,potionRemained,lastVisitedFire);
        else
            playerController.InitPlayer(currentHP,potionRemained,initPositions[currentSceneIndex]);
        
        Debug.Log("New Scene Loaded");
    }


    // Player HP 변화 있을 때와 Potion 마실 때 호출해서 Sync 맞추기
    public void SetPlayerStatus(float currentHP, int potionRemained)
    {
        this.currentHP = currentHP;
        this.potionRemained = potionRemained;
    }


    //Handle Event(Save, NextStage) Called by PlayerEventManager
    public void handleEvent(int eventId)
    {
        if(eventId == (int)GameManagement.Event.Fire)
        {
            lastVisitedFire = player.transform.position;
            
            currentHP = PlayerHealthManager.maxHP;
            // potionRemained = PlayerPotionManager.maxPotion;
            SaveGame();
            Debug.Log("Saved and Healed");
        }

        if(eventId == (int)GameManagement.Event.NextStage)
        {
            if(wellPurified[0] && wellPurified[1] && wellPurified[2] && wellPurified[3])
                NextScene();
            else
                Debug.Log("Some wells are not Purified");
        }
    }

    public void PurifyWell(int wellId)
    {
        Debug.Log(wellId.ToString() + " Purified!!");
        wellPurified[wellId] = true;
    }
}
