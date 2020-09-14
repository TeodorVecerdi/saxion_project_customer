using System;
using UnityEngine;

[RequireComponent(typeof(TurtleBoost))]
public class TurtleSlownessEffect : MonoBehaviour {
    [Header("References")]
    public Camera Camera;

    [Header("General Settings")]
    public float Duration = 4f;
    
    [Header("Slowness Settings")]
    public float SlownessSpeed = 1f;
    public float NormalFOV = 50f;
    public float SlownessFOV = 40f;
    public float TransitionTime = 0.75f;

    private TurtleBoost turtleBoost;
    
    // Slowness variables
    private bool isSlowing; 
    private float slownessTimer;
    
    // Transition variables
    private bool isTransitionActive;
    private int transitionTimeDirection;
    private float transitionTimer;

    private void Awake() {
        turtleBoost = GetComponent<TurtleBoost>();
    }

    public void Trigger() {
        slownessTimer = 0f;
        isSlowing = true;
        transitionTimer = isTransitionActive ? transitionTimer : 0f;
        isTransitionActive = true;
        transitionTimeDirection = 1;
        turtleBoost.Disabled = true;
    }

    private void Update() {
        if (isSlowing) {
            slownessTimer += GameTime.DeltaTime;
            if (slownessTimer >= Duration) {
                isSlowing = false;

                transitionTimer = isTransitionActive ? transitionTimer : TransitionTime;
                isTransitionActive = true;
                transitionTimeDirection = -1;
            }
        }

        if (!isTransitionActive)
            return;

        transitionTimer += GameTime.DeltaTime * transitionTimeDirection;

        // Calculate current speed and FOV
        float currentSpeed;
        float currentFOV;
        if (transitionTimeDirection == 1) {
            currentSpeed = Mathfx.Hermite(TurtleStats.Instance.NormalSpeed, SlownessSpeed, transitionTimer / TransitionTime);
            currentFOV = Mathfx.Hermite(NormalFOV, SlownessFOV, transitionTimer / TransitionTime);
        } else {
            currentSpeed = Mathfx.Hermite(TurtleStats.Instance.NormalSpeed, SlownessSpeed, transitionTimer / TransitionTime);
            currentFOV = Mathfx.Hermite(NormalFOV, SlownessFOV, transitionTimer / TransitionTime);
        }

        // Apply speed and FOV
        TurtleStats.Instance.CurrentSpeed = currentSpeed;
        Camera.fieldOfView = currentFOV;

        // Check if transition ended
        if (transitionTimer < 0f) {
            isTransitionActive = false;
            TurtleStats.Instance.CurrentSpeed = TurtleStats.Instance.NormalSpeed;
            Camera.fieldOfView = NormalFOV;
            turtleBoost.Disabled = false;
        } else if (transitionTimer > TransitionTime) {
            isTransitionActive = false;
            TurtleStats.Instance.CurrentSpeed = SlownessSpeed;
            Camera.fieldOfView = SlownessFOV;
        }
    }
}