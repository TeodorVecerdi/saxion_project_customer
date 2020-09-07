using UnityEngine;
using UnityEngine.UI;

public class TurtleBoost : MonoBehaviour {
    [Header("References")]
    public Camera Camera;

    [Header("General Settings")]
    public float BoostDuration = 3f;
    public float BoostCooldown = 4f;
    [Header("Boost Settings")]
    public float NormalSpeed = 2f;
    public float BoostSpeed = 4f;
    public float NormalFOV = 50f;
    public float BoostFOV = 65f;
    public float TimeToActivate = 1f;
    [Header("Boost UI References")]
    public Color AvailableColor;
    public Color CooldownColor;
    public Image CooldownProgressImage;
    public Image BoostIconImage;
    public Image BorderImage;
    
    private bool isBoostAvailable = true;
    private bool isCooldownActive = false;
    private float cooldownTimer;
    private float boostTimer;
    
    private bool isBoosting;
    private bool isTransitionActive;
    private int transitionTimeDirection;
    private float transitionTimer;

    private void Update() {
        if (isBoostAvailable && Input.GetKeyDown(KeyCode.B)) {
            isBoostAvailable = false;
            boostTimer = 0f;
            
            cooldownTimer = 0f;
            isCooldownActive = true;

            isBoosting = true;
            transitionTimer = isTransitionActive ? transitionTimer : 0f;
            isTransitionActive = true;
            transitionTimeDirection = 1;

            BoostIconImage.color = CooldownColor;
            BorderImage.color = CooldownColor;
        }

        if (isBoosting) {
            boostTimer += GameTime.DeltaTime;
            if (boostTimer >= BoostDuration) {
                isBoosting = false;

                transitionTimer = isTransitionActive ? transitionTimer : TimeToActivate;
                isTransitionActive = true;
                transitionTimeDirection = -1;
            }
        }

        if (isCooldownActive) {
            cooldownTimer += GameTime.DeltaTime;
            CooldownProgressImage.fillAmount = 1f - cooldownTimer / BoostCooldown;
            if (cooldownTimer >= BoostCooldown) {
                isCooldownActive = false;
                isBoostAvailable = true;
                CooldownProgressImage.fillAmount = 0f;
                BoostIconImage.color = AvailableColor;
                BorderImage.color = AvailableColor;
            }
        }

        if (!isTransitionActive)
            return;

        transitionTimer += GameTime.DeltaTime * transitionTimeDirection;

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