using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBoost : MonoBehaviour {
    [Header("References")]
    public Camera Camera;
    [Header("Settings")]
    public float NormalSpeed = 2f;
    public float BoostSpeed = 4f;
    public float NormalFOV = 50f;
    public float BoostFOV = 65f;
    public float TimeToActivate = 1f;

    private bool isBoosting;
    private bool isTransitionActive;
    private int transitionTimeDirection;
    private float transitionTimer = 0f;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B) && !isBoosting) {
            isBoosting = true;

            transitionTimer = isTransitionActive ? transitionTimer : 0f;
            isTransitionActive = true;
            transitionTimeDirection = 1;
        }

        if (Input.GetKeyUp(KeyCode.B) && isBoosting) {
            isBoosting = false;
            transitionTimer = isTransitionActive ? transitionTimer : TimeToActivate;
            isTransitionActive = true;
            transitionTimeDirection = -1;
        }

        if (!isTransitionActive)
            return;

        transitionTimer += Time.deltaTime * transitionTimeDirection;

        // Calculate current speed and FOV
        float currentSpeed;
        float currentFOV;
        if (transitionTimeDirection == 1) {
            currentSpeed = Mathfx.Hermite(NormalSpeed, BoostSpeed, transitionTimer / TimeToActivate);
            currentFOV = Mathfx.Hermite(NormalFOV, BoostFOV, transitionTimer / TimeToActivate);
        } else {
            currentSpeed = Mathfx.Hermite(NormalSpeed, BoostSpeed, transitionTimer / TimeToActivate);
            currentFOV = Mathfx.Hermite(NormalFOV, BoostFOV, transitionTimer / TimeToActivate);
        }

        // Apply speed and FOV
        TurtleStats.Instance.CurrentSpeed = currentSpeed;
        Camera.fieldOfView = currentFOV;

        // Check if transition ended
        if (transitionTimer < 0f) {
            isTransitionActive = false;
            TurtleStats.Instance.CurrentSpeed = NormalSpeed;
            Camera.fieldOfView = NormalFOV;
        } else if (transitionTimer > TimeToActivate) {
            isTransitionActive = false;
            TurtleStats.Instance.CurrentSpeed = BoostSpeed;
            Camera.fieldOfView = BoostFOV;
        }
    }
}