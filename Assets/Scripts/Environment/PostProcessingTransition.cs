using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingTransition : MonoBehaviour {
    [Header("Clean")]
    public Volume VolumeA;
    public float FogDensityA = 0.02f;
    public Color FogColorA;
    [Header("Dirty")]
    public Volume VolumeB;
    public float FogDensityB = 0.025f;
    public Color FogColorB;
    [Header("References")]
    public Camera Camera;

    private float currentWeight = 0f;
    
    private void Start() {
        VolumeA.weight = 1f;
        VolumeB.weight = 0f;
    }

    private void Update() {
        currentWeight = Mathf.Clamp01(TurtleStats.Instance.Time / TurtleStats.Instance.EssentialDeathTime);
        
        VolumeA.weight = 1f - currentWeight;
        VolumeB.weight = currentWeight;

        RenderSettings.fogDensity = Mathf.Lerp(FogDensityA, FogDensityB, currentWeight);
        var color = Color.Lerp(FogColorA, FogColorB, currentWeight);
        RenderSettings.fogColor = color;
        Camera.backgroundColor = color;
    }
}