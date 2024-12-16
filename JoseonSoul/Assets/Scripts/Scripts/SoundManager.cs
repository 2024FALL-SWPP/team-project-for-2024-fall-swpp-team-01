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
    public List<AudioClip> growlingSounds;
    public List<AudioClip> longRoarSounds;
    public List<AudioClip> shortRoarSounds;
    public List<AudioClip> shieldBlockSounds;

    public static SoundManager Instance { get; private set; }

    private AudioSource BGMAudioSource;
    private List<AudioSource> SFXAudioSources = new List<AudioSource>();
    private int SFXSourcesNum = 8; // 0: Walking, 1: GameOver, 2: Knife, 3: Attacked, 4 : Growling, 5 : Long Roar, 6 : Short Roar, 7 : Shield Block

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
        switch (sceneId)
        {
            case -2: // Stop the bgm
                BGMAudioSource.Stop();
                break;

            case -1: // Title Scene
                BGMAudioSource.clip = titleSceneMusic;
                BGMAudioSource.loop = true;
                BGMAudioSource.volume = 1.0f;
                BGMAudioSource.Play();
                break;

            case 1: // Stage 1 Scene 1
                BGMAudioSource.clip = stage1Music;
                BGMAudioSource.loop = true;
                BGMAudioSource.volume = 0.3f;
                BGMAudioSource.Play();
                break;

            case 2: // Stage 1 Scene 2
                break;

            case 3: // Stage 2
                BGMAudioSource.clip = stage2Music;
                BGMAudioSource.volume = 0.3f;
                BGMAudioSource.loop = true;
                BGMAudioSource.Play();
                break;

            case 4: // Boss CutScene
                BGMAudioSource.clip = bossStageMusic;
                BGMAudioSource.volume = 0.5f;
                BGMAudioSource.loop = true;
                BGMAudioSource.Play();
                break;

            case 6: // Ending CutScene
                BGMAudioSource.clip = titleSceneMusic;
                BGMAudioSource.volume = 1.0f;
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
            SFXAudioSources[0].volume = 0.3f;
            SFXAudioSources[0].Play();
        }
        else if (!isRunning && SFXAudioSources[0].isPlaying)
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
        SFXAudioSources[2].volume = 0.6f;
        SFXAudioSources[2].Play();
    }

    public void SetAttacked()
    {
        SFXAudioSources[3].clip = bodyPunchedSounds[Random.Range(0, bodyPunchedSounds.Count)];
        SFXAudioSources[3].volume = 0.4f;
        SFXAudioSources[3].Play();
    }

    public void SetGrowlingSound()
    {
        SFXAudioSources[4].clip = growlingSounds[Random.Range(0, growlingSounds.Count)];
        SFXAudioSources[4].volume = 0.4f;
        SFXAudioSources[4].Play();
    }

    public void SetLongRoarSound()
    {
        SFXAudioSources[5].clip = longRoarSounds[Random.Range(0, longRoarSounds.Count)];
        SFXAudioSources[5].volume = 0.7f;
        SFXAudioSources[5].Play();
    }

    public void SetShortRoarSound()
    {
        SFXAudioSources[6].clip = shortRoarSounds[Random.Range(0, shortRoarSounds.Count)];
        SFXAudioSources[6].volume = 0.4f;
        SFXAudioSources[6].Play();
    }

    public void SetShieldBlockSound()
    {
        SFXAudioSources[7].clip = shieldBlockSounds[Random.Range(0, shieldBlockSounds.Count)];
        SFXAudioSources[7].volume = 1.0f;
        SFXAudioSources[7].Play();
    }
}
