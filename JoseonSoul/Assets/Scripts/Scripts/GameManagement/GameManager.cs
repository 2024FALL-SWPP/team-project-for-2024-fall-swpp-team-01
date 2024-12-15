using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
using System.IO;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Save Info")]
    [SerializeField] private int currentSceneIndex;
    [SerializeField] private Vector3 lastVisitedFire;
    [SerializeField] private int lastVisitedFireSceneIdx;
    [SerializeField] private bool[] wellPurified;
    [SerializeField] private String filePath;

    [Header("Plyaer Status")]
    [SerializeField] private float currentHP;
    [SerializeField] private int potionRemained;
    [Header("Player Init Positions")]
    [SerializeField] private Vector3[] initPositions = new Vector3[4]{
        new Vector3(0,0,0),//Stage1Scene1
        new Vector3(0,0,0),//Stage1Scene2
        new Vector3(0,0,0),//Stage2
        new Vector3(0,0,0),//boss
    };

    [Header("Singleton Objects")]
    public GameObject player = PlayerController.Instance;
    public Stage_UIManager stage_UIManager = Stage_UIManager.Instance;
    public ThirdPersonCameraController mainCamera = ThirdPersonCameraController.Instance;


    private PlayerHealthManager healthManager;
    private PlayerPotionManager potionManager;
    private SoundManager soundManager;
    

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

    void Start()
    {
        healthManager = PlayerController.Instance.GetComponent<PlayerHealthManager>();
        if(healthManager == null)
            Debug.LogError("Player Health Manager Not Detected");

        potionManager = PlayerController.Instance.GetComponent<PlayerPotionManager>();
        if(potionManager == null)
            Debug.LogError("Player Potion Manager Not Detected");

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        soundManager.SetBgm(-1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        int sceneIdx = scene.buildIndex;
        Debug.Log("Scene Loaded + " + sceneIdx.ToString());
        if(1<= sceneIdx && sceneIdx <=4)
        {
            Debug.Log("Player Activated!");
            player.SetActive(true);
        }
        else
        {
            player.SetActive(false);
        }
         
        Stage_UIManager.Instance.EventTextOff();
    }

    public void LoadGame()
    {
        LoadInfo();
        LoadScene(true);
    }

    public void LoadInfo()
    {
        if (File.Exists(filePath))
        {
            // 파일이 존재하면 JSON 데이터 읽기
            string jsonData = File.ReadAllText(filePath);

            GameData gameData = JsonUtility.FromJson<GameData>(jsonData);
            Debug.Log("SceneIndex: " + gameData.currentSceneIndex);
            Debug.Log("Last Visited Fire: " + gameData.lastVisitedFire);

            currentSceneIndex = gameData.currentSceneIndex;
            lastVisitedFireSceneIdx = currentSceneIndex;
            lastVisitedFire = gameData.lastVisitedFire;
            wellPurified = gameData.wellPurified;

            currentHP = PlayerHealthManager.maxHP;
            potionRemained = PlayerPotionManager.maxPotion;
        }
        else
        {
            currentHP = PlayerHealthManager.maxHP;
            currentSceneIndex = 0;
            wellPurified = new bool[4]{false,false,false,false};
            lastVisitedFireSceneIdx = 0;
            lastVisitedFire = new Vector3(0,0,0);
            potionRemained = PlayerPotionManager.maxPotion;
        }
    }

    public void NewGame()
    {
        currentHP = PlayerHealthManager.maxHP;
        currentSceneIndex = 0;
        wellPurified = new bool[4]{false,false,false,false};
        lastVisitedFireSceneIdx = 0;
        lastVisitedFire = new Vector3(0,0,0);
        potionRemained = PlayerPotionManager.maxPotion;
        SaveGame();
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
    public void LoadScene(bool load)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        if(load)
        {
            SceneManager.LoadScene(lastVisitedFireSceneIdx);
            soundManager.SetBgm(lastVisitedFireSceneIdx);
            if(lastVisitedFire == Vector3.zero)
                playerController.InitPlayer(currentHP,potionRemained,initPositions[currentSceneIndex-1]);
            else
                playerController.InitPlayer(currentHP,potionRemained,lastVisitedFire);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex);
            soundManager.SetBgm(currentSceneIndex);
            playerController.InitPlayer(currentHP,potionRemained,initPositions[currentSceneIndex-1]);
        }
            
        
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
            
            healthManager.updateCurrentHP(PlayerHealthManager.maxHP, true);
            potionManager.updateCurrentPotion(PlayerPotionManager.maxPotion);
            SaveGame();
            Debug.Log("Saved and Healed");

            stage_UIManager.gameObject.GetComponent<EventResultUIManager>().ActvateEventCanvas("Game Saved!");
        }

        if(eventId == (int)GameManagement.Event.NextStage)
        {
            if(wellPurified[0] && wellPurified[1] && wellPurified[2] && wellPurified[3])
                NextScene();
            else
            {
                stage_UIManager.gameObject.GetComponent<EventResultUIManager>().ActvateEventCanvas("Some well is not Purified");
            }
        }
    }

    public bool[] GetWellPurifiedStatus()
    {
        return wellPurified;
    }

    public void PurifyWell(int wellId)
{
    Debug.Log($"Attempting to purify well_{wellId}");
    if(!wellPurified[wellId])
    {
        stage_UIManager.gameObject.GetComponent<EventResultUIManager>().ActvateEventCanvas("Well Purifed");
        wellPurified[wellId] = true;

        GameObject well = GameObject.Find($"well_{wellId}");
        if (well != null)
        {
            Debug.Log($"Found well_{wellId}, applying purification effect");
            WellParticleManager particleManager = well.GetComponent<WellParticleManager>();
            if (particleManager != null)
            {
                particleManager.OnWellPurified();
            }
        }
        else
        {
            Debug.LogWarning($"Could not find well_{wellId}");
        }
    }
}

    public void exitGame()
    {
        Debug.Log("QUIT BUTTON PRESSED");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(); // 빌드된 환경에서 게임 종료
        #endif
    }

}
