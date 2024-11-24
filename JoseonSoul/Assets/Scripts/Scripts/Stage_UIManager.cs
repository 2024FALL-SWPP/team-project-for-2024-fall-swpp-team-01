using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stage_UIManager : MonoBehaviour
{
    // public variables that MUST be assigned objects before playing
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

    [Header("Player")]
    [SerializeField] private Transform player;
    private PlayerHealthManager healthManager;

    [Header("Strings for message")]
    public String wellString = "우물 정화 : E";
    public String nextStageString = "이동 : E";
    public String saveString = "저장 및 회복 : E";

    void Start()
    {
        healthManager = player.gameObject.GetComponent<PlayerHealthManager>();
        if(healthManager == null)
            Debug.LogError("Player Health Manager Not Detected");
        maxHP = healthManager.getMaxHP();
        maxSP = healthManager.getMaxSP();
        
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


        healthManager.updateMaxHP(maxHP, true);
        healthManager.updateCurrentHP(nowHP, true);
        healthManager.updateCurrentSP(SP, true);

        // 인터랙션 텍스트 초기 설정 (하단 중앙에 위치)
        interactionText.rectTransform.anchorMin = new Vector2(0.5f, 0f);  // X축 중앙, Y축 하단
        interactionText.rectTransform.anchorMax = new Vector2(0.5f, 0f);  // X축 중앙, Y축 하단
        interactionText.rectTransform.anchoredPosition = new Vector2(0, 100);  // 하단에서 50px 위쪽
        interactionText.gameObject.SetActive(false);  // 초기에는 텍스트 숨기기
    }

    void Awake()
    {
    }

    void Update()
    {
        
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
        SP = healthManager.getCurrentSP();
        sp.fillAmount = SP / 100;
    }

        // OnTriggerEnter 함수 추가
    void OnTriggerEnter(Collider other)
    {
        // 상호작용할 오브젝트와 충돌했을 때
        if (other.CompareTag("Well"))  
        {
            interactionText.text = wellString;  // 텍스트 변경
            interactionText.gameObject.SetActive(true);  // 텍스트 활성화
        }

        if (other.CompareTag("Fire"))  
        {
            interactionText.text = saveString;  
            interactionText.gameObject.SetActive(true);
        }

        if (other.CompareTag("NextStage"))  
        {
            interactionText.text = nextStageString;  
            interactionText.gameObject.SetActive(true);
        }
    }

    // OnTriggerExit 함수 추가
    void OnTriggerExit(Collider other)
    {
        // 상호작용할 오브젝트에서 벗어났을 때
        if (other.CompareTag("Well") || other.CompareTag("Fire") || other.CompareTag("NextStage"))
        {
            interactionText.gameObject.SetActive(false);  // 텍스트 비활성화
        }
    }

}
