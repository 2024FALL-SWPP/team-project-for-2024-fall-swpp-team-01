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
    public static Stage_UIManager Instance { get; private set; }

    private Animator animator;
    
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

    // public variables that MUST be assigned objects before playing
    public Image imageObject;
    public Sprite profileImage;
    public TextMeshProUGUI profileText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI bossNameText;
    public TextMeshProUGUI potionCountText;

    public Image hpMax;
    public Image hpOld;
    public Image hpNow;
    public Image sp;
    public Image bossHpMax;
    public Image bossHpOld;
    public Image bossHpNow;
    public Image potionContainer;
    public Image potionImage;

    // parameters that need to be shared over functions
    private float canvasWidth;
    private float canvasHeight;
    RectTransform hpMaxRect;
    RectTransform hpOldRect;
    RectTransform hpNowRect;
    private float hpMaxWidth, hpMaxHeight;
    private float hpOldWidth, hpOldHeight;
    private float hpNowWidth, hpNowHeight;
    RectTransform bossHpMaxRect;
    RectTransform bossHpOldRect;
    RectTransform bossHpNowRect;
    private float bossHpMaxWidth, bossHpMaxHeight;
    private float bossHpOldWidth, bossHpOldHeight;
    private float bossHpNowWidth, bossHpNowHeight;
    Boolean isHPAnimPlaying = false;

    // constant parameters that are used to position & scale UI elements
    private float profileImageScale = 13;
    private float profileImageOffsetScaleX = 1, profileImageOffsetScaleY = 1;
    private float profileTextOffsetScaleX = 1, profileTextOffsetScaleY = 1;
    private float hpTextOffsetScaleX = 1, hpTextOffsetScaleY = 1;
    private float hpMaxOffsetScaleX = 1, hpMaxOffsetScaleY = 1;
    private float hpNowHeightScale = 0.6f;
    private float hpWidthScale = 1;
    private float spOffsetScaleX = 1, spOffsetScaleY = 1;
    private float interactionTextOffsetScaleY = 1;
    private float bossHpMaxOffsetScaleY = 1;
    private float bossHpNowHeightScale = 0.6f;
    private float bossHpWidthScale = 1;
    private float potionContainerOffsetScaleY = 1;

    // Switches UI between boss mode and stage mode
    Boolean isBossSet = false;
    public void setBossStage(Boolean setBoss)
    {
        bossHpMax.gameObject.SetActive(setBoss);
        bossHpOld.gameObject.SetActive(setBoss);
        bossHpNow.gameObject.SetActive(setBoss);
        bossNameText.gameObject.SetActive(setBoss);
    }

    // Temporary function and variable to debug setBossStage function
    // ------------------------------ DEBUG BUTTONS ------------------------------
    public void debugBossSet()
    {
        isBossSet = !isBossSet;
        setBossStage(isBossSet);
    }
    public void BossNowHPInc()
    {
        UpdateBossNowHP(10, false);
    }
    public void BossNowHPDec()
    {
        UpdateBossNowHP(-10, false);
    }
    public void PotionInc()
    {
        potionCount++;
        potionCountText.text = potionCount.ToString();
    }
    public void PotionDec()
    {
        potionCount--;
        potionCountText.text = potionCount.ToString();
    }
    public void HPInc()
    {
        healthManager.updateCurrentHP(10, false);
    }
    public void HPDec()
    {
        healthManager.updateCurrentHP(-10, false);
    }
    // ----------------------------------- END -----------------------------------

    // initial settings - constants
    private float maxHP, maxSP, bossMaxHP = 500, hpAnimSpeed = 1, hpAnimDelay = 2;

    // initial settings - variables
    private float nowHP = 87, oldHP = 87, SP = 68, bossNowHP = 470, bossOldHP = 470, potionCount = 0;

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
        RectTransform imageRect = imageObject.rectTransform;
        RectTransform profileTextRect = profileText.rectTransform;
        RectTransform hpTextRect = hpText.rectTransform;
        hpMaxRect = hpMax.rectTransform;
        hpOldRect = hpOld.rectTransform;
        hpNowRect = hpNow.rectTransform;
        RectTransform spRect = sp.rectTransform;
        RectTransform interactionTextRect = interactionText.rectTransform;
        bossHpMaxRect = bossHpMax.rectTransform;
        bossHpOldRect = bossHpOld.rectTransform;
        bossHpNowRect = bossHpNow.rectTransform;
        RectTransform bossNameTextRect = bossNameText.rectTransform;
        RectTransform potionContainerRect = potionContainer.rectTransform;
        RectTransform potionImageRect = potionImage.rectTransform;
        RectTransform potionCountTextRect = potionCountText.rectTransform;

        canvasWidth = canvasRect.rect.width;
        canvasHeight = canvasRect.rect.height;

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
        hpMaxWidth = canvasWidth * 1.5f * maxHP / 976 * hpWidthScale;
        hpMaxHeight = canvasWidth * 20 / 976;
        hpMaxRect.sizeDelta = new Vector2(hpMaxWidth, hpMaxHeight);

        // Position & scale hpNow
        hpNowRect.anchorMin = new Vector2(0, 1);
        hpNowRect.anchorMax = new Vector2(0, 1);
        float hpNowOffsetX = hpMaxOffsetX;
        float hpNowOffsetY = hpMaxOffsetY - (1 - hpNowHeightScale) * hpMaxHeight / 2;
        hpNowRect.anchoredPosition = new Vector2(hpNowOffsetX, hpNowOffsetY);
        hpNowHeight = hpMaxHeight * hpNowHeightScale;

        // Position & scale hpOld
        hpOldRect.anchorMin = new Vector2(0, 1);
        hpOldRect.anchorMax = new Vector2(0, 1);
        float hpOldOffsetX = hpNowOffsetX;
        float hpOldOffsetY = hpNowOffsetY;
        hpOldRect.anchoredPosition = new Vector2(hpOldOffsetX, hpOldOffsetY);
        hpOldHeight = hpNowHeight;

        // Position & scale sp
        spRect.anchorMin = new Vector2(0, 1);
        spRect.anchorMax = new Vector2(0, 1);
        float spSize = canvasWidth * 100 / 976;
        float spOffsetX = profileImageOffsetX * 9 * spOffsetScaleX;
        float spOffsetY = profileImageOffsetY * spOffsetScaleY;
        spRect.sizeDelta = new Vector2(spSize, spSize);
        spRect.anchoredPosition = new Vector2(spOffsetX, spOffsetY);

        // Position & scale interactionText
        interactionTextRect.anchorMin = new Vector2(0.5f, 0f);
        interactionTextRect.anchorMax = new Vector2(0.5f, 0f);
        float interactionTextOffsetX = 0;
        float interactionTextOffsetY = canvasHeight * 0.25f * interactionTextOffsetScaleY;
        interactionTextRect.anchoredPosition = new Vector2(interactionTextOffsetX, interactionTextOffsetY);
        interactionText.fontSize = canvasWidth * 20 / 976;
        interactionText.gameObject.SetActive(false);

        // Position & scale bossHpMax
        bossHpMaxRect.anchorMin = new Vector2(0.5f, 0.5f);
        bossHpMaxRect.anchorMax = new Vector2(0.5f, 0.5f);
        bossHpMaxRect.pivot = new Vector2(0.5f, 1);
        float bossHpMaxOffsetX = 0;
        float bossHpMaxOffsetY = -canvasWidth * 140 / 976 * bossHpMaxOffsetScaleY;
        bossHpMaxRect.anchoredPosition = new Vector2(bossHpMaxOffsetX, bossHpMaxOffsetY);
        bossHpMaxWidth = canvasWidth * 500 / 976 * bossHpWidthScale;
        bossHpMaxHeight = canvasWidth * 20 / 976;
        bossHpMaxRect.sizeDelta = new Vector2(bossHpMaxWidth, bossHpMaxHeight);

        // Position & scale bossHpNow
        bossHpNowRect.anchorMin = new Vector2(0, 0.5f);
        bossHpNowRect.anchorMax = new Vector2(0, 0.5f);
        bossHpNowRect.pivot = new Vector2(0, 1);
        bossHpNowHeight = bossHpMaxHeight * bossHpNowHeightScale;
        float bossHpNowOffsetX = canvasWidth / 2 - bossHpMaxWidth / 2;
        float bossHpNowOffsetY = bossHpMaxOffsetY - (bossHpMaxHeight - bossHpNowHeight) / 2;
        bossHpNowRect.anchoredPosition = new Vector2(bossHpNowOffsetX, bossHpNowOffsetY);

        // Position & scale bossHpOld
        bossHpOldRect.anchorMin = new Vector2(0, 0.5f);
        bossHpOldRect.anchorMax = new Vector2(0, 0.5f);
        bossHpOldRect.pivot = new Vector2(0, 1);
        bossHpOldWidth = bossHpNowWidth;
        bossHpOldHeight = bossHpNowHeight;
        float bossHpOldOffsetX = bossHpNowOffsetX;
        float bossHpOldOffsetY = bossHpNowOffsetY;
        bossHpOldRect.anchoredPosition = new Vector2(bossHpOldOffsetX, bossHpOldOffsetY);
        bossHpOldRect.sizeDelta = new Vector2(bossHpOldWidth, bossHpOldHeight);

        // Position & scale bossNameText
        bossNameText.fontSize = canvasWidth * 20 / 976;
        bossNameTextRect.anchorMin = new Vector2(0, 0.5f);
        bossNameTextRect.anchorMax = new Vector2(0, 0.5f);
        bossNameTextRect.pivot = new Vector2(0, 0);
        float bossNameTextOffsetX = bossHpNowOffsetX;
        float bossNameTextOffsetY = bossHpMaxOffsetY;
        bossNameTextRect.anchoredPosition = new Vector2(bossNameTextOffsetX, bossNameTextOffsetY);

        // Position & scale potionContainer
        potionContainerRect.anchorMin = new Vector2(0, 1);
        potionContainerRect.anchorMax = new Vector2(0, 1);
        float potionContainerWidth = imageWidth;
        float potionContainerHeight = imageHeight;
        float potionContainerOffsetX = profileImageOffsetX;
        float potionContainerOffsetY = (-canvasHeight - profileImageOffsetY + potionContainerHeight) * potionContainerOffsetScaleY;
        potionContainerRect.sizeDelta = new Vector2(potionContainerWidth, potionContainerHeight);
        potionContainerRect.anchoredPosition = new Vector2(potionContainerOffsetX, potionContainerOffsetY);

        // Position & scale potionImage
        potionImageRect.anchorMin = new Vector2(0, 1);
        potionImageRect.anchorMax = new Vector2(0, 1);
        potionImageRect.pivot = new Vector2(0.5f, 0.5f);
        float potionImageWidth = potionContainerHeight * 2 / 3;
        float potionImageHeight = potionContainerHeight;
        float potionImageOffsetX = potionContainerOffsetX + potionContainerWidth / 2;
        float potionImageOffsetY = potionContainerOffsetY - potionContainerHeight / 2;
        potionImageRect.sizeDelta = new Vector2(potionImageWidth, potionImageHeight);
        potionImageRect.anchoredPosition = new Vector2(potionImageOffsetX, potionImageOffsetY);

        // Position & scale potionCount
        potionCountTextRect.anchorMin = new Vector2(0, 1);
        potionCountTextRect.anchorMax = new Vector2(0, 1);
        potionCountTextRect.pivot = new Vector2(1, 1);
        float potionCountOffsetX = potionContainerOffsetX + potionContainerWidth;
        float potionCountOffsetY = potionContainerOffsetY;
        potionCountTextRect.anchoredPosition = new Vector2(potionCountOffsetX, potionCountOffsetY);
        potionCountText.fontSize = canvasWidth * 18 / 1067;
        potionCountText.text = "0";

        setBossStage(isBossSet);

        healthManager.updateCurrentHP(nowHP, true);
        healthManager.updateCurrentSP(SP, true);
        UpdateBossNowHP(bossNowHP, true);
    }


    // Format the text showing nowHP/maxHP
    string GenerateHPText(float nowHP, float maxHP)
    {
        return "HP " + (int)nowHP + "/" + (int)maxHP;
    }

    // Update nowHP value
    public void UpdateCurrentHP()
    {
        oldHP = nowHP;
        nowHP = healthManager.getCurrentHP();
        hpText.text = GenerateHPText(nowHP, maxHP);
        hpNowWidth = canvasWidth * 1.5f * nowHP / 976 * hpWidthScale;
        hpNowRect.sizeDelta = new Vector2(hpNowWidth, hpNowHeight);
        if (nowHP < oldHP)
        {
            if (!isHPAnimPlaying)
            {
                isHPAnimPlaying = true;
                StartCoroutine(HPAnim(oldHP));
            }
        }
        else
        {
            hpOldWidth = hpNowWidth;
            hpOldRect.sizeDelta = new Vector2(hpOldWidth, hpOldHeight);
        }
    }

    IEnumerator HPAnim(float oldHP)
    {
        yield return new WaitForSeconds(hpAnimDelay);
        while (nowHP < oldHP)
        {
            oldHP -= hpAnimSpeed;
            hpOldWidth = canvasWidth * 1.5f * oldHP / 976 * hpWidthScale;
            hpOldRect.sizeDelta = new Vector2(hpOldWidth, hpOldHeight);
            yield return null;
        }
        isHPAnimPlaying = false;
    }

    // Update SP value
    public void UpdateSP()
    {
        SP = healthManager.getCurrentSP();
        sp.fillAmount = SP / maxSP;
    }

    public void UpdateBossNowHP(float value, bool isAbsolute)
    {
        bossNowHP = isAbsolute ? value : bossNowHP + value;
        bossHpNowWidth = canvasWidth * 500 / 976 * bossNowHP / bossMaxHP * bossHpWidthScale;
        bossHpNowRect.sizeDelta = new Vector2(bossHpNowWidth, bossHpNowHeight);
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
