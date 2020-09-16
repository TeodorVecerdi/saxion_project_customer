using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour {
    private static BrightnessController instance;
    public float MinBrightnessAlpha = 0.7f;
    
    private Image brightnessOverlay;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        brightnessOverlay = GetComponent<Image>();
        SetBrightness(PlayerPrefs.GetFloat("Brightness", 1f));
    }

    public static void SetBrightness(float brightness) {
        var color = instance.brightnessOverlay.color;
        color.a = instance.MinBrightnessAlpha * (1 - brightness);
        instance.brightnessOverlay.color = color;
    }
}
