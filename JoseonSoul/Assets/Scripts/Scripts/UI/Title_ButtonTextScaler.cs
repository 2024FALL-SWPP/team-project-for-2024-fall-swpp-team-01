using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Title_ButtonTextScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText;
    public float scaleFactor = 1.2f;
    private Vector3 originalScale;

    void Start()
    {
        if (buttonText != null)
        {
            originalScale = buttonText.transform.localScale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.transform.localScale = originalScale * scaleFactor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.transform.localScale = originalScale;
        }
    }
}
