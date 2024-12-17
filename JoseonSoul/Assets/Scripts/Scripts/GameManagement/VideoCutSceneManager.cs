using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoCutSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer 컴포넌트

    public string nextSceneName; // 로드할 다음 씬 이름

    private void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // VideoPlayer가 끝날 때 호출될 이벤트 설정
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameManager.Instance.NextScene();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        GameManager.Instance.NextScene();
    }
}
