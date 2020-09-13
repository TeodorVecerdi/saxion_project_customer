using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TurtleStats : MonoBehaviour {
    [Header("Stats")]
    public float DistanceTravelled;
    public float Time;
    public float TimeSpeed = 2f;
    public float NormalSpeed = 2f;
    public float CurrentSpeed = 2f;

    [Header("Stage")]
    public TurtleStage Stage;
    [FormerlySerializedAs("TeenDistance")]
    public float TeenConversionTime = 100f;
    [FormerlySerializedAs("AdultDistance")]
    public float AdultConversionTime = 200f;
    [FormerlySerializedAs("EssentialDeathDistance")]
    public float EssentialDeathTime = 300f;

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

    [Space]
    public bool JustAteTrash;
    public bool JustGotByPoachers;

    public static TurtleStats Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else throw new Exception("There can only be one TurtleStats in a scene!");
    }

    private void Start() {
        CurrentSpeed = NormalSpeed;
        DistanceTravelled = 0f;
        Time = 0f;
    }

    private void Update() {
        DistanceTravelled += CurrentSpeed * GameTime.DeltaTime;
        Time += TimeSpeed * GameTime.DeltaTime;
        // Junk chance: {CurrentJunkDistribution}\n
        DistanceText.text = $"<b>[{(Stage != TurtleStage.ShouldDieSoon ? Stage : TurtleStage.Adult).ToString()}]</b>\nDistance travelled:\n<b><color=#2ECC71>{Math.Round(DistanceTravelled, 2):.00}m</color></b>";
        
        // Update stage
        if (Time >= TeenConversionTime && Time < AdultConversionTime) Stage = TurtleStage.Teen;
        else if (Time >= AdultConversionTime) Stage = TurtleStage.Adult;
        else if (Time >= EssentialDeathTime) Stage = TurtleStage.ShouldDieSoon;
        else Stage = TurtleStage.Hatchling;
        
        // Update junk distribution and item spawn chance
        UpdateChances();
    }

    public void UpdateChances() {
        switch (Stage) {
            case TurtleStage.Hatchling:
                CurrentJunkDistribution = Mathf.Lerp(HatchlingTeenJunkDistribution.Min, HatchlingTeenJunkDistribution.Max, Time / TeenConversionTime);
                CurrentItemSpawningChance = Mathf.Lerp(HatchlingTeenItemChance.Min, HatchlingTeenItemChance.Max, Time / TeenConversionTime);
                break;
            case TurtleStage.Teen:
                CurrentJunkDistribution = Mathf.Lerp(TeenAdultJunkDistribution.Min, TeenAdultJunkDistribution.Max, (Time - TeenConversionTime) / (AdultConversionTime - TeenConversionTime));
                CurrentItemSpawningChance = Mathf.Lerp(TeenAdultItemChance.Min, TeenAdultItemChance.Max, (Time - TeenConversionTime) / (AdultConversionTime - TeenConversionTime));
                break;
            case TurtleStage.Adult:
                CurrentJunkDistribution = Mathf.Lerp(PostAdultJunkDistribution.Min, PostAdultJunkDistribution.Max, (Time - AdultConversionTime) / (EssentialDeathTime - AdultConversionTime));
                CurrentItemSpawningChance = Mathf.Lerp(PostAdultItemChance.Min, PostAdultItemChance.Max, (Time - AdultConversionTime) / (EssentialDeathTime - AdultConversionTime));
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