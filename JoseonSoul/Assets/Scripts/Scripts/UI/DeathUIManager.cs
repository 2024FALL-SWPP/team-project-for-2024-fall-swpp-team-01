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

    void Start()
    {
        // UI 요소들을 시작할 때 숨김
        deathImage.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(canvas);
    }

    public void ShowDeathUI()
    {
        if (!isCountingDown)
        {
            deathImage.gameObject.SetActive(true);
            countdownText.gameObject.SetActive(true);
            StartCoroutine(CountdownToRestart());
        }
    }

    private IEnumerator CountdownToRestart()
    {
        isCountingDown = true;
        
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = $"To be continued in {i} sec...";
            yield return new WaitForSeconds(1f);
        }

        // UI 숨기기
        deathImage.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        isCountingDown = false;
    }
}
