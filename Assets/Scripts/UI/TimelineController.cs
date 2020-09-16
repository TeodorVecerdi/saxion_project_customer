using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TimelineController : MonoBehaviour {
    [Header("Settings")]
    public float Offset = 600f;
    public bool AutoOffset;
    public float TimelineScale = 2f;
    public float FadeOffset = 25f;
    public float FadeDistance = 75f;
    public float TimelineFadeTime = 2f;
    public float MoveYearsTime = 0.5f;

    [Header("Year Settings")]
    public float TimeToYearRatio = 1f;
    public bool AutoRatio = true;
    public float StartingYear;

    [Header("References")]
    public Image TeenLine;
    public TMP_Text TeenTitle;
    public Image AdultLine;
    public TMP_Text AdultTitle;
    public TMP_Text YearsText;
    public Image Line;
    public Image DottedLine;
    public Image MiddleDot;
    public GameObject TimelineImage;

    private float timelineSize, timelineFadeTimer, moveYearsTimer;
    private bool teenFadeOut, adultFadeIn, adultFadeOut, timelineFadeOut;
    private bool moveTeen = true, moveAdult = true, moveLine, moveDottedLine, moveYears;
    private bool timelineActive = true;
    private Material dottedLineMaterial;

    private void Awake() {
        timelineSize = GetComponent<RectTransform>().sizeDelta.x;
        if (AutoOffset) {
            Offset = timelineSize / 2f;
        }

        dottedLineMaterial = new Material(DottedLine.material);
        DottedLine.material = dottedLineMaterial;
    }

    private void Start() {
        StartingYear = 2020 - Random.Range(50, 101);
        if (AutoRatio) {
            TimeToYearRatio = 100 / TurtleState.Instance.StageSettings.EssentialDeathTime;
        }
    }

    private void Update() {
        UpdateTimeline();
        UpdateYear();
    }

    private void UpdateYear() {
        YearsText.text = $"{StartingYear + TurtleState.Instance.Time * TimeToYearRatio: 0.}";
    }

    private void UpdateTimeline() {
        if (timelineActive) {
            var teenPosition = Offset + (TurtleState.Instance.StageSettings.TeenConversionTime - TurtleState.Instance.Time) * TimelineScale;
            var adultPosition = Offset + (TurtleState.Instance.StageSettings.AdultConversionTime - TurtleState.Instance.Time) * TimelineScale;

            if (teenPosition > timelineSize) {
                AdultLine.rectTransform.localScale = Vector3.zero;
                AdultTitle.rectTransform.localScale = Vector3.zero;
                teenPosition = timelineSize;
            }

            if (teenPosition < timelineSize - FadeOffset) {
                adultFadeIn = true;
            }

            if (teenPosition < Offset) {
                teenFadeOut = true;
            }

            if (adultPosition > timelineSize) {
                adultPosition = timelineSize;
            } else if (adultPosition > 0f) {
                moveLine = true;
                moveDottedLine = true;
            }

            if (adultPosition < Offset) {
                adultFadeOut = true;
            }

            if (adultFadeIn) {
                AdultLine.rectTransform.localScale = Vector3.one;
                AdultTitle.rectTransform.localScale = Vector3.one;
                var alpha = 1 - (teenPosition - (timelineSize - FadeDistance - FadeOffset)) / FadeDistance;
                if (alpha > 1f)
                    adultFadeIn = false;
                else {
                    SetAdultAlpha(alpha);
                }
            }

            if (adultFadeOut) {
                var alpha = (adultPosition - (Offset - FadeDistance - FadeOffset)) / FadeDistance;
                if (alpha < 0f) {
                    AdultLine.rectTransform.localScale = Vector3.zero;
                    AdultTitle.rectTransform.localScale = Vector3.zero;
                    adultFadeOut = false;
                    moveAdult = false;
                } else {
                    SetAdultAlpha(alpha);
                }
            }

            if (teenFadeOut) {
                var alpha = (teenPosition - (Offset - FadeDistance - FadeOffset)) / FadeDistance;
                if (alpha < 0f) {
                    TeenLine.rectTransform.localScale = Vector3.zero;
                    TeenTitle.rectTransform.localScale = Vector3.zero;
                    teenFadeOut = false;
                    moveTeen = false;
                } else {
                    SetTeenAlpha(alpha);
                }
            }

            if (moveTeen) {
                TeenLine.rectTransform.anchoredPosition = new Vector2(teenPosition, 0);
                TeenTitle.rectTransform.anchoredPosition = new Vector2(teenPosition, -32);
            }

            if (moveAdult) {
                AdultLine.rectTransform.anchoredPosition = new Vector2(adultPosition, 0);
                AdultTitle.rectTransform.anchoredPosition = new Vector2(adultPosition, -32);
            }

            if (moveLine) {
                Line.rectTransform.anchoredPosition = new Vector2(adultPosition, 0);
                if (adultPosition < 0)
                    moveLine = false;
            }

            if (moveDottedLine) {
                DottedLine.rectTransform.anchoredPosition = new Vector2(adultPosition, 0);
                if (adultPosition < 0) {
                    moveDottedLine = false;
                    dottedLineMaterial.SetFloat("_TimeOffset", Time.time);
                    dottedLineMaterial.SetFloat("_TimeScale", TurtleState.Instance.TimeSpeed / TimelineScale / 2f);
                    DottedLine.enabled = false;
                    DottedLine.enabled = true;

                    // Start fading timeline
                    timelineFadeOut = true;
                    timelineFadeTimer = 0f;
                }
            }

            if (timelineFadeOut) {
                timelineFadeTimer += GameTime.DeltaTime;
                var targetAlpha = 1f - Mathf.Lerp(0f, 1f, timelineFadeTimer / TimelineFadeTime);
                if (timelineFadeTimer >= TimelineFadeTime) {
                    timelineFadeOut = false;
                    targetAlpha = 0f;
                    
                    timelineActive = false;
                    TimelineImage.SetActive(false);
                    
                    moveYears = true;
                    moveYearsTimer = 0f;
                }

                var dottedLineColor = DottedLine.color;
                var middleDotColor = MiddleDot.color;
                
                dottedLineColor.a = targetAlpha;
                middleDotColor.a = targetAlpha;
                
                DottedLine.color = dottedLineColor;
                MiddleDot.color = middleDotColor;
            }
        }

        if (moveYears) {
            moveYearsTimer += GameTime.DeltaTime;
            var targetY = Mathf.Lerp(-64f, 0f, moveYearsTimer / MoveYearsTime);
            if (moveYearsTimer >= MoveYearsTime) {
                moveYears = false;
                targetY = 0f;
            }

            YearsText.rectTransform.anchoredPosition = new Vector2(0f, targetY);
        }
    }

    private void SetAdultAlpha(float alpha) {
        var lineColor = AdultLine.color;
        lineColor.a = alpha;
        AdultLine.color = lineColor;

        var titleColor = AdultTitle.color;
        titleColor.a = alpha;
        AdultTitle.color = titleColor;
    }

    private void SetTeenAlpha(float alpha) {
        var lineColor = TeenLine.color;
        lineColor.a = alpha;
        TeenLine.color = lineColor;

        var titleColor = TeenTitle.color;
        titleColor.a = alpha;
        TeenTitle.color = titleColor;
    }
}