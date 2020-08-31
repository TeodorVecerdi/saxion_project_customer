using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleWobbleAnimation : MonoBehaviour {
    [Header("Settings")]
    public float MaxY = 0.25f;
    public float Speed = 0.25f;
    public float RotationPeak = 15f;
    public AnimationCurve Motion;

    [Header("Current Stats")]
    public int Direction = 1;
    public float CurrentTime = 0f;

    private void Start() {
        transform.localPosition = new Vector3(0, -Direction * MaxY, 0);
    }

    private void Update() {

        var currentPosition = transform.localPosition;
        var currentY = Motion.Evaluate(CurrentTime) * MaxY * 2 - MaxY;
        currentPosition.y = currentY;
        transform.localPosition = currentPosition;
        
        var currentRot = -Direction * (Motion.Evaluate(CurrentTime + .5f) * RotationPeak * 2 - RotationPeak);
        transform.localRotation = Quaternion.Euler(currentRot, 0f, 0f);

        CurrentTime += Direction * Speed * Time.deltaTime;
        if (CurrentTime >= 1f) {
            CurrentTime = 1f;
            Direction *= -1;
        } else if (CurrentTime <= 0f) {
            CurrentTime = 0f;
            Direction *= -1;
        }

    }
}