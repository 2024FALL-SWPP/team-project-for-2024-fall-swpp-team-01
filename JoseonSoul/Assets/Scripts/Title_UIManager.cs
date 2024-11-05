using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class Title_UIManager : MonoBehaviour
{

    public GameObject ControlsPanel;
    // Start is called before the first frame update
    void Start()
    {
        ControlsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickNewGameButton()
    {
        SceneManager.LoadScene("Stage1Scene");
    }

    public void ClickLoadGameButton()
    {
        // TODO : Load 버튼을 눌렀을 때 함수 호출
    }
    /*title scene에서 "조작법" 버튼을 눌렀을 때 조작법 창을 띄우는 함수*/
    public void ClickShowControlsButton()
    {
        ControlsPanel.SetActive(true);
    }

    /* 조작법 창에서 x버튼을 눌렀을 때 창을 종료하는 함수*/
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
