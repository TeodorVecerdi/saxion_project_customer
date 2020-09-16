using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    public Slider Music;
    public Slider SFX;
    public Slider Brightness;
    
    private void Start() {
        Music.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 0.75f));
        SFX.SetValueWithoutNotify(PlayerPrefs.GetFloat("SfxVolume", 0.75f));
        Brightness.SetValueWithoutNotify(PlayerPrefs.GetFloat("Brightness", 1f));
    }
    
    public void OnMusicValueChanged(float newValue) {
        SoundManager.MusicVolume = newValue;
    }

    public void OnSFXValueChanged(float newValue) {
        SoundManager.SfxVolume = newValue;
        SoundManager.PlaySound("click");
    }

    public void OnBrightnessValueChanged(float newValue) {
        BrightnessController.SetBrightness(newValue);
        PlayerPrefs.SetFloat("Brightness", newValue);
        PlayerPrefs.Save();
    }
}
