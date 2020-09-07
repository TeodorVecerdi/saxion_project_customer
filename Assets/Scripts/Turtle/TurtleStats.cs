using System;
using TMPro;
using UnityEngine;

public class TurtleStats : MonoBehaviour {
    [Header("Stats")]
    public float Hunger;
    public float Health;
    public float DistanceTravelled;
    public float CurrentSpeed = 2;

    [Header("Other References")]
    public TMP_Text DistanceText;

    public static TurtleStats Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else Debug.LogError("There should only be one instance of `TurtleStats` active in the scene!");
    }

    private void Start() {
        DistanceTravelled = 0f;
    }

    private void Update() {
        DistanceTravelled += CurrentSpeed * GameTime.DeltaTime;
        DistanceText.text = $"Distance travelled:\n<b><color=#2ECC71>{Math.Round(DistanceTravelled, 2):.00}m</color></b>";
    }
}