using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class Title_UIManager : MonoBehaviour
{

    public static Title_UIManager Instance { get; private set; }
    public GameObject canvas;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 객체 유지
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 있으면 새로 생성된 오브젝트 삭제
            return;
        }

        DontDestroyOnLoad(canvas);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.buildIndex == 7)
            canvas.SetActive(true);

        else
            canvas.SetActive(false);
    }

    [Header("Manager Objects")]
    public GameObject ControlsPanel;
    public GameManager gameManager;
    
    void Start()
    {
        gameManager = GameManager.Instance;
        if(gameManager == null)
            Debug.LogError("Game Manager Not Found");
        ControlsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickNewGameButton()
    {
        gameManager.NewGame();
    }

    public void ClickLoadGameButton()
    {
        gameManager.LoadGame();
    }
    
    public void ClickShowControlsButton()
    {
        ControlsPanel.SetActive(true);
    }

    
    public void CilckExitControlsPanelButton()
    {
        ControlsPanel.SetActive(false);
    }

    public void ClickExitGameButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
