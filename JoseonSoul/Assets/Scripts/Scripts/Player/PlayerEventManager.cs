using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;
using System.ComponentModel;

public class PlayerEventManager : MonoBehaviour
{
    // Handle Well Event and Send other events to game manager


    Stage_UIManager UIManager;
    GameManager gameManager;

    private bool canEvent = false;
    private int eventId;
    private int wellId;
    
    void Start()
    {
        UIManager = Stage_UIManager.Instance;
        if(UIManager == null)
            Debug.LogError("UIManager not found");       

        gameManager = GameManager.Instance;
        if(gameManager == null)
            Debug.LogError("GameManager not Detected"); 
    }

    // Update is called once per frame
    void Update()
    {
        if(canEvent && Input.GetKeyDown(KeyCode.E))
        {
            if(eventId == (int)GameManagement.Event.Well)
                gameManager.PurifyWell(wellId);
            else
                gameManager.handleEvent(eventId);
        }
            
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Well"))  
        {
            UIManager.EventTextOn((int)GameManagement.Event.Well);
            eventId = (int)GameManagement.Event.Well;
            canEvent = true;

            string wellName = other.gameObject.name;
            char lastChar = wellName[wellName.Length - 1];

            wellId = int.Parse(lastChar.ToString());
            //Add logic to detect wellId
        }

        if (other.CompareTag("Fire"))  
        {
            UIManager.EventTextOn((int)GameManagement.Event.Fire);
            eventId = (int)GameManagement.Event.Fire;
            canEvent = true;
        }

        if (other.CompareTag("NextStage"))  
        {
            Debug.Log("Next Stage Detected");
            UIManager.EventTextOn((int)GameManagement.Event.NextStage);
            eventId = (int)GameManagement.Event.NextStage;
            canEvent = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Well") || other.CompareTag("Fire") || other.CompareTag("NextStage"))
        {
            UIManager.EventTextOff();
            canEvent = false;
        }
    }
}
