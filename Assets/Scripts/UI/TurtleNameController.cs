using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurtleNameController : MonoBehaviour {
    public float AnimationDuration = 0.5f;
    public Image FillImage;
    public float MinAlpha = 0.8f;

    private TMP_InputField inputField;
    private bool wasFocusedLastFrame;
    private bool isFilledIn;
    private float fillTimer;
    private float alphaTimer;
    private bool isFillingIn, isFillingOut, isAlphaIncreasing, isAlphaDecreasing;

    private void Awake() {
        inputField = GetComponent<TMP_InputField>();
    }

    private void Start() {
        inputField.text = PlayerPrefs.GetString("name");
    }

    private void Update() {
        UpdateGraphics();
        ProcessInput();
    }

    private void UpdateGraphics() {
        if (isFillingIn) {
            fillTimer += GameTime.UnpausedDeltaTime;
            FillImage.fillAmount = fillTimer / AnimationDuration;
            if (fillTimer >= AnimationDuration) {
                isFillingIn = false;
                fillTimer = 0f;
                FillImage.fillAmount = 1f;
            }
        }

        if (isFillingOut) {
            fillTimer += GameTime.UnpausedDeltaTime;
            FillImage.fillAmount = 1f - fillTimer / AnimationDuration;
            if (fillTimer >= AnimationDuration) {
                isFillingOut = false;
                fillTimer = 0f;
                FillImage.fillAmount = 0f;
            }
        }

        if (isAlphaIncreasing) {
            alphaTimer += GameTime.UnpausedDeltaTime;
            var color = FillImage.color;
            color.a = MinAlpha + (1 - MinAlpha) * alphaTimer / AnimationDuration;
            FillImage.color = color;
            if (alphaTimer >= AnimationDuration) {
                isAlphaIncreasing = false;
                alphaTimer = 0f;

                color.a = 1f;
                FillImage.color = color;
            }
        }

        if (isAlphaDecreasing) {
            alphaTimer += GameTime.UnpausedDeltaTime;
            var color = FillImage.color;
            color.a = MinAlpha + (1 - MinAlpha) * (1 - alphaTimer / AnimationDuration);
            FillImage.color = color;
            if (alphaTimer >= AnimationDuration) {
                isAlphaDecreasing = false;
                alphaTimer = 0f;

                color.a = MinAlpha;
                FillImage.color = color;
            }
        }
    }

    private void ProcessInput() {
        if (inputField.isFocused && !wasFocusedLastFrame) {
            if(!isFilledIn) FillIn();
            IncreaseAlpha();
        } else if (!inputField.isFocused && wasFocusedLastFrame) {
            if(isFilledIn) FillOut();
            DecreaseAlpha();
        }

        wasFocusedLastFrame = inputField.isFocused;
    }

    public void UpdateName(string newName) {
        PlayerPrefs.SetString("name", string.IsNullOrEmpty(newName) ? "Unnamed Turtle" : newName);
        PlayerPrefs.Save();
    }

    public void OnPointerEnter() {
        if (isFilledIn) return;
        FillIn();
    }

    public void OnPointerExit() {
        if (inputField.isFocused) return;
        FillOut();
    }

    private void FillIn() {
        isFillingIn = true;
        isFilledIn = true;
        if (isFillingOut) {
            isFillingOut = false;
            fillTimer = AnimationDuration - fillTimer;
        }
        else fillTimer = 0f;
    }

    private void FillOut() {
        isFillingOut = true;
        isFilledIn = false;
        if (isFillingIn) {
            isFillingIn = false;
            fillTimer = AnimationDuration - fillTimer;
        }
        else fillTimer = 0f;
    }

    private void IncreaseAlpha() {
        isAlphaIncreasing = true;
        if (isAlphaDecreasing)
            isAlphaDecreasing = false;
        else alphaTimer = 0f;
    }

    private void DecreaseAlpha() {
        isAlphaDecreasing = true;
        if (isAlphaIncreasing)
            isAlphaIncreasing = false;
        else alphaTimer = 0f;
    }
}