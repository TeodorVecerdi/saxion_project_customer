using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotation : MonoBehaviour {
    public float RotationSpeed = 0.5f;
    public Material Skybox;
    private static readonly int RotationProperty = Shader.PropertyToID("_Rotation");

    private void Update() {
        var currentRotation = Skybox.GetFloat(RotationProperty);
        currentRotation += RotationSpeed;
        if (currentRotation >= 360f) {
            currentRotation -=  360f;
        }

        Skybox.SetFloat(RotationProperty, currentRotation);
    }
}