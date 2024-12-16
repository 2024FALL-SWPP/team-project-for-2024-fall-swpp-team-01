using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using TMPro;
using UnityEngine;

public class EventResultUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;
    void Start()
    {
        text.gameObject.SetActive(false);
    }

    public void ActvateEventCanvas(String text)
    {
        this.text.gameObject.SetActive(true);
        this.text.text = text;
        Invoke("EndEvent",3.0f);
    }

    void EndEvent()
    {
        text.gameObject.SetActive(false);
    }
}
