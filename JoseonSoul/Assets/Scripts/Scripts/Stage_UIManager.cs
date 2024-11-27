using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameManagement;
using UnityEngine.SceneManagement;

public class Stage_UIManager : MonoBehaviour
{
    // public variables that MUST be assigned objects before playing
    public static Stage_UIManager Instance { get; private set; }

    private Animator animator;
    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 객체 유지
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 있으면 새로 생성된 오브젝트 삭제
            return;
        }

        DontDestroyOnLoad(canvas);
        canvas.SetActive(false);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.buildIndex == 5)
            canvas.SetActive(false);
        else
            canvas.SetActive(true);
    }

    public Image imageObject;
    public Sprite profileImage;
    public TextMeshProUGUI profileText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI interactionText;  

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

    // initial settings
    private float maxHP = 120, nowHP = 87, SP = 68, maxSP = 100;

    [Header("Stage_Canvas")]
    public GameObject canvas;

    [Header("Player")]
    public GameObject player;

    [Header("Strings for message")]
    private String[] eventStrings = {"Purify a Well : E", // Well Event String
                                    "Save and Heal : E", // Fire Event String
                                    "Move to next Stage : E"}; // Next Stage Event String


    private PlayerHealthManager healthManager;

    void Start()
    {
        healthManager = player.GetComponent<PlayerHealthManager>();
        if(healthManager == null)
            Debug.LogError("Player Health Manager Not Detected");

        maxHP = healthManager.getMaxHP();
        maxSP = healthManager.getMaxSP();
        
        // Assign profile image to imageObject
        imageObject.sprite = profileImage;

        // Get component RectTransform of each UI elements
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
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

        interactionText.rectTransform.anchorMin = new Vector2(0.5f, 0f);  // X축 중앙, Y축 하단
        interactionText.rectTransform.anchorMax = new Vector2(0.5f, 0f);  // X축 중앙, Y축 하단
        interactionText.rectTransform.anchoredPosition = new Vector2(0, 100);  // 하단에서 50px 위쪽

        interactionText.gameObject.SetActive(false);  // 초기에는 텍스트 숨기기


        healthManager.updateMaxHP(maxHP, true);
        healthManager.updateCurrentHP(nowHP, true);
        healthManager.updateCurrentSP(SP, true);
    }


    // Format the text showing nowHP/maxHP
    string GenerateHPText(float nowHP, float maxHP)
    {
        return "HP " + (int)nowHP + "/" + (int)maxHP;
    }

    // Update maxHP value
    public void UpdateMaxHP()
    {
        maxHP = healthManager.getMaxHP();
        hpText.text = GenerateHPText(nowHP, maxHP);
        hpMaxWidth = canvasWidth * 1.5f * maxHP / 976;
        hpMaxRect.sizeDelta = new Vector2(hpMaxWidth, hpMaxHeight);
    }

    // Update nowHP value
    public void UpdateCurrentHP()
    {
        nowHP = healthManager.getCurrentHP();
        hpText.text = GenerateHPText(nowHP, maxHP);
        hpNowWidth = canvasWidth * 1.5f * nowHP / 976;
        hpNowRect.sizeDelta = new Vector2(hpNowWidth, hpNowHeight);
    }

    // Update SP value
    public void UpdateSP()
    {
        if(healthManager == null)
            Debug.LogError("health Manager is NULL!!!");
        SP = healthManager.getCurrentSP();
        sp.fillAmount = SP / 100;
    }

    public void EventTextOn(int eventNum)
    {
        interactionText.text = eventStrings[eventNum];
        interactionText.gameObject.SetActive(true);
    }

    public void EventTextOff()
    {
        interactionText.gameObject.SetActive(false);
    }

}
