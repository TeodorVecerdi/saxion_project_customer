using System.Collections;
using System.Collections.Generic;
using NoiseTest;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour {
    [Header("Position Settings (Decrease speed if increasing magnitude)")]
    public float xPositionMagnitude = .1f;
    public float yPositionMagnitude = .1f;
    public float zPositionMagnitude = .1f;
    public float xPositionSpeed = 1f;
    public float yPositionSpeed = 1f;
    public float zPositionSpeed = 1f;
    
    [Header("Rotation Settings (Decrease speed if increasing magnitude)")]
    public float xRotationMagnitude = 10f;
    public float yRotationMagnitude = 10f;
    public float zRotationMagnitude = 10f;
    public float xRotationSpeed = 1f;
    public float yRotationSpeed = 1f;
    public float zRotationSpeed = 1f;


    private OpenSimplexNoise simplexNoise;
    private float xStartPos;
    private float yStartPos;
    private float zStartPos;
    private float xRotStartPos;
    private float yRotStartPos;
    private float zRotStartPos;
    private Vector3 offset; 

    private void Start() {
        simplexNoise = new OpenSimplexNoise();
        xStartPos = UnityEngine.Random.Range(-10000f, 10000f);
        yStartPos = UnityEngine.Random.Range(-10000f, 10000f);
        zStartPos = UnityEngine.Random.Range(-10000f, 10000f);
        xRotStartPos = UnityEngine.Random.Range(-10000f, 10000f);
        yRotStartPos = UnityEngine.Random.Range(-10000f, 10000f);
        zRotStartPos = UnityEngine.Random.Range(-10000f, 10000f);
        offset = transform.localPosition;
    }

    // Update is called once per frame
    private void Update() {
        var currentPosition = Vector3.zero;
        var currentRotation = Vector3.zero;
        currentPosition.x = (float) simplexNoise.Evaluate(xStartPos, Time.time * xPositionSpeed) * xPositionMagnitude;
        currentPosition.y = (float) simplexNoise.Evaluate(yStartPos, Time.time * yPositionSpeed) * yPositionMagnitude;
        currentPosition.z = (float) simplexNoise.Evaluate(zStartPos, Time.time * zPositionSpeed) * zPositionMagnitude;
        currentRotation.x = (float) simplexNoise.Evaluate(xRotStartPos, Time.time * xRotationSpeed) * xRotationMagnitude;
        currentRotation.y = (float) simplexNoise.Evaluate(yRotStartPos, Time.time * yRotationSpeed) * yRotationMagnitude;
        currentRotation.z = (float) simplexNoise.Evaluate(zRotStartPos, Time.time * zRotationSpeed) * zRotationMagnitude;
        transform.localPosition = currentPosition + offset;
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
}