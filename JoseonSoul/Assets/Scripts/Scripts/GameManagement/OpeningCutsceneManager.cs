using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OpeningCutsceneManager : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private Image backgroundImage; // 검은 배경
    [SerializeField] private Image displayImage;    // 컷신 이미지
    [SerializeField] private TextMeshProUGUI subtitleText;
    
    [Header("Content")]
    [SerializeField] private Sprite[] openingImages;
    [SerializeField] private string[] subtitles;
    
    [Header("Settings")]
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float subtitleFadeTime = 0.5f;
    
    private int currentImageIndex = 0;
    private bool isTransitioning = false;
    
    private void Start()
    {
        // 배경 이미지를 검은색으로 설정
        backgroundImage.color = Color.black;
        
        // 이미지 AspectRatioFitter 설정
        SetupImageAspectRatio();
        
        // 초기 이미지와 자막 설정
        if (openingImages.Length > 0)
        {
            displayImage.sprite = openingImages[0];
            if (subtitles.Length > 0)
            {
                subtitleText.text = subtitles[0];
            }
        }
    }
    
    private void SetupImageAspectRatio()
    {
        // AspectRatioFitter가 없다면 추가
        if (!displayImage.GetComponent<AspectRatioFitter>())
        {
            AspectRatioFitter fitter = displayImage.gameObject.AddComponent<AspectRatioFitter>();
            fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            fitter.aspectRatio = 16f / 9f; // 기본값, 실제 이미지 비율에 따라 업데이트됨
        }
    }
    
    private void UpdateAspectRatio(Sprite sprite)
    {
        if (sprite != null)
        {
            AspectRatioFitter fitter = displayImage.GetComponent<AspectRatioFitter>();
            if (fitter != null)
            {
                fitter.aspectRatio = sprite.rect.width / sprite.rect.height;
            }
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTransitioning)
        {
            if (currentImageIndex < openingImages.Length - 1)
            {
                StartCoroutine(TransitionToNextImage());
            }
            else
            {
                EndCutscene();
            }
        }
    }
    
    private IEnumerator TransitionToNextImage()
    {
        isTransitioning = true;
        
        // 현재 이미지 페이드 아웃
        float elapsedTime = 0f;
        Color imageColor = displayImage.color;
        Color textColor = subtitleText.color;
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - (elapsedTime / fadeTime);
            
            imageColor.a = alpha;
            displayImage.color = imageColor;
            
            textColor.a = alpha;
            subtitleText.color = textColor;
            
            yield return null;
        }
        
        // 다음 이미지로 변경
        currentImageIndex++;
        displayImage.sprite = openingImages[currentImageIndex];
        UpdateAspectRatio(openingImages[currentImageIndex]); // 새 이미지의 비율 적용
        
        if (currentImageIndex < subtitles.Length)
        {
            subtitleText.text = subtitles[currentImageIndex];
        }
        
        // 새 이미지 페이드 인
        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeTime;
            
            imageColor.a = alpha;
            displayImage.color = imageColor;
            
            textColor.a = alpha;
            subtitleText.color = textColor;
            
            yield return null;
        }
        
        isTransitioning = false;
    }
    
    private void EndCutscene()
    {
        Debug.Log("Cutscene ended");
        GameManager.Instance.NextScene();
    }
}