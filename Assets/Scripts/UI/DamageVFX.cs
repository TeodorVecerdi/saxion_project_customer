using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageVFX : MonoBehaviour {
    [Header("References")]
    public Volume VolumeA;
    public Volume VolumeB;

    [Header("Settings")]
    public float EffectHalfDuration = 0.5f;
    public float VignetteIntensity = 0.3f;
    public float CameraShakeMagnitude = 1f;
    public float CameraShakeRoughness = 2.5f;
    public float InvincibilityDuration = 2f;
    public float ExtremeMultiplier = 2f;

    private Vignette vignetteA;
    private Vignette vignetteB;

    private bool isEffectActive;
    private bool isInvicibilityActive;
    private bool isExtreme;
    private float effectTimer;
    private int fadeDirection;
    private float invincibilityTimer;
    private TurtleDamage turtleDamage;

    private void Awake() {
        VolumeA.profile.TryGet(out vignetteA);
        VolumeB.profile.TryGet(out vignetteB);
    }

    public void Trigger(TurtleDamage turtleDamage, bool extreme = false) {
        this.turtleDamage = turtleDamage;
        turtleDamage.IsInvincible = true;
        isInvicibilityActive = true;
        invincibilityTimer = 0f;
        isExtreme = extreme;
        
        isEffectActive = true;
        effectTimer = 0f;
        fadeDirection = 1;
        if (extreme)
            CameraShaker.Shake(CameraShakeMagnitude * ExtremeMultiplier, CameraShakeRoughness * ExtremeMultiplier, EffectHalfDuration * ExtremeMultiplier / 2f, EffectHalfDuration * ExtremeMultiplier / 2f, EffectHalfDuration * ExtremeMultiplier);
        else
            CameraShaker.Shake(CameraShakeMagnitude, CameraShakeRoughness, EffectHalfDuration / 2f, EffectHalfDuration / 2f, EffectHalfDuration);
    }

    private void Update() {
        if (isInvicibilityActive) {
            invincibilityTimer += GameTime.DeltaTime;
            if (invincibilityTimer > InvincibilityDuration) {
                isInvicibilityActive = false;
                turtleDamage.IsInvincible = false;
            }
        }

        if (isEffectActive) {
            effectTimer += fadeDirection * GameTime.DeltaTime;
            UpdateEffect();
            if (effectTimer > EffectHalfDuration) {
                effectTimer = EffectHalfDuration;
                fadeDirection = -1;
            } else if (effectTimer < 0f) {
                isEffectActive = false;
                ResetEffect();
            }
        }
    }

    private void UpdateEffect() {
        var intensity = (isExtreme ? ExtremeMultiplier : 1) * VignetteIntensity * effectTimer / EffectHalfDuration;
        vignetteA.intensity.value = intensity;
        vignetteB.intensity.value = intensity;
    }

    private void ResetEffect() {
        vignetteA.intensity.value = 0f;
        vignetteB.intensity.value = 0f;
    }
}