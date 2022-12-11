using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> Source;
    [SerializeField]
    private List<AudioClip> BackgroundClip;
    [SerializeField]
    private List<AudioClip> EffectClip;

    public static SoundManager instance = null;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Start()
    {
        SetBackgroundSource();
        GameManager.instance.AddSceneMoveEvent(SetBackgroundSource);
    }

    private void SetBackgroundSource()
    {
        if (SceneManager.GetActiveScene().name == "Town")
        {
            Source[(int)UISoundOrder.Background].clip = BackgroundClip[0];
        }
        if (SceneManager.GetActiveScene().name == "Home")
        {
            Source[(int)UISoundOrder.Background].clip = BackgroundClip[1];
        }
        else if(SceneManager.GetActiveScene().name == "JustChat")
        {
            Source[(int)UISoundOrder.Background].clip = BackgroundClip[2];
        }
        else if (SceneManager.GetActiveScene().name == "Town" || SceneManager.GetActiveScene().name == "TestScene")
        {
            Source[(int)UISoundOrder.Background].clip = BackgroundClip[3];
        }

        Source[(int)UISoundOrder.Background].Play();
    }

    public void MuteAudioSource(UISoundOrder Type, bool IsMute)
    {
        Source[(int)Type].mute = IsMute;
        if (IsMute)
        {
            Source[(int)Type].Pause();
        }
        else
        {
            Source[(int)Type].UnPause();
        }
    }

    public void VolumeAudioSource(UISoundOrder Type, float Volume)
    {
        Source[(int)Type].volume = Volume;
    }

    public bool SourceMute(UISoundOrder Type)
    {
        return Source[(int)Type].mute;
    }

    public float SourceVolume(UISoundOrder Type)
    {
        return Source[(int)Type].volume;
    }
}
