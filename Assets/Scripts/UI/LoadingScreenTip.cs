using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingScreenTip : MonoBehaviour {
    [Serializable] public class OnTipActivateEvent : UnityEvent {}
    
    public Transform World;
    public RectTransform UI;
    public float FadeTime = 0.5f;
    public OnTipActivateEvent OnActivate;

    private float fadeOutPos = -5.6f;
    private float fadeNormalPos = 0f;
    private float fadeInPos = 5.6f;
    private float fadeTimer;
    private bool fadingIn, fadingOut;

    private void Update() {
        if (fadingIn) {
            fadeTimer += GameTime.UnpausedDeltaTime;
            World.localPosition = new Vector3(Mathf.Lerp(fadeInPos, fadeNormalPos, fadeTimer/FadeTime), -0.3f, 5f);
            UI.anchoredPosition = new Vector2(Mathf.Lerp(1920f, 0f, fadeTimer/FadeTime), 0f);
            if (fadeTimer >= FadeTime) {
                fadingIn = false;
                World.localPosition = new Vector3(fadeNormalPos, -0.3f, 5f);
                UI.anchoredPosition = new Vector2(0f, 0f);
            }
        }

        if (fadingOut) {
            fadeTimer += GameTime.UnpausedDeltaTime;
            World.localPosition = new Vector3(Mathf.Lerp(fadeNormalPos, fadeOutPos, fadeTimer/FadeTime), -0.3f, 5f);
            UI.anchoredPosition = new Vector2(Mathf.Lerp(0f, -1920f, fadeTimer/FadeTime), 0f);
            if (fadeTimer >= FadeTime) {
                fadingOut = false;
                gameObject.SetActive(false);
                World.gameObject.SetActive(false);
            }
        }
    }

    public void Enable() {
        gameObject.SetActive(true);
        World.gameObject.SetActive(true);
        OnActivate?.Invoke();
    }

    public void FadeIn() {
        gameObject.SetActive(true);
        World.gameObject.SetActive(true);
        World.localPosition = new Vector3(fadeInPos, -0.3f, 5f);
        UI.anchoredPosition = new Vector2(1920f, 0f);
        fadingIn = true;
        fadeTimer = 0f;
        OnActivate?.Invoke();
    }

    public void FadeOut() {
        World.localPosition = new Vector3(fadeNormalPos, -0.3f, 5f);
        UI.anchoredPosition = new Vector2(0f, 0f);
        fadingOut = true;
        fadeTimer = 0f;
    }
}
