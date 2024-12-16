using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneCameraController : MonoBehaviour
{
    public static CutSceneCameraController Instance { get; private set; }
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
