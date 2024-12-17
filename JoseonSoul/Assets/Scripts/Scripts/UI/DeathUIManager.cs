using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathUIManager : MonoBehaviour
{
    public Image deathImage;
    public TextMeshProUGUI countdownText;
    public Canvas canvas;
    private bool isCountingDown = false;
    public static DeathUIManager Instance { get; private set; }
    

    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

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

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(canvas);
    }

    public void ShowDeathUI()
    {
        if (!isCountingDown)
        {
            canvas.gameObject.SetActive(true);
            StartCoroutine(CountdownToRestart());
        }
    }

    private IEnumerator CountdownToRestart()
    {
        isCountingDown = true;
        
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = $"{i}초 후 다시 시작됩니다";
            yield return new WaitForSeconds(1f);
        }

        // UI 숨기기
        canvas.gameObject.SetActive(false);
        isCountingDown = false;
    }
}
