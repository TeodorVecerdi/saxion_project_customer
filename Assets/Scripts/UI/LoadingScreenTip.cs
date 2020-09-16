using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenTip : MonoBehaviour {
    public Transform World;
    public RectTransform UI;
    public float FadeTime = 0.5f;

    private float fadeOutPos = -485.8f;
    private float fadeNormalPos = -480f;
    private float fadeInPos = -474.2f;
    private float fadeTimer;
    private bool fadingIn, fadingOut;

    private void Update() {
        if (fadingIn) {
            fadeTimer += GameTime.UnpausedDeltaTime;
            World.localPosition = new Vector3(Mathf.Lerp(fadeInPos, fadeNormalPos, fadeTimer/FadeTime), -475.3f, 5f);
            UI.anchoredPosition = new Vector2(Mathf.Lerp(1920f, 0f, fadeTimer/FadeTime), 0f);
            if (fadeTimer >= FadeTime) {
                fadingIn = false;
                World.localPosition = new Vector3(fadeNormalPos, -475.3f, 5f);
                UI.anchoredPosition = new Vector2(0f, 0f);
            }
        }

        if (fadingOut) {
            fadeTimer += GameTime.UnpausedDeltaTime;
            World.localPosition = new Vector3(Mathf.Lerp(fadeNormalPos, fadeOutPos, fadeTimer/FadeTime), -475.3f, 5f);
            UI.anchoredPosition = new Vector2(Mathf.Lerp(0f, -1920f, fadeTimer/FadeTime), 0f);
            if (fadeTimer >= FadeTime) {
                fadingOut = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void FadeIn() {
        gameObject.SetActive(true);
        World.localPosition = new Vector3(fadeInPos, -475.3f, 5f);
        UI.anchoredPosition = new Vector2(1920f, 0f);
        fadingIn = true;
        fadeTimer = 0f;
    }

    public void FadeOut() {
        World.localPosition = new Vector3(fadeNormalPos, -475.3f, 5f);
        UI.anchoredPosition = new Vector2(0f, 0f);
        fadingOut = true;
        fadeTimer = 0f;
    }
}
