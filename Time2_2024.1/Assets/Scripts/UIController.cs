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
        music.volume = _musicSlider.value;
        
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(_sfxSlider.value);
        sfx.volume = _sfxSlider.value;
    }

    private void Update()
    {
        MusicVolume();
        SFXVolume();
    }
}
