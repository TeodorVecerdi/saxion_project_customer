using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(TurtleTransition))]
public class TurtleState : MonoBehaviour {
    [HideInInspector] public float DistanceTravelled; // TODO: Legacy, remove later
    [HideInInspector] public float Time;
    [Header("Settings")]
    public float TimeSpeed = 2f;
    public float NormalSpeed = 2f;
    public float CurrentSpeed = 2f;
    [Space]
    public StageSettings_Data StageSettings;
    public SingleMinMaxData SpeedOverTime;
    public StagedChanceData JunkDistribution;
    public StagedChanceData ItemSpawningChance;
    public SingleChanceData PoacherChance;

    [HideInInspector] public bool JustAteTrash;
    [HideInInspector] public bool JustGotByPoachers;

    private TurtleTransition turtleTransition;
    private bool hasTransitioned;

    public static TurtleState Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else throw new Exception("There can only be one TurtleStats in a scene!");
        turtleTransition = GetComponent<TurtleTransition>();
    }

    private void Start() {
        CurrentSpeed = NormalSpeed;
        DistanceTravelled = 0f;
        Time = 0f;
    }

    private void Update() {
        DistanceTravelled += CurrentSpeed * GameTime.DeltaTime;
        Time += TimeSpeed * GameTime.DeltaTime;
        // Update stage
        if (Time >= StageSettings.TeenConversionTime && Time < StageSettings.AdultConversionTime) {
            StageSettings.CurrentStage = TurtleStage.Teen;
            if (!hasTransitioned) {
                turtleTransition.TriggerEffect();
                hasTransitioned = true;
            }
        }
        else if (Time >= StageSettings.AdultConversionTime) StageSettings.CurrentStage = TurtleStage.Adult;
        else if (Time >= StageSettings.EssentialDeathTime) StageSettings.CurrentStage = TurtleStage.ShouldDieSoon;
        else StageSettings.CurrentStage = TurtleStage.Hatchling;
        
        // Update junk distribution and item spawn chance
        UpdateMinMaxes();
    }

    public void UpdateMinMaxes() {
        SpeedOverTime.Current = Mathf.Lerp(SpeedOverTime.MinMax.Min, SpeedOverTime.MinMax.Max, Time / StageSettings.EssentialDeathTime);
        NormalSpeed = SpeedOverTime.Current;
        PoacherChance.Current = Mathf.Lerp(PoacherChance.Chance.Min, PoacherChance.Chance.Max, Time / StageSettings.EssentialDeathTime);
        switch (StageSettings.CurrentStage) {
            case TurtleStage.Hatchling:
                JunkDistribution.Current = Mathf.Lerp(JunkDistribution.HatchlingTeen.Min, JunkDistribution.HatchlingTeen.Max, Time / StageSettings.TeenConversionTime);
                ItemSpawningChance.Current = Mathf.Lerp(ItemSpawningChance.HatchlingTeen.Min, ItemSpawningChance.HatchlingTeen.Max, Time / StageSettings.TeenConversionTime);
                break;
            case TurtleStage.Teen:
                JunkDistribution.Current = Mathf.Lerp(JunkDistribution.TeenAdult.Min, JunkDistribution.TeenAdult.Max, (Time - StageSettings.TeenConversionTime) / (StageSettings.AdultConversionTime - StageSettings.TeenConversionTime));
                ItemSpawningChance.Current = Mathf.Lerp(ItemSpawningChance.TeenAdult.Min, ItemSpawningChance.TeenAdult.Max, (Time - StageSettings.TeenConversionTime) / (StageSettings.AdultConversionTime - StageSettings.TeenConversionTime));
                break;
            case TurtleStage.Adult:
                JunkDistribution.Current = Mathf.Lerp(JunkDistribution.PostAdult.Min, JunkDistribution.PostAdult.Max, (Time - StageSettings.AdultConversionTime) / (StageSettings.EssentialDeathTime - StageSettings.AdultConversionTime));
                ItemSpawningChance.Current = Mathf.Lerp(ItemSpawningChance.PostAdult.Min, ItemSpawningChance.PostAdult.Max, (Time - StageSettings.AdultConversionTime) / (StageSettings.EssentialDeathTime - StageSettings.AdultConversionTime));
                break;
            case TurtleStage.ShouldDieSoon:
                JunkDistribution.Current = JunkDistribution.PostAdult.Max;
                ItemSpawningChance.Current = ItemSpawningChance.PostAdult.Max;
                break;
        }
    }

    [Serializable]
    public class StageSettings_Data {
        public TurtleStage CurrentStage;
        public float TeenConversionTime = 150f;
        public float AdultConversionTime = 400f;
        public float EssentialDeathTime = 700f;
    }

    [Serializable]
    public class StagedChanceData {
        [HideInInspector] public float Current = 0f;
        [FormerlySerializedAs("HatchlingTeenChance"),MinMaxSlider(0f, 1f)] public MinMax HatchlingTeen = new MinMax(0f, 1f);
        [FormerlySerializedAs("TeenAdultChance"),MinMaxSlider(0f, 1f)] public MinMax TeenAdult = new MinMax(0f, 1f);
        [FormerlySerializedAs("PostAdultChance"),MinMaxSlider(0f, 1f)] public MinMax PostAdult = new MinMax(0f, 1f);
    }
    [Serializable]
    public class SingleChanceData {
        [HideInInspector] public float Current = 0f;
        [FormerlySerializedAs("MinMax"),MinMaxSlider(0f, 1f)] public MinMax Chance = new MinMax(0f, 1f);
    }

    [Serializable] public class SingleMinMaxData {
        [HideInInspector] public float Current = 0f;
        [MinMaxSlider(0f, 20f)] public MinMax MinMax = new MinMax(0f, 1f);
    }
}


public enum TurtleStage {
    Hatchling,
    Teen,
    Adult,
    ShouldDieSoon
}