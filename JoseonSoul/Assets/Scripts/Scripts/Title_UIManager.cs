using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class Title_UIManager : MonoBehaviour
{
    [Header("Manager Objects")]
    public GameObject ControlsPanel;
    public GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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
