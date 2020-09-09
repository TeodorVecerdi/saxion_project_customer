using System;
using UnityEngine;

public class CameraShaker : MonoBehaviour {
    private static CameraShaker instance;

    public Vector3 PositionMagnitude = Vector3.one * 0.1f;
    public Vector3 RotationMagnitude = Vector3.one;

    private Vector3 originalPosition;
    private Vector3 originalEulerAngles;

    private float magnitude;
    private float roughness;
    private float fadeTimer;
    private float sustainTimer;
    private float fadeInTime;
    private float sustainTime;
    private float fadeOutTime;
    private float perlinOffset;
    private bool isActive;
    private bool isFadingIn;
    private bool isSustaining;
    private bool isFadingOut;

    private void Awake() {
        if (instance == null) instance = this;
        else throw new Exception("There can only be one CameraShaker in a scene!");
        
        originalPosition = transform.localPosition;
        originalEulerAngles = transform.localEulerAngles;
    }

    private void Update() {
        if(!isActive) return;
        if (!isFadingIn && !isFadingOut && !isSustaining) {
            transform.localPosition = originalPosition;
            transform.localEulerAngles = originalEulerAngles;
            isActive = false;
            return;
        }

        UpdateTimers();
        
        transform.localPosition = originalPosition + GetShakeValue(PositionMagnitude);
        transform.localEulerAngles = originalEulerAngles + GetShakeValue(RotationMagnitude);
    }

    private void UpdateTimers() {
        if (isFadingIn) {
            fadeTimer += GameTime.DeltaTime / fadeInTime;
            if (fadeTimer > 1f) {
                isFadingIn = false;
                if (sustainTime > 0f)
                    isSustaining = true;
                else if (fadeOutTime > 0f)
                    isFadingOut = true;
            }
        }

        if (isSustaining) {
            sustainTimer += GameTime.DeltaTime / sustainTime;
            if (sustainTimer > 1f) {
                isSustaining = false;
                if (fadeOutTime > 0f)
                    isFadingOut = true;
            }
        }

        if (isFadingOut) {
            fadeTimer -= GameTime.DeltaTime / fadeOutTime;
            if (fadeTimer < 0f)
                isFadingOut = false;
        }
    }

    private Vector3 GetShakeValue(Vector3 vectorMagnitude) {
        var value = Vector3.zero;
        
        value.x = Mathf.PerlinNoise(perlinOffset, 0f) * vectorMagnitude.x;
        value.y = Mathf.PerlinNoise(0f, perlinOffset) * vectorMagnitude.y;
        value.z = Mathf.PerlinNoise(perlinOffset, perlinOffset) * vectorMagnitude.z;
        
        value *= magnitude;
        if (!isSustaining) value *= fadeTimer;
        
        if (isSustaining) perlinOffset += GameTime.DeltaTime * roughness;
        else perlinOffset += GameTime.DeltaTime * fadeTimer * roughness;

        return value;
    }

    public static void Shake(float magnitude, float roughness, float fadeInTime, float fadeOutTime, float sustainTime = 0f) {
        // Update settings
        instance.magnitude = magnitude;
        instance.roughness = roughness;
        instance.fadeInTime = fadeInTime;
        instance.sustainTime = sustainTime;
        instance.fadeOutTime = fadeOutTime;
        
        // Reset
        instance.fadeTimer = 0f;
        instance.sustainTimer = 0f;
        instance.isActive = true;
        instance.isFadingIn = false;
        instance.isSustaining = false;
        instance.isFadingOut = false;
        if(fadeInTime > 0f)
            instance.isFadingIn = true;
        else if(sustainTime > 0f)
            instance.isSustaining = true;
        else if(fadeOutTime > 0f)
            instance.isFadingOut = true;

        instance.perlinOffset = UnityEngine.Random.Range(-10000f, 10000f);
    }

    /*public static void Shake (float duration, float amount, float fadeInTime, float fadeOutTime) {
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(Instance.CameraShake(duration, amount, fadeInTime, fadeOutTime));
    }*/
}