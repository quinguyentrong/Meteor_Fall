//using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MoreMountains.NiceVibrations;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class SoundManager: MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        SetBGMusicState();
        SetSFXState();
        SetHapticState();
    }

    #region LOAD BG MUSIC
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case GameConfig.HOME_SCENE:
                BGAudioSource.clip = Resources.Load<AudioClip>("Background Music/MainBG");
                BGAudioSource.Play();
                break;
        }
    }
    #endregion LOAD BG MUSIC


    #region Background Music
    [SerializeField] private AudioSource BGAudioSource;
    private const string BGMusicSetting = "BGMusicSetting";
    public bool IsBGMusicOn { get; private set; } = true;
    private void SetBGMusicState()
    {
        BGAudioSource.Play();
        if (PlayerPrefs.HasKey(BGMusicSetting) == false)
        {
            PlayerPrefs.SetInt(BGMusicSetting, 1);
        }
        IsBGMusicOn = PlayerPrefs.GetInt(BGMusicSetting, 0) == 1;
        if (IsBGMusicOn == false)
        {
            BGAudioSource.mute = true;
        }
        else
        {
            BGAudioSource.mute = false;
        }
    }
    /// <summary>
    /// Bật tắt background music
    /// </summary>
    public void SwitchBGSound()
    {
        IsBGMusicOn = !IsBGMusicOn;
        if (IsBGMusicOn)
        {
            BGAudioSource.mute = false;
        }
        else
        {
            BGAudioSource.mute = true;
        }

        PlayerPrefs.SetInt(BGMusicSetting, IsBGMusicOn ? 1 : 0);
    }
    #endregion Background Music

    #region SOUND EFFECT
    [SerializeField] private AudioSource SFXAudioSource;
    private const string SFXSetting = "SFXSetting";
    public bool IsFXMusicOn { get; private set; } = true;

    private void SetSFXState()
    {
        if (PlayerPrefs.HasKey(SFXSetting) == false)
        {
            PlayerPrefs.SetInt(SFXSetting, 1);
        }
        IsFXMusicOn = PlayerPrefs.GetInt(SFXSetting, 0) == 1;
        if (IsFXMusicOn)
        {
            SFXAudioSource.mute = false;
        }
        else
        {
            SFXAudioSource.mute = true;
        }
    }
    /// <summary>
    /// Bật tắt sound effect
    /// </summary>
    public void SwitchFXSound()
    {
        IsFXMusicOn = !IsFXMusicOn;
        if (IsFXMusicOn)
        {
            SFXAudioSource.mute = false;
        }
        else
        {
            SFXAudioSource.mute = true;
        }
        PlayerPrefs.SetInt(SFXSetting, IsFXMusicOn ? 1 : 0);
    }
    public void PlayEffect(AudioClip clip, float pitch = 1, float volume = 1)
    {
        if (clip == null)
        {
            Debug.LogError("null sound");
            return;
        }
        SFXAudioSource.pitch = pitch;
        SFXAudioSource.PlayOneShot(clip, volume);
    }

    public void PlayEffectWithDelay(AudioClip clip, Action CallbackAction = null, float delayTime = 0.25f, float pitch = 1, float volume = 1)
    {
        if (CallbackAction != null)
        {
            DOVirtual.DelayedCall(delayTime, () => CallbackAction());
        }
        PlayEffect(clip, pitch, volume);
    }
    #endregion SOUND EFFECT

    #region Haptic
    private const string HapticSetting = "HapticSetting";
    public bool IsHapticOn { get; private set; } = true;
    private void SetHapticState()
    {
        if (PlayerPrefs.HasKey(HapticSetting) == false)
        {
            PlayerPrefs.SetInt(HapticSetting, 1);
        }
        IsHapticOn = PlayerPrefs.GetInt(HapticSetting, 0) == 1;
    }
    /// <summary>
    /// Bật tắt rung
    /// </summary>
    public void SwitchHaptic()
    {
        IsHapticOn = !IsHapticOn;

        PlayerPrefs.SetInt(HapticSetting, IsHapticOn ? 1 : 0);
    }
    public void Vibrate()
    {
        IsHapticOn = PlayerPrefs.GetInt(HapticSetting, 0) == 1;
        if (IsHapticOn)
        {
            //MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
    }
    #endregion Haptic
}
