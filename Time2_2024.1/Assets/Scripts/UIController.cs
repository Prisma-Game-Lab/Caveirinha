using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    //Comentario
    public Slider _musicSlider, _sfxSlider;
    public AudioSource music, sfx;

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(_sfxSlider.value);
    }

    private void Start()
    {
        _musicSlider.value = AudioManager.instance.musicSource.volume;
        _sfxSlider.value = AudioManager.instance.sfxSource.volume;
    }

    private void Update()
    {
        MusicVolume();
        SFXVolume();
    }
}
