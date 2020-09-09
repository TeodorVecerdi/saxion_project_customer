using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurtleMovement : MonoBehaviour {
    [Header("Horizontal Movement Settings")]
    public float MaxHorizontal = 2;
    public float MaxHorizontalHatchling = 2.5f;
    public float DeadZoneRatioHorizontal = 0.1f;
    public float RatioHorizontal = 0f;

    [Header("Vertical Movement Settings")]
    public float MaxVertical = 2;
    public float MaxVerticalHatchling = 2.5f;
    public float VerticalOffset = 2f;
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

    [Header("Camera Movement Settings")]
    public Transform CameraTransform;
    public Vector4 CameraMovementRange;
    [Range(0f, 1f)] public float CameraSmoothMovementDamping = 0.25f;

    private Vector3 movementDampingVelocity;
    private Vector3 cameraMovementDampingVelocity;

    private float maxHorizontalActual;
    private float maxVerticalActual;

    private void Update() {
        if (TurtleStats.Instance.Stage == TurtleStage.Hatchling) {
            maxHorizontalActual = MaxHorizontalHatchling;
            maxVerticalActual = MaxVerticalHatchling;
        } else {
            maxHorizontalActual = MaxHorizontal;
            maxVerticalActual = MaxVertical;
        }
        
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
        var mousePositionY = Mathf.Clamp(Input.mousePosition.y - screenSizeVertical / 2f, -screenSizeVertical / 2f + deadZoneRatioVertical, screenSizeVertical / 2f - deadZoneRatioVertical);
        RatioVertical = mousePositionY / (activeScreenZoneVertical / 2f);

        // Set Movement
        var targetPosition = transform.position;
        var currentPosition = targetPosition;
        targetPosition.x = RatioHorizontal * maxHorizontalActual;
        targetPosition.y = RatioVertical * maxVerticalActual + VerticalOffset;
        var interpolatedPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref movementDampingVelocity, SmoothMovementDamping);
        transform.position = interpolatedPosition;

        // Rotation
        var difference = targetPosition - currentPosition;
        var differenceHorizontal = difference.x;
        var differenceVertical = difference.y;

        var currentDegreesHorizontal = Mathf.Max(-MaxAngleRotationHorizontal, Mathf.Min(MaxAngleRotationHorizontal, -(RotationMultiplierHorizontal * differenceHorizontal) / (2 * maxHorizontalActual) * MaxAngleRotationHorizontal));
        var currentDegreesVertical = Mathf.Max(-MaxAngleRotationVertical, Mathf.Min(MaxAngleRotationVertical, -(RotationMultiplierVertical * differenceVertical) / (2 * maxVerticalActual) * MaxAngleRotationVertical));

        var currentRotation = transform.rotation;
        var targetRotation = Quaternion.Euler(currentDegreesVertical, 0, currentDegreesHorizontal);
        var interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, SmoothRotationDamping);
        transform.rotation = interpolatedRotation;

        // Camera movement;
        var turtleVec2 = new Vector2(interpolatedPosition.x, interpolatedPosition.y);
        var cameraPositionVec2 = MiscUtils.MapRange(turtleVec2, new Vector2(-maxHorizontalActual, -maxVerticalActual + VerticalOffset),
            new Vector2(maxHorizontalActual, maxVerticalActual + VerticalOffset), new Vector2(CameraMovementRange.x, CameraMovementRange.y),
            new Vector2(CameraMovementRange.z, CameraMovementRange.w));
        var cameraPosition = new Vector3(cameraPositionVec2.x, cameraPositionVec2.y, 0f);
        var interpolatedCameraPosition = Vector3.SmoothDamp(CameraTransform.localPosition, cameraPosition, ref cameraMovementDampingVelocity, CameraSmoothMovementDamping);
        CameraTransform.localPosition = interpolatedCameraPosition;
    }
}