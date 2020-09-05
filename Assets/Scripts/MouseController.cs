using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using World;

public class MouseController : MonoBehaviour {
    [Header("Horizontal Movement Settings")]
    public float MaxHorizontal = 4.5f;
    public float DeadZoneRatioHorizontal = 0.1f;
    public float RatioHorizontal = 0f;

    [Header("Vertical Movement Settings")]
    public float MaxVertical = 1f;
    public float DeadZoneRatioVertical = 0.1f;
    public float RatioVertical = 0f;

    [Header("Horizontal Rotation Settings")]
    public float MaxAngleRotationHorizontal = 45f;
    public float RotationMultiplierHorizontal = 2f;

    [Header("Vertical Rotation Settings")]
    public float MaxAngleRotationVertical = 30f;
    public float RotationMultiplierVertical = 2f;

    [Header("Other Settings")]
    [Range(0f, 1f)] public float SmoothMovementDamping = 0.25f;
    [Range(0f, 1f)] public float SmoothRotationDamping = 0.3f;

    private Vector3 movementDampingVelocity;
    private HPBars bars;

    private void Start() {
        bars = HPBars.GetInstance();
    }

    private void Update() {
        // Horizontal Movement
        var screenSizeHorizontal = Screen.width;
        var deadZoneRatioHorizontal = screenSizeHorizontal * DeadZoneRatioHorizontal;
        var activeScreenZoneHorizontal = screenSizeHorizontal - 2 * deadZoneRatioHorizontal;
        var mousePositionX = Mathf.Clamp(Input.mousePosition.x - screenSizeHorizontal / 2f, -screenSizeHorizontal / 2f + deadZoneRatioHorizontal, screenSizeHorizontal / 2f - deadZoneRatioHorizontal);
        RatioHorizontal = mousePositionX / (activeScreenZoneHorizontal / 2f);

        // Vertical Movement
        var screenSizeVertical = Screen.height;
        var deadZoneRatioVertical = screenSizeVertical * DeadZoneRatioVertical;
        var activeScreenZoneVertical = screenSizeVertical - 2 * deadZoneRatioVertical;
        var mousePositionY = Mathf.Clamp(Input.mousePosition.y, deadZoneRatioVertical, screenSizeVertical - deadZoneRatioVertical);
        RatioVertical = mousePositionY / activeScreenZoneVertical;
        
        // Set Movement
        var targetPosition = transform.position;
        var currentPosition = targetPosition;
        targetPosition.x = RatioHorizontal * MaxHorizontal;
        targetPosition.y = RatioVertical * MaxVertical;
        var interpolatedPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref movementDampingVelocity, SmoothMovementDamping);
        transform.position = interpolatedPosition;

        // Rotation
        var difference = targetPosition - currentPosition;
        var differenceHorizontal = difference.x;
        var differenceVertical = difference.y;

        var currentDegreesHorizontal = Mathf.Max(-MaxAngleRotationHorizontal, Mathf.Min(MaxAngleRotationHorizontal, -(RotationMultiplierHorizontal * differenceHorizontal) / (2 * MaxHorizontal) * MaxAngleRotationHorizontal));
        var currentDegreesVertical = Mathf.Max(-MaxAngleRotationVertical, Mathf.Min(MaxAngleRotationVertical, -(RotationMultiplierVertical * differenceVertical) / (2 * MaxVertical) * MaxAngleRotationVertical));

        var currentRotation = transform.rotation;
        var targetRotation = Quaternion.Euler(currentDegreesVertical, 0, currentDegreesHorizontal);
        var interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, SmoothRotationDamping);
        transform.rotation = interpolatedRotation;
    }

    private void OnTriggerEnter(Collider other) {
        var isFood = other.gameObject.CompareTag("Food");
        var isJunk = other.gameObject.CompareTag("Junk");
        if (!isFood && !isJunk) return;

        var spawnableItem = other.GetComponentInParent<SpawnableItem>();
        bars.ChangeHealth(spawnableItem.HealthAmount);
        bars.ChangeHunger(spawnableItem.FoodAmount);

        Destroy(other.gameObject);
    }
}