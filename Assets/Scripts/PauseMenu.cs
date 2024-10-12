using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider sliderMusic;
    public Slider sliderSound;
    public Toggle toggleSound;
    public Toggle toggleMusic;
    public bool isCameraScript = false;

    private const float _multiplier = 20f;

    public GameDataScript gameData;

    private AudioManager audioManager = AudioManager.Instance;

    private bool initComplete = false;

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    private void OnBecameVisible()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        toggleMusic.isOn = gameData.music;
        toggleSound.isOn = gameData.sound;
        sliderMusic.value = gameData.musicVolume;
        sliderMusic.interactable = toggleMusic.isOn;
        sliderSound.value = gameData.soundVolume;
        sliderSound.interactable = toggleSound.isOn;
    }

    private void Awake()
    {
        LoadSettings();
        
        sliderMusic.onValueChanged.AddListener(HandleSliderValueChanged);
        sliderSound.onValueChanged.AddListener(OnSoundSliderValueChanged);
        initComplete = true;
        
    }
    private void HandleSliderValueChanged(float value)
    {
        gameData.musicVolume = value;
        audioManager.UpdateMixerVolume();
    }

    private void OnSoundSliderValueChanged(float value)
    {
        Debug.Log(value);
        gameData.soundVolume = value;
        audioManager.UpdateMixerVolume();
    }

    public void OnTogglerMusicChanged()
    {
        if (!initComplete)
            return;
        Commands.GetMuteMusicCommand().Execute();
        
        sliderMusic.interactable = gameData.music;
    }

    public void OnTogglerSoundChanged()
    {
        if (!initComplete)
            return;
        Commands.GetMuteSoundEfectsCommand().Execute();
        sliderSound.interactable = gameData.sound;
    }

    public void OnContinueButtonClick()
    {
        this.gameObject.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void OnRestartButtonClick()
    {
        gameData.Reset();
        //gameData.Load();
        audioManager.ReStart();
        Cursor.visible = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }

    public void OnExitButtonClick()
    {
        Commands.GetExitCommand().Execute();
    }
}
