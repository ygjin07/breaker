using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;
    public Slider musicSlider;
    public Slider btnSlider;
    public float Vol = 1f;
    public float Vol2 = 1f;

    public void Start()
    {
        Vol = PlayerPrefs.GetFloat("Vol", 1f);
        musicSlider.value = Vol;
        musicsource.volume = musicSlider.value;
        Vol2 = PlayerPrefs.GetFloat("Vol2", 1f);
        btnSlider.value = Vol2;
        btnsource.volume = btnSlider.value;
    }

    public void SetMusicVolume(float volume)
    {
        musicsource.volume = musicSlider.value;
        Vol = musicSlider.value;
        PlayerPrefs.SetFloat("Vol", Vol);
    }

    public void SetButtonVolume(float volume)
    {
        btnsource.volume =btnSlider.value;
        Vol2 = btnSlider.value;
        PlayerPrefs.SetFloat("Vol2", Vol2);
    }

    public void OnSfx()
    {
        btnsource.Play();
    }
}
