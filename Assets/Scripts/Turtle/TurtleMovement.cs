using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TurtleMovement : MonoBehaviour {
    public Camera Camera;
    public MovementSettingsData MovementSettings;
    public BoostSettingsData BoostSettings;
    public SlownessSettingsData SlownessSettings;

    private readonly BoostVariables boostVariables = new BoostVariables();
    private readonly SlownessVariables slownessVariables = new SlownessVariables();
    private Vector3 movementDampingVelocity;
    private float maxHorizontalActual;
    private float maxVerticalActual;
    private bool isBoostDisabled;

    private void Update() {
        // Movement
        MovementStageCheck();
        UpdateMovement();

        // Boost
        BoostCheck();
        BoostUpdate();

        // Slowness
        SlownessCheck();
        SlownessUpdate();

        // Misc
        if (!boostVariables.IsBoostActive && !slownessVariables.IsSlownessActive) {
            TurtleState.Instance.CurrentSpeed = TurtleState.Instance.NormalSpeed;
        }
    }

    #region Movement Members
    private void MovementStageCheck() {
        if (TurtleState.Instance.StageSettings.CurrentStage == TurtleStage.Hatchling) {
            maxHorizontalActual = MovementSettings.MaxHorizontalHatchling;
            maxVerticalActual = MovementSettings.MaxVerticalHatchling;
        } else {
            maxHorizontalActual = MovementSettings.MaxHorizontal;
            maxVerticalActual = MovementSettings.MaxVertical;
        }
    }

    private void UpdateMovement() {
        // Horizontal Movement
        var screenSizeHorizontal = Screen.width;
        var deadZoneRatioHorizontal = screenSizeHorizontal * MovementSettings.DeadZoneRatioHorizontal;
        var activeScreenZoneHorizontal = screenSizeHorizontal - 2 * deadZoneRatioHorizontal;
        var mousePositionX = Mathf.Clamp(Input.mousePosition.x - screenSizeHorizontal / 2f, -screenSizeHorizontal / 2f + deadZoneRatioHorizontal, screenSizeHorizontal / 2f - deadZoneRatioHorizontal);
        MovementSettings.RatioHorizontal = mousePositionX / (activeScreenZoneHorizontal / 2f);

        // Vertical Movement
        var screenSizeVertical = Screen.height;
        var deadZoneRatioVertical = screenSizeVertical * MovementSettings.DeadZoneRatioVertical;
        var activeScreenZoneVertical = screenSizeVertical - 2 * deadZoneRatioVertical;
        var mousePositionY = Mathf.Clamp(Input.mousePosition.y - screenSizeVertical / 2f, -screenSizeVertical / 2f + deadZoneRatioVertical, screenSizeVertical / 2f - deadZoneRatioVertical);
        MovementSettings.RatioVertical = mousePositionY / (activeScreenZoneVertical / 2f);

        // Set Movement
        var targetPosition = transform.position;
        var currentPosition = targetPosition;
        targetPosition.x = MovementSettings.RatioHorizontal * maxHorizontalActual;
        targetPosition.y = MovementSettings.RatioVertical * maxVerticalActual + MovementSettings.VerticalOffset;
        var interpolatedPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref movementDampingVelocity, MovementSettings.SmoothPositionDamping);
        transform.position = interpolatedPosition;

        // Rotation
        var difference = targetPosition - currentPosition;
        var differenceHorizontal = difference.x;
        var differenceVertical = difference.y;

        var currentDegreesHorizontal = Mathf.Max(-MovementSettings.MaxAngleRotationHorizontal, Mathf.Min(MovementSettings.MaxAngleRotationHorizontal, -(MovementSettings.RotationMultiplierHorizontal * differenceHorizontal) / (2 * maxHorizontalActual) * MovementSettings.MaxAngleRotationHorizontal));
        var currentDegreesVertical = Mathf.Max(-MovementSettings.MaxAngleRotationVertical, Mathf.Min(MovementSettings.MaxAngleRotationVertical, -(MovementSettings.RotationMultiplierVertical * differenceVertical) / (2 * maxVerticalActual) * MovementSettings.MaxAngleRotationVertical));

        var currentRotation = transform.rotation;
        var targetRotation = Quaternion.Euler(currentDegreesVertical, 0, currentDegreesHorizontal);
        var interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, MovementSettings.SmoothRotationDamping);
        transform.rotation = interpolatedRotation;
    }
    #endregion

    #region Boost Members
    private void BoostCheck() {
        if (boostVariables.IsBoostAvailable && Input.GetKeyDown(KeyCode.Space) && !IsBoostDisabled) {
            boostVariables.IsBoostActive = true;
            boostVariables.IsBoostAvailable = false;
            boostVariables.BoostTimer = 0f;
            boostVariables.IsBoosting = true;
            boostVariables.TransitionTimer = boostVariables.IsTransitionActive ? boostVariables.TransitionTimer : 0f;
            boostVariables.IsTransitionActive = true;
            boostVariables.TransitionTimeDirection = 1;
            SoundManager.PlaySound("boost_use");
        }

        if (boostVariables.IsBoosting) {
            boostVariables.BoostTimer += GameTime.DeltaTime;
            BoostSettings.BoostFillImage.fillAmount = 1 - boostVariables.BoostTimer / BoostSettings.Duration;
            if (boostVariables.BoostTimer >= BoostSettings.Duration) {
                boostVariables.IsBoosting = false;
                boostVariables.CooldownTimer = 0f;
                boostVariables.IsCooldownActive = true;
                boostVariables.TransitionTimer = boostVariables.IsTransitionActive ? boostVariables.TransitionTimer : BoostSettings.TransitionTime;
                boostVariables.IsTransitionActive = true;
                boostVariables.TransitionTimeDirection = -1;
            }
        }

        if (boostVariables.IsCooldownActive) {
            boostVariables.CooldownTimer += GameTime.DeltaTime;
            BoostSettings.BoostFillImage.fillAmount = boostVariables.CooldownTimer / BoostSettings.Cooldown;
            if (boostVariables.CooldownTimer >= BoostSettings.Cooldown) {
                boostVariables.IsCooldownActive = false;
                boostVariables.IsBoostAvailable = true;
                SoundManager.PlaySound("boost_ready");
                BoostSettings.BoostFillImage.fillAmount = 1f;
            }
        }
    }

    private void BoostUpdate() {
        if (!boostVariables.IsTransitionActive)
            return;

        boostVariables.TransitionTimer += GameTime.DeltaTime * boostVariables.TransitionTimeDirection;

        // Calculate current speed and FOV
        float currentSpeed;
        float currentFOV;

        currentSpeed = MathFunctions.EaseInOut(TurtleState.Instance.NormalSpeed, TurtleState.Instance.NormalSpeed + BoostSettings.Speed, boostVariables.TransitionTimer / BoostSettings.TransitionTime);
        currentFOV = MathFunctions.EaseInOut(BoostSettings.NormalFOV, BoostSettings.BoostFOV, boostVariables.TransitionTimer / BoostSettings.TransitionTime);

        // Check if transition ended
        if (boostVariables.TransitionTimer < 0f) {
            boostVariables.IsBoostActive = false;
            boostVariables.IsTransitionActive = false;
            currentSpeed = TurtleState.Instance.NormalSpeed;
            currentFOV = BoostSettings.NormalFOV;
        } else if (boostVariables.TransitionTimer > BoostSettings.TransitionTime) {
            boostVariables.IsTransitionActive = false;
            currentSpeed = TurtleState.Instance.NormalSpeed + BoostSettings.Speed;
            currentFOV = BoostSettings.BoostFOV;
        }

        // Apply speed and FOV
        TurtleState.Instance.CurrentSpeed = currentSpeed;
        Camera.fieldOfView = currentFOV;
    }

    private void DisableBoost() {
        boostVariables.IsTransitionActive = false;
        boostVariables.IsBoosting = false;
        boostVariables.IsCooldownActive = false;
        boostVariables.IsBoostAvailable = false;
        TurtleState.Instance.CurrentSpeed = TurtleState.Instance.NormalSpeed;
        Camera.fieldOfView = BoostSettings.NormalFOV;
        BoostSettings.BoostFillImage.fillAmount = 0f;
    }

    private void EnableBoost() {
        boostVariables.IsBoostAvailable = true;
        BoostSettings.BoostFillImage.fillAmount = 1f;
    }

    private bool IsBoostDisabled {
        get => isBoostDisabled;
        set {
            isBoostDisabled = value;
            if (isBoostDisabled) {
                DisableBoost();
            } else {
                EnableBoost();
            }
        }
    }
    #endregion

    #region Slowness Members
    public void TriggerSlowness() {
        slownessVariables.SlownessTimer = 0f;
        slownessVariables.IsSlowing = true;
        slownessVariables.IsSlownessActive = true;
        slownessVariables.TransitionTimer = slownessVariables.IsTransitionActive ? slownessVariables.TransitionTimer : 0f;
        slownessVariables.IsTransitionActive = true;
        slownessVariables.TransitionTimeDirection = 1;
        IsBoostDisabled = true;
    }

    private void SlownessCheck() {
        if (slownessVariables.IsSlowing) {
            slownessVariables.SlownessTimer += GameTime.DeltaTime;
            if (slownessVariables.SlownessTimer >= SlownessSettings.Duration) {
                slownessVariables.IsSlowing = false;

                slownessVariables.TransitionTimer = slownessVariables.IsTransitionActive ? slownessVariables.TransitionTimer : SlownessSettings.TransitionTime;
                slownessVariables.IsTransitionActive = true;
                slownessVariables.TransitionTimeDirection = -1;
            }
        }
    }

    private void SlownessUpdate() {
        if (!slownessVariables.IsTransitionActive)
            return;

        slownessVariables.TransitionTimer += GameTime.DeltaTime * slownessVariables.TransitionTimeDirection;

        // Calculate current speed and FOV
        float currentSpeed;
        float currentFOV;
        currentSpeed = MathFunctions.EaseInOut(TurtleState.Instance.NormalSpeed, TurtleState.Instance.NormalSpeed * SlownessSettings.SpeedMultiplier, slownessVariables.TransitionTimer / SlownessSettings.TransitionTime);
        currentFOV = MathFunctions.EaseInOut(SlownessSettings.NormalFOV, SlownessSettings.SlownessFOV, slownessVariables.TransitionTimer / SlownessSettings.TransitionTime);
        
        // Check if transition ended
        if (slownessVariables.TransitionTimer < 0f) {
            slownessVariables.IsSlownessActive = false;
            slownessVariables.IsTransitionActive = false;
            currentSpeed = TurtleState.Instance.NormalSpeed;
            currentFOV = SlownessSettings.NormalFOV;
            IsBoostDisabled = false;
        } else if (slownessVariables.TransitionTimer > SlownessSettings.TransitionTime) {
            slownessVariables.IsTransitionActive = false;
            currentSpeed = TurtleState.Instance.NormalSpeed * SlownessSettings.SpeedMultiplier;
            currentFOV = SlownessSettings.SlownessFOV;
        }
        
        // Apply speed and FOV
        TurtleState.Instance.CurrentSpeed = currentSpeed;
        Camera.fieldOfView = currentFOV;
    }
    #endregion

    [Serializable] public class MovementSettingsData {
        [Header("Horizontal Position Settings")]
        public float MaxHorizontal = 2f;
        public float MaxHorizontalHatchling = 2.5f;
        public float DeadZoneRatioHorizontal = 0.15f;
        public float RatioHorizontal = 0f;

        [Header("Vertical Position Settings")]
        public float MaxVertical = 1f;
        public float MaxVerticalHatchling = 1f;
        public float VerticalOffset = 1.5f;
        public float DeadZoneRatioVertical = 0.2f;
        public float RatioVertical = 0f;

        [Header("Horizontal Rotation Settings")]
        public float MaxAngleRotationHorizontal = 45f;
        public float RotationMultiplierHorizontal = 1.5f;

        [Header("Vertical Rotation Settings")]
        public float MaxAngleRotationVertical = 45f;
        public float RotationMultiplierVertical = 3f;

        [Header("Other Settings")]
        [Range(0f, 1f)] public float SmoothPositionDamping = 0.3f;
        [Range(0f, 1f)] public float SmoothRotationDamping = 0.2f;
    }

    [Serializable] public class BoostSettingsData {
        public float Duration = 4f;
        public float Cooldown = 6f;
        [Space]
        public float Speed = 3f;
        public float NormalFOV = 50f;
        public float BoostFOV = 65f;
        public float TransitionTime = 0.5f;
        [Space]
        public Image BoostFillImage;
    }

    private class BoostVariables {
        // Boost variables
        public bool IsBoostActive;
        public bool IsBoostAvailable = true;
        public bool IsBoosting;
        public float BoostTimer;

        // Cooldown variables
        public bool IsCooldownActive;
        public float CooldownTimer;

        // Transition variables
        public bool IsTransitionActive;
        public int TransitionTimeDirection;
        public float TransitionTimer;
    }

    [Serializable] public class SlownessSettingsData {
        public float Duration = 4f;
        [Space]
        public float SpeedMultiplier = 1f;
        public float NormalFOV = 50f;
        public float SlownessFOV = 45f;
        public float TransitionTime = 0.75f;
    }

    private class SlownessVariables {
        // Slowness variables
        public bool IsSlownessActive;
        public bool IsSlowing;
        public float SlownessTimer;

        // Transition variables
        public bool IsTransitionActive;
        public int TransitionTimeDirection;
        public float TransitionTimer;
    }
}