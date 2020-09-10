using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour {
    [Header("Settings")]
    public float Offset = 600f;
    public bool AutoOffset;
    public float TimelineScale = 2f;
    public float FadeOffset = 25f;
    public float FadeDistance = 75f;

    [Header("References")]
    public Image TeenLine;
    public TMP_Text TeenTitle;
    public Image AdultLine;
    public TMP_Text AdultTitle;
    public TMP_Text YearsText;
    public Image Line;
    public Image DottedLine;

    private float timelineSize;
    private bool teenFadeOut, adultFadeIn, adultFadeOut;
    private bool moveTeen = true, moveAdult = true, moveLine, moveDottedLine;
    private Material dottedLineMaterial;

    private void Awake() {
        timelineSize = GetComponent<RectTransform>().sizeDelta.x;
        if (AutoOffset) {
            Offset = timelineSize / 2f;
        }

        dottedLineMaterial = new Material(DottedLine.material);
        DottedLine.material = dottedLineMaterial;
    }

    private void Update() {
        var teenPosition = Offset + (TurtleStats.Instance.TeenConversionTime - TurtleStats.Instance.Time) * TimelineScale;
        var adultPosition = Offset + (TurtleStats.Instance.AdultConversionTime - TurtleStats.Instance.Time) * TimelineScale;

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
        } else if(adultPosition > 0f) {
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
            ;
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
            ;
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
                dottedLineMaterial.SetFloat("_TimeScale", TurtleStats.Instance.TimeSpeed/TimelineScale/2f);
                DottedLine.enabled = false;
                DottedLine.enabled = true;
            }
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