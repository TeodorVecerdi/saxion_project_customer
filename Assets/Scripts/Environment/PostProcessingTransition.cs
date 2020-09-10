using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingTransition : MonoBehaviour {
    public Volume VolumeA;
    public Volume VolumeB;

    private float currentWeight = 0f;
    
    private void Start() {
        VolumeA.weight = 1f;
        VolumeB.weight = 0f;
    }

    private void Update() {
        currentWeight = Mathf.Clamp01(TurtleStats.Instance.DistanceTravelled / TurtleStats.Instance.EssentialDeathTime);
        VolumeA.weight = 1f - currentWeight;
        VolumeB.weight = currentWeight;
    }
}