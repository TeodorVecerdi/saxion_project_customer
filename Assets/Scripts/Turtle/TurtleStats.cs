using System;
using TMPro;
using UnityEngine;

public class TurtleStats : MonoBehaviour {
    [Header("Stats")]
    public float DistanceTravelled;
    public float NormalSpeed = 2;
    public float CurrentSpeed = 2;

    [Header("Stage")]
    public TurtleStage Stage;
    public float TeenDistance = 100f;
    public float AdultDistance = 200f;
    public float EssentialDeathDistance = 300f;

    [Header("Food/Junk Spawning Settings")]
    public float CurrentJunkDistribution = 0f;
    [MinMaxSlider(0f, 1f)] public MinMax HatchlingTeenJunkDistribution = new MinMax(0f, 0.1f);
    [MinMaxSlider(0f, 1f)] public MinMax TeenAdultJunkDistribution = new MinMax(0.1f, 0.75f);
    [MinMaxSlider(0f, 1f)] public MinMax PostAdultJunkDistribution = new MinMax(0.75f, 0.9f);
    [Space]
    public float CurrentItemSpawningChance = 0f;
    [MinMaxSlider(0f, 1f)] public MinMax HatchlingTeenItemChance = new MinMax(0.5f, 0.75f);
    [MinMaxSlider(0f, 1f)] public MinMax TeenAdultItemChance = new MinMax(0.75f, 1f);
    [MinMaxSlider(0f, 1f)] public MinMax PostAdultItemChance = new MinMax(1f, 1f);

    [Header("Other References")]
    public TMP_Text DistanceText;

    public static TurtleStats Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else throw new Exception("There can only be one TurtleStats in a scene!");
    }

    private void Start() {
        CurrentSpeed = NormalSpeed;
        DistanceTravelled = 0f;
    }

    private void Update() {
        DistanceTravelled += CurrentSpeed * GameTime.DeltaTime;
        // Junk chance: {CurrentJunkDistribution}\n
        DistanceText.text = $"<b>[{(Stage != TurtleStage.ShouldDieSoon ? Stage : TurtleStage.Adult).ToString()}]</b>\nDistance travelled:\n<b><color=#2ECC71>{Math.Round(DistanceTravelled, 2):.00}m</color></b>";
        
        // Update stage
        if (DistanceTravelled >= TeenDistance && DistanceTravelled < AdultDistance) Stage = TurtleStage.Teen;
        else if (DistanceTravelled >= AdultDistance) Stage = TurtleStage.Adult;
        else if (DistanceTravelled >= EssentialDeathDistance) Stage = TurtleStage.ShouldDieSoon;
        else Stage = TurtleStage.Hatchling;
        
        // Update junk distribution and item spawn chance
        switch (Stage) {
            case TurtleStage.Hatchling:
                CurrentJunkDistribution = Mathf.Lerp(HatchlingTeenJunkDistribution.Min, HatchlingTeenJunkDistribution.Max, DistanceTravelled / TeenDistance);
                CurrentItemSpawningChance = Mathf.Lerp(HatchlingTeenItemChance.Min, HatchlingTeenItemChance.Max, DistanceTravelled / TeenDistance);
                break;
            case TurtleStage.Teen:
                CurrentJunkDistribution = Mathf.Lerp(TeenAdultJunkDistribution.Min, TeenAdultJunkDistribution.Max, (DistanceTravelled - TeenDistance) / (AdultDistance - TeenDistance));
                CurrentItemSpawningChance = Mathf.Lerp(TeenAdultItemChance.Min, TeenAdultItemChance.Max, (DistanceTravelled - TeenDistance) / (AdultDistance - TeenDistance));
                break;
            case TurtleStage.Adult:
                CurrentJunkDistribution = Mathf.Lerp(PostAdultJunkDistribution.Min, PostAdultJunkDistribution.Max, (DistanceTravelled - AdultDistance) / (EssentialDeathDistance - AdultDistance));
                CurrentItemSpawningChance = Mathf.Lerp(PostAdultItemChance.Min, PostAdultItemChance.Max, (DistanceTravelled - AdultDistance) / (EssentialDeathDistance - AdultDistance));
                break;
            case TurtleStage.ShouldDieSoon:
                CurrentJunkDistribution = PostAdultJunkDistribution.Max;
                CurrentItemSpawningChance = PostAdultItemChance.Max;
                break;
        }
        
        
    }
}

public enum TurtleStage {
    Hatchling,
    Teen,
    Adult,
    ShouldDieSoon
}