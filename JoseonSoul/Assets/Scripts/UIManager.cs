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

    public GameObject GameMenuPanel; //Game Menu Panel

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

    // initial settings
    private int maxHP = 120, nowHP = 87, SP = 68;

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
        hpText.text = GenerateHPText(nowHP, maxHP);

        // Position & scale hpMax
        hpMaxRect.anchorMin = new Vector2(0, 1);
        hpMaxRect.anchorMax = new Vector2(0, 1);
        float hpMaxOffsetX = profileTextOffsetX * hpMaxOffsetScaleX;
        float hpMaxOffsetY = profileImageOffsetY * 3.4f * hpMaxOffsetScaleY;
        hpMaxRect.anchoredPosition = new Vector2(hpMaxOffsetX, hpMaxOffsetY);
        hpMaxHeight = canvasWidth * 20 / 976;
        UpdateHPMax(maxHP, true);

        // Position & scale hpNow
        hpNowRect.anchorMin = new Vector2(0, 1);
        hpNowRect.anchorMax = new Vector2(0, 1);
        float hpNowOffsetX = hpMaxOffsetX;
        float hpNowOffsetY = hpMaxOffsetY - (1 - hpNowHeightScale) * hpMaxHeight / 2;
        hpNowRect.anchoredPosition = new Vector2(hpNowOffsetX, hpNowOffsetY);
        hpNowHeight = hpMaxHeight * hpNowHeightScale;
        UpdateHPNow(nowHP, true);

        // Position & scale sp (TEMPORARY)
        spRect.anchorMin = new Vector2(0, 1);
        spRect.anchorMax = new Vector2(0, 1);
        float spSize = canvasWidth * 100 / 976;
        float spOffsetX = profileImageOffsetX * 9;
        float spOffsetY = profileImageOffsetY;
        spRect.sizeDelta = new Vector2(spSize, spSize);
        spRect.anchoredPosition = new Vector2(spOffsetX, spOffsetY);
        UpdateSP(SP, true);

        
}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameMenuPanel.SetActive(!GameMenuPanel.activeSelf);
        }
    }

    // Format the text showing nowHP/maxHP
    string GenerateHPText(int nowHP, int maxHP)
    {
        return "HP " + nowHP + "/" + maxHP;
    }

    // Update maxHP value
    public void UpdateHPMax(int value, bool isAbsolute)
    {
        maxHP = isAbsolute ? value : (maxHP + value);
        hpText.text = GenerateHPText(nowHP, maxHP);
        hpMaxWidth = canvasWidth * 1.5f * maxHP / 976;
        hpMaxRect.sizeDelta = new Vector2(hpMaxWidth, hpMaxHeight);
    }

    // Update nowHP value
    public void UpdateHPNow(int value, bool isAbsolute)
    {
        nowHP = isAbsolute ? value : (nowHP + value);
        hpText.text = GenerateHPText(nowHP, maxHP);
        hpNowWidth = canvasWidth * 1.5f * nowHP / 976;
        hpNowRect.sizeDelta = new Vector2(hpNowWidth, hpNowHeight);
    }

    // Update SP value
    public void UpdateSP(int value, bool isAbsolute)
    {
        SP = isAbsolute ? value : (SP + value);
        sp.fillAmount =(float)SP / 100;
    }
     // TEMPORARY FUNCTIONS used to debug UIManager
    public void Temp_IncHPMax()
    {
        UpdateHPMax(1, false);
    }

    public void Temp_DecHPMax()
    {
        UpdateHPMax(-1, false);
    }

    public void Temp_IncHPNow()
    {
        UpdateHPNow(1, false);
    }

    public void Temp_DecHPNow()
    {
        UpdateHPNow(-1, false);
    }

    public void Temp_IncSP()
    {
        UpdateSP(1, false);
    }

    public void Temp_DecSP()
    {
        UpdateSP(-1, false);
    }

    //메인 메뉴로 나가기 버튼
    public void ClickReturnToGameButton()
    {
        GameMenuPanel.SetActive(!GameMenuPanel.activeSelf);
    }

    //게임 종료 버튼
    public void ClickExitGameButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
