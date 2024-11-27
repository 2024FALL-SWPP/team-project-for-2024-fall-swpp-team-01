using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

public class PlayerEventManager : MonoBehaviour
{
    // Start is called before the first frame update
    Stage_UIManager UIManager;
    GameManager gameManager;
    
    void Start()
    {
        UIManager = FindObjectOfType<Stage_UIManager>();
        if(UIManager == null)
            Debug.LogError("UIManager not found");       

        gameManager = FindObjectOfType<GameManager>();
        if(gameManager == null)
            Debug.LogError("GameManager not Detected"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Well"))  
        {
            UIManager.EventTextOn((int)GameManagement.Event.Well);
        }

        if (other.CompareTag("Fire"))  
        {
            UIManager.EventTextOn((int)GameManagement.Event.Fire);
        }

        if (other.CompareTag("NextStage"))  
        {
            UIManager.EventTextOn((int)GameManagement.Event.NextStage);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Well") || other.CompareTag("Fire") || other.CompareTag("NextStage"))
        {
            UIManager.EventTextOff();
        }
    }
}
