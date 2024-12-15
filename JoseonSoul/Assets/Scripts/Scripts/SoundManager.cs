using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip titleSceneMusic;
    public AudioClip stage1Music;
    public AudioClip stage2Music;
    public AudioClip bossStageMusic;

    public AudioClip walkingSound;
    public AudioClip gameOverSound;
    public List<AudioClip> knifeSounds;
    public List<AudioClip> bodyPunchedSounds;

    public static SoundManager Instance { get; private set; }

    private AudioSource BGMAudioSource;
    private List<AudioSource> SFXAudioSources = new List<AudioSource>();
    private int SFXSourcesNum = 5; // 0: Walking, 1: GameOver, 2: Knife, 3: Attacked, ...

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the SoundManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate SoundManager objects
        }

        BGMAudioSource = gameObject.AddComponent<AudioSource>();
        for (int i = 0; i < SFXSourcesNum; i++)
        {
            SFXAudioSources.Add(gameObject.AddComponent<AudioSource>());
        }

        SFXAudioSources[0].clip = walkingSound;
        SFXAudioSources[1].clip = gameOverSound;
    }

    void Start()
    {

    }

    public void SetBgm(int sceneId)
    {
        Debug.Log(sceneId);
        switch (sceneId)
        {
            case -2: // Stop the bgm
                BGMAudioSource.Stop();
                break;

            case -1: // Title Scene
                BGMAudioSource.clip = titleSceneMusic;
                BGMAudioSource.loop = true;
                BGMAudioSource.Play();
                break;

            case 1: // Stage 1 Scene 1
                BGMAudioSource.clip = stage1Music;
                BGMAudioSource.loop = true;
                BGMAudioSource.Play();
                break;

            case 2: // Stage 1 Scene 2
                break;

            case 3: // Stage 2
                BGMAudioSource.clip = stage2Music;
                BGMAudioSource.loop = true;
                BGMAudioSource.Play();
                break;

            case 4: // Boss Stage
                BGMAudioSource.clip = bossStageMusic;
                BGMAudioSource.loop = true;
                BGMAudioSource.Play();
                break;
        }
    }

    public void SetRunning(bool isRunning)
    {
        if (isRunning && !SFXAudioSources[0].isPlaying)
        {
            SFXAudioSources[0].loop = true;
            SFXAudioSources[0].Play();
        }
        else if (!isRunning & SFXAudioSources[0].isPlaying)
        {
            SFXAudioSources[0].Stop();
        }
    }

    public void SetGameOver()
    {
        SFXAudioSources[1].Play();
        SetBgm(-2);
        SFXAudioSources[0].Stop();
    }

    public void SetKnifeSound()
    {
        SFXAudioSources[2].clip = knifeSounds[Random.Range(0, knifeSounds.Count)];
        SFXAudioSources[2].Play();
    }

    public void SetAttacked()
    {
        SFXAudioSources[3].clip = bodyPunchedSounds[Random.Range(0, bodyPunchedSounds.Count)];
        SFXAudioSources[3].Play();
    }
}
