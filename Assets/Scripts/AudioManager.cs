using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager: MonoBehaviour
{
    public GameDataScript gameData;

    public static AudioManager Instance = null;
    [SerializeField]
    private Sound[] sounds;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;

    private void Awake()
    {
        if (Instance == null)
        { // Ёкземпл€р менеджера был найден
            Instance = this; // «адаем ссылку на экземпл€р объекта
        }
        else if (Instance == this)
        { // Ёкземпл€р объекта уже существует на сцене
            Destroy(gameObject); // ”дал€ем объект
        }
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    private void Initialize()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.audioType)
            {
                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    //s.source.volume = gameData.soundVolume;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    //s.source.volume = gameData.musicVolume;
                    break;
            }

           
        }
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    //foreach (Sound s in sounds)
    //    //{
    //    //    if (s.playOnAwake)
    //    //        s.source.Play();
    //    //}
    //    //UpdateMixerVolume();
    //}

    public void ReStart()
    {
        UpdateMixerVolume();
        foreach (Sound s in sounds)
        {
            if (s.playOnAwake)
            {
                s.source.Play();
            }
                
        }
        

    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == soundName);
        if (s == null)
        {
            throw new KeyNotFoundException($"Sound: {soundName} does NOT exist!");
        }
        s.source.Play();
    }

    public void MuteMusic()
    {
        //musicMixerGroup.audioMixer.SetFloat("MusicVolume", -80f);
        gameData.music = !gameData.music;
        foreach (Sound s in sounds)
        {
            if (s.audioType == Sound.AudioTypes.music)
            {
                s.source.mute = !gameData.music;
                //if (s.source.mute)
                //{
                //    s.source.Stop();
                //}
            }
        }
    }

    public void MuteSoundEffect()
    {
        gameData.sound = !gameData.sound;
        //soundEffectsMixerGroup.audioMixer.SetFloat("SoundEffectsVolume", 0f);
        foreach (Sound s in sounds)
        {
            if (s.audioType == Sound.AudioTypes.soundEffect)
            {
                if (!gameData.sound)
                {
                    s.source.volume = 0.0001f;
                }
                else
                {
                    s.source.volume = 1.0f;
                }
                
                //if (s.source.mute)
                //{
                //    s.source.Stop();
                //}
            }
        }
    }

    public void UpdateMixerVolume()
    {
        //Debug.Log($"MUSIC VOLUME {gameData.musicVolume} {Mathf.Log10(gameData.musicVolume) * 20}");
        //Debug.Log($"SOUND EFFECTS VOLUME {gameData.soundVolume}");
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(gameData.musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(gameData.soundVolume) * 20);
    }
}
