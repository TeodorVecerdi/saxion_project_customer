using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotation : MonoBehaviour {
    public float RotationSpeed = 0.5f;

    private void Update() {
        transform.localEulerAngles += Vector3.up * RotationSpeed;
        if (transform.localEulerAngles.y >= 360f) {
            transform.localEulerAngles -= Vector3.up * 360f;
        }
    }
}