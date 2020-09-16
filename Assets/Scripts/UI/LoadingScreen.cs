using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
    public Image LoadingIcon;
    public Image LoadingFill;
    public TMP_Text LoadingDoneText;
    public List<LoadingScreenTip> Tips;
    public float TipTime = 4f;
    public float FadeTime = 0.5f;

    private int currentTip;
    private bool isLoadingDone;
    private bool isFading;
    private float fadeTimer;
    private float tipTimer;

    private void Start() {
        StartCoroutine(LoadScene());
        GameTime.IsPaused = true;
        Tips[0].gameObject.SetActive(true);
    }

    private void Update() {
        if (isFading) {
            fadeTimer += GameTime.UnpausedDeltaTime;
            var colorOut = LoadingIcon.color;
            var colorIn = LoadingDoneText.color;
            colorOut.a = 1f - fadeTimer / FadeTime;
            colorIn.a = fadeTimer / FadeTime;

            if (fadeTimer >= FadeTime) {
                isLoadingDone = true;
                isFading = false;
                colorOut.a = 0f;
                colorIn.a = 1f;
            }

            LoadingIcon.color = colorOut;
            LoadingFill.color = colorOut;
            LoadingDoneText.color = colorIn;
        }

        tipTimer += GameTime.UnpausedDeltaTime;
        if (tipTimer > TipTime) {
            NextTip();
        }
    }

    private IEnumerator LoadScene() {
        yield return null;
        var sceneLoading = SceneManager.LoadSceneAsync("Game");
        sceneLoading.allowSceneActivation = false;
        while (!sceneLoading.isDone) {
            if (!isLoadingDone) {
                LoadingFill.fillAmount = Mathf.Lerp(LoadingFill.fillAmount, sceneLoading.progress / 0.9f, 0.05f);
                if (sceneLoading.progress >= 0.9f && !isFading) {
                    LoadingFill.fillAmount = 1f;
                    isFading = true;
                    fadeTimer = 0f;
                }
            } else {
                if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2)) {
                    GameTime.IsPaused = false;
                    SoundManager.PlaySound("start_game");
                    SoundManager.PauseSound("theme_menu");
                    SoundManager.PlaySound("theme_game", skipIfAlreadyPlaying: true);
                    sceneLoading.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    private void NextTip() {
        tipTimer = 0f;
        Tips[currentTip].FadeOut();
        currentTip = (currentTip + 1) % Tips.Count;
        Tips[currentTip].FadeIn();
    }
}