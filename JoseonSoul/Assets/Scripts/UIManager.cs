using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // public variables that MUST be assigned objects before playing
    public Image imageObject;
    public Sprite profileImage;
    public TextMeshProUGUI profileText;
    public TextMeshProUGUI hpText;
    public Image hpMax;
    public Image hpNow;
    public Image sp;

    // parameters that need to be shared over functions
    private float canvasWidth;
    RectTransform hpMaxRect;
    RectTransform hpNowRect;
    private float hpMaxWidth, hpMaxHeight;
    private float hpNowWidth, hpNowHeight;

    // constant parameters that are used to position & scale UI elements
    private float profileImageScale = 13;
    private float profileImageOffsetScaleX = 1, profileImageOffsetScaleY = 1;
    private float profileTextOffsetScaleX = 1, profileTextOffsetScaleY = 1;
    private float hpTextOffsetScaleX = 1, hpTextOffsetScaleY = 1;
    private float hpMaxOffsetScaleX = 1, hpMaxOffsetScaleY = 1;
    private float hpNowHeightScale = 0.6f;

    void Start()
    {
        // Assign profile image to imageObject
        imageObject.sprite = profileImage;

        // Get component RectTransform of each UI elements
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        RectTransform imageRect = imageObject.GetComponent<RectTransform>();
        RectTransform profileTextRect = profileText.GetComponent<RectTransform>();
        RectTransform hpTextRect = hpText.GetComponent<RectTransform>();
        hpMaxRect = hpMax.GetComponent<RectTransform>();
        hpNowRect = hpNow.GetComponent<RectTransform>();
        RectTransform spRect = sp.GetComponent<RectTransform>();

        canvasWidth = canvasRect.rect.width;

        // Position & scale imageObject
        imageRect.anchorMin = new Vector2(0, 1);
        imageRect.anchorMax = new Vector2(0, 1);
        float imageWidth = canvasWidth / profileImageScale;
        float imageHeight = imageWidth * 1.23f;
        float profileImageOffsetX = imageWidth / 2 * profileImageOffsetScaleX;
        float profileImageOffsetY = (-imageWidth + imageHeight / 2) * profileImageOffsetScaleY;
        imageRect.sizeDelta = new Vector2(imageWidth, imageHeight);
        imageRect.anchoredPosition = new Vector2(profileImageOffsetX, profileImageOffsetY);

        // Position & scale profileText
        profileTextRect.anchorMin = new Vector2(0, 1);
        profileTextRect.anchorMax = new Vector2(0, 1);
        float profileTextOffsetX = profileImageOffsetX * 3.4f * profileTextOffsetScaleX;
        float profileTextOffsetY = profileImageOffsetY * 1.05f * profileTextOffsetScaleY;
        profileTextRect.anchoredPosition = new Vector2(profileTextOffsetX, profileTextOffsetY);
        profileText.fontSize = canvasWidth * 36 / 976;

        // Position & scale hpText
        hpTextRect.anchorMin = new Vector2(0, 1);
        hpTextRect.anchorMax = new Vector2(0, 1);
        float hpTextOffsetX = profileTextOffsetX * hpTextOffsetScaleX;
        float hpTextOffsetY = profileImageOffsetY * 2.6f * hpTextOffsetScaleY;
        hpTextRect.anchoredPosition = new Vector2(hpTextOffsetX, hpTextOffsetY);
        hpText.fontSize = canvasWidth * 18 / 976;

        // Position & scale hpMax
        hpMaxRect.anchorMin = new Vector2(0, 1);
        hpMaxRect.anchorMax = new Vector2(0, 1);
        float hpMaxOffsetX = profileTextOffsetX * hpMaxOffsetScaleX;
        float hpMaxOffsetY = profileImageOffsetY * 3.4f * hpMaxOffsetScaleY;
        hpMaxRect.anchoredPosition = new Vector2(hpMaxOffsetX, hpMaxOffsetY);
        hpMaxHeight = canvasWidth * 20 / 976;

        // Position & scale hpNow
        hpNowRect.anchorMin = new Vector2(0, 1);
        hpNowRect.anchorMax = new Vector2(0, 1);
        float hpNowOffsetX = hpMaxOffsetX;
        float hpNowOffsetY = hpMaxOffsetY - (1 - hpNowHeightScale) * hpMaxHeight / 2;
        hpNowRect.anchoredPosition = new Vector2(hpNowOffsetX, hpNowOffsetY);
        hpNowHeight = hpMaxHeight * hpNowHeightScale;

        // Position & scale sp (TEMPORARY)
        spRect.anchorMin = new Vector2(0, 1);
        spRect.anchorMax = new Vector2(0, 1);
        float spSize = canvasWidth * 100 / 976;
        float spOffsetX = profileImageOffsetX * 9;
        float spOffsetY = profileImageOffsetY;
        spRect.sizeDelta = new Vector2(spSize, spSize);
        spRect.anchoredPosition = new Vector2(spOffsetX, spOffsetY);
    }

    void Update()
    {
        // Update HP info
        int nowHP = 87;
        int maxHP = 120;
        int SP = 68;
        hpText.text = "HP " + nowHP + "/" + maxHP;
        hpMaxWidth = canvasWidth * 1.5f * maxHP / 976;
        hpMaxRect.sizeDelta = new Vector2(hpMaxWidth, hpMaxHeight);
        hpNowWidth = canvasWidth * 1.5f * nowHP / 976;
        hpNowRect.sizeDelta = new Vector2( hpNowWidth, hpNowHeight);
        sp.fillAmount = (float)SP / 100;
    }
}
