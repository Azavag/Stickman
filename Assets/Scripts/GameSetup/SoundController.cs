using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [Header("Volume control")]
    [SerializeField] public AudioMixer mixer;
    [SerializeField] AudioMixerGroup musicMixerGroup, effectsMixerGroup, masterMixerGroup;
    float sfxVolume, musicVolume;
    [Header("All sounds")]  
    [SerializeField] Sound[] sounds;
    [Header("UI")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    [Header("References")]
    [SerializeField] GameController gameController;

    private void Awake()
    {
        transform.SetParent(null);
        foreach (Sound s in sounds)
        {                 
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.isPlayOnAwake;
            switch (s.typeOfSound)
            {
                case TypeOfSound.Music:
                    s.audioSource.outputAudioMixerGroup = musicMixerGroup;
                    break;
                case TypeOfSound.SFX:
                    s.audioSource.outputAudioMixerGroup = effectsMixerGroup;
                    break;
            }
        }
    }

    void Start()
    {
        sfxVolume = Progress.Instance.playerInfo.effectsVolume;
        sfxSlider.value = sfxVolume;
        musicVolume = Progress.Instance.playerInfo.musicVolume;
        musicSlider.value = musicVolume;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Play();
    }
    public void StopPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Stop();
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s;
    }
    //По кнопке
    public void MakeClickSound()
    {
        Play("ButtonClick");
    }

    public void SetSFXLevel()
    {      
        mixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 40);
        sfxVolume = sfxSlider.value;
        Progress.Instance.playerInfo.effectsVolume = sfxVolume;
    }
    public void SetMusicLevel()
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 40);       
        musicVolume = musicSlider.value;
        Progress.Instance.playerInfo.musicVolume = musicVolume;
    }

    //По кнопке Закрыть настройки
    public void SaveVolumeSetting()
    {
        YandexSDK.Save();
    }

    private void OnApplicationFocus(bool focus)
    {       
        Silence(!focus);
#if !UNITY_EDITOR
        if (!focus)
            gameController.SetPause();
#endif
    }
    private void OnApplicationPause(bool pause)
    {
        Silence(pause);
#if !UNITY_EDITOR
        if (pause)
            gameController.SetPause();
#endif
    }
    void Silence(bool silence)
    {
        AudioListener.pause = silence;
    }

    public void MuteGame()
    {
        AudioListener.volume = 0;
    }
    public void UnmuteGame()
    {
        AudioListener.volume = 1;
    }

}
