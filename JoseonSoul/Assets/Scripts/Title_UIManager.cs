using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class Title_UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        // Load ��ư�� ������ �� �Լ� ȣ��
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
