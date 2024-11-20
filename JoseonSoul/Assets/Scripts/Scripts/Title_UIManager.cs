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
        SceneManager.LoadScene("Stage1Scene1");
    }

    public void ClickLoadGameButton()
    {
        // TODO : Load ��ư�� ������ �� �Լ� ȣ��
    }
    /*title scene���� "���۹�" ��ư�� ������ �� ���۹� â�� ���� �Լ�*/
    public void ClickShowControlsButton()
    {
        ControlsPanel.SetActive(true);
    }

    /* ���۹� â���� x��ư�� ������ �� â�� �����ϴ� �Լ�*/
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
