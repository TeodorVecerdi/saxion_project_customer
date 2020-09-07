using System;
using UnityEngine;

public class SunShaft : MonoBehaviour {
    public SunShaftsController Controller;
    private new ParticleSystem particleSystem;
    private ParticleSystem.MainModule mainModule;
    private bool fading;
    private float currentFadeAmount;

    private void Awake() {
        particleSystem = GetComponent<ParticleSystem>();
        mainModule = particleSystem.main;
    }

    private void Start() {
        fading = true;
        currentFadeAmount = Controller.StartFadeAlpha;
    }

    private void Update() {
        if (transform.position.z < Controller.StopFadeZ) {
            fading = false;
            var color = mainModule.startColor.color;
            color.a = Controller.EndFadeAlpha / 100f;
            
            mainModule.startColor = color;
        }

        if (fading) {
            var color = mainModule.startColor.color;
            var zTotal = Controller.StartZ - Controller.StopFadeZ;
            var zProgress = transform.position.z - Controller.StopFadeZ;
            color.a = Mathf.Lerp(Controller.StartFadeAlpha/100f, Controller.EndFadeAlpha/100f, 1 - zProgress / zTotal);
            
            mainModule.startColor = color;
        }
        
        transform.position -= new Vector3(0f, 0f, TurtleStats.Instance.CurrentSpeed * GameTime.DeltaTime);
    }
}