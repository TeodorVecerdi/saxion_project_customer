using System;
using TMPro;
using UnityEngine;

public class TurtleStats : MonoBehaviour {
    [Header("Stats")]
    public float DistanceTravelled;
    public float CurrentSpeed = 2;

    [Header("Stage")]
    public TurtleStage Stage;
    public float TeenDistance = 100f;
    public float AdultDistance = 200f;

    [Header("Other References")]
    public TMP_Text DistanceText;

    public static TurtleStats Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else throw new Exception("There can only be one TurtleHealth in a scene!");
    }

    private void Start() {
        DistanceTravelled = 0f;
    }

    private void Update() {
        DistanceTravelled += CurrentSpeed * GameTime.DeltaTime;
        DistanceText.text = $"<b>[{Stage.ToString()}]</b>\nDistance travelled:\n<b><color=#2ECC71>{Math.Round(DistanceTravelled, 2):.00}m</color></b>";
        
        if (DistanceTravelled >= TeenDistance && DistanceTravelled < AdultDistance) Stage = TurtleStage.Teen;
        else if (DistanceTravelled >= AdultDistance) Stage = TurtleStage.Adult;
        else Stage = TurtleStage.Hatchling;
    }
}

public enum TurtleStage {
    Hatchling,
    Teen,
    Adult
}