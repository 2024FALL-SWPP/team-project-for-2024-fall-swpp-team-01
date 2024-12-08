using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using TMPro;
using UnityEngine;

public class EventResultUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    void Start()
    {
        DontDestroyOnLoad(canvas);
        canvas.gameObject.SetActive(false);
    }

    public void ActvateEventCanvas(String text)
    {
        canvas.gameObject.SetActive(true);
        canvas.GetComponentInChildren<TextMeshProUGUI>().text = text;
        Invoke("EndEvent",3.0f);
    }

    void EndEvent()
    {
        canvas.gameObject.SetActive(false);
    }
}
