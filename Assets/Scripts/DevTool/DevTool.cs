using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DevTool : MonoBehaviour {
    public static DevTool Instance;

    [Header("References")]
    public RectTransform TextInputTransform;
    public TMP_InputField ExportInput;

    public DamageVFX DamageVFX;
    public TurtleHealth TurtleHealth;
    public TurtleState TurtleState;
    [FormerlySerializedAs("TurtleBoost")]
    public TurtleMovement TurtleMovement;
    [Space]
    public TMP_InputField FogDensity;
    [Header("DamageVFX")]
    public TMP_InputField EffectDuration;
    public Slider VignetteIntensity;
    public TMP_InputField CameraShakeMag;
    public TMP_InputField InvincibilityTime;
    [Header("TurtleHealth")]
    public TMP_InputField MaxHealth;
    public TMP_InputField MaxHunger;
    public TMP_InputField Starvation;
    public TMP_InputField HealthLoss;
    public Slider HealingThreshold;
    public TMP_InputField HealingAmount;
    [Header("TurtleStats")]
    public TMP_InputField Speed;
    public TMP_InputField TeenDist;
    public TMP_InputField AdultDist;
    public TMP_InputField DeathDist;
    public TMP_InputField HatchlinkJunkMin;
    public TMP_InputField HatchlinkJunkMax;
    public TMP_InputField TeenJunkMin;
    public TMP_InputField TeenJunkMax;
    public TMP_InputField AdultJunkMin;
    public TMP_InputField AdultJunkMax;
    public TMP_InputField HatchlinkItemChanceMin;
    public TMP_InputField HatchlinkItemChanceMax;
    public TMP_InputField TeenItemChanceMin;
    public TMP_InputField TeenItemChanceMax;
    public TMP_InputField AdultItemChanceMin;
    public TMP_InputField AdultItemChanceMax;
    [Header("TurtleBoost")]
    public TMP_InputField BoostSpeed;
    public TMP_InputField BoostDuration;
    public TMP_InputField BoostCooldown;

    private bool isShown;

    private void Awake() {
        DontDestroyOnLoad(transform.parent.gameObject);
        if (Instance != null) {
            Instance.DamageVFX = FindObjectOfType<DamageVFX>();
            Instance.TurtleHealth = FindObjectOfType<TurtleHealth>();
            Instance.TurtleState = FindObjectOfType<TurtleState>();
            Instance.TurtleMovement = FindObjectOfType<TurtleMovement>();
            Instance.UpdateValues();
            Destroy(transform.parent.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        Setup();
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isShown = !isShown;
            GameTime.IsPaused = isShown;
            transform.localScale = isShown ? Vector3.one : Vector3.zero;
        }

        UpdateValues();
    }

    public void OnRestart() {
        SceneManager.LoadScene("Game");
    }

    public void OnExport() {
        TextInputTransform.localScale = Vector3.one;
        var sb = new StringBuilder();
        sb.AppendLine($"RenderSettings.fogDensity: {RenderSettings.fogDensity}");
        sb.AppendLine($"DamageVFX.EffectHalfDuration: {DamageVFX.EffectHalfDuration}");
        sb.AppendLine($"DamageVFX.VignetteIntensity: {DamageVFX.VignetteIntensity}");
        sb.AppendLine($"DamageVFX.CameraShakeMagnitude: {DamageVFX.CameraShakeMagnitude}");
        sb.AppendLine($"DamageVFX.InvincibilityDuration: {DamageVFX.InvincibilityDuration}");
        sb.AppendLine($"TurtleHealth.MaxHealth: {TurtleHealth.MaxHealth}");
        sb.AppendLine($"TurtleHealth.MaxHunger: {TurtleHealth.MaxHunger}");
        sb.AppendLine($"TurtleHealth.HungerStarvation: {TurtleHealth.HungerStarvation}");
        sb.AppendLine($"TurtleHealth.HealthStarvation: {TurtleHealth.HealthStarvation}");
        sb.AppendLine($"TurtleHealth.HealingAmount: {TurtleHealth.HealingAmount}");
        sb.AppendLine($"TurtleHealth.HealingThreshold: {TurtleHealth.HealingThreshold}");
        sb.AppendLine($"TurtleStats.NormalSpeed: {TurtleState.NormalSpeed}");
        sb.AppendLine($"TurtleStats.TeenDistance: {TurtleState.StageSettings.TeenConversionTime}");
        sb.AppendLine($"TurtleStats.AdultDistance: {TurtleState.StageSettings.AdultConversionTime}");
        sb.AppendLine($"TurtleStats.EssentialDeathDistance: {TurtleState.StageSettings.EssentialDeathTime}");
        sb.AppendLine($"TurtleStats.HatchlingTeenJunkDistribution.Min: {TurtleState.JunkDistribution.HatchlingTeen.Min}");
        sb.AppendLine($"TurtleStats.HatchlingTeenJunkDistribution.Max: {TurtleState.JunkDistribution.HatchlingTeen.Max}");
        sb.AppendLine($"TurtleStats.TeenAdultJunkDistribution.Min: {TurtleState.JunkDistribution.TeenAdult.Min}");
        sb.AppendLine($"TurtleStats.TeenAdultJunkDistribution.Max: {TurtleState.JunkDistribution.TeenAdult.Max}");
        sb.AppendLine($"TurtleStats.PostAdultJunkDistribution.Min: {TurtleState.JunkDistribution.PostAdult.Min}");
        sb.AppendLine($"TurtleStats.PostAdultJunkDistribution.Max: {TurtleState.JunkDistribution.PostAdult.Max}");
        sb.AppendLine($"TurtleStats.HatchlingTeenItemChance.Min: {TurtleState.ItemSpawningChance.HatchlingTeen.Min}");
        sb.AppendLine($"TurtleStats.HatchlingTeenItemChance.Max: {TurtleState.ItemSpawningChance.HatchlingTeen.Max}");
        sb.AppendLine($"TurtleStats.TeenAdultItemChance.Min: {TurtleState.ItemSpawningChance.TeenAdult.Min}");
        sb.AppendLine($"TurtleStats.TeenAdultItemChance.Max: {TurtleState.ItemSpawningChance.TeenAdult.Max}");
        sb.AppendLine($"TurtleStats.PostAdultItemChance.Min: {TurtleState.ItemSpawningChance.PostAdult.Min}");
        sb.AppendLine($"TurtleStats.PostAdultItemChance.Max: {TurtleState.ItemSpawningChance.PostAdult.Max}");
        sb.AppendLine($"TurtleBoost.BoostSpeed: {TurtleMovement.BoostSettings.Speed}");
        sb.AppendLine($"TurtleBoost.BoostDuration: {TurtleMovement.BoostSettings.Duration}");
        sb.AppendLine($"TurtleBoost.BoostCooldown: {TurtleMovement.BoostSettings.Cooldown}");
        ExportInput.text = sb.ToString();
    }

    public void OnBack() {
        TextInputTransform.localScale = Vector3.zero;
        ExportInput.text = "";
    }

    private void Setup() {
        FogDensity.text = $"{RenderSettings.fogDensity:0.00}";

        EffectDuration.text = $"{DamageVFX.EffectHalfDuration * 2f:0.00}";
        VignetteIntensity.value = DamageVFX.VignetteIntensity;
        CameraShakeMag.text = $"{DamageVFX.CameraShakeMagnitude:0.00}";
        InvincibilityTime.text = $"{DamageVFX.InvincibilityDuration:0.00}";

        MaxHealth.text = $"{TurtleHealth.MaxHealth:0.00}";
        MaxHunger.text = $"{TurtleHealth.MaxHunger:0.00}";
        Starvation.text = $"{TurtleHealth.HungerStarvation:0.00}";
        HealthLoss.text = $"{TurtleHealth.HealthStarvation:0.00}";
        HealingAmount.text = $"{TurtleHealth.HealingAmount:0.00}";
        HealingThreshold.value = TurtleHealth.HealingThreshold;

        Speed.text = $"{TurtleState.NormalSpeed:0.00}";
        TeenDist.text = $"{TurtleState.StageSettings.TeenConversionTime:0.00}";
        AdultDist.text = $"{TurtleState.StageSettings.AdultConversionTime:0.00}";
        DeathDist.text = $"{TurtleState.StageSettings.EssentialDeathTime:0.00}";
        HatchlinkJunkMin.text = $"{TurtleState.JunkDistribution.HatchlingTeen.Min:0.00}";
        HatchlinkJunkMax.text = $"{TurtleState.JunkDistribution.HatchlingTeen.Max:0.00}";
        TeenJunkMin.text = $"{TurtleState.JunkDistribution.TeenAdult.Min:0.00}";
        TeenJunkMax.text = $"{TurtleState.JunkDistribution.TeenAdult.Max:0.00}";
        AdultJunkMin.text = $"{TurtleState.JunkDistribution.PostAdult.Min:0.00}";
        AdultJunkMax.text = $"{TurtleState.JunkDistribution.PostAdult.Max:0.00}";
        HatchlinkItemChanceMin.text = $"{TurtleState.ItemSpawningChance.HatchlingTeen.Min:0.00}";
        HatchlinkItemChanceMax.text = $"{TurtleState.ItemSpawningChance.HatchlingTeen.Max:0.00}";
        TeenItemChanceMin.text = $"{TurtleState.ItemSpawningChance.TeenAdult.Min:0.00}";
        TeenItemChanceMax.text = $"{TurtleState.ItemSpawningChance.TeenAdult.Max:0.00}";
        AdultItemChanceMin.text = $"{TurtleState.ItemSpawningChance.PostAdult.Min:0.00}";
        AdultItemChanceMax.text = $"{TurtleState.ItemSpawningChance.PostAdult.Max:0.00}";

        BoostSpeed.text = $"{TurtleMovement.BoostSettings.Speed:0.00}";
        BoostDuration.text = $"{TurtleMovement.BoostSettings.Duration:0.00}";
        BoostCooldown.text = $"{TurtleMovement.BoostSettings.Cooldown:0.00}";
    }

    private void UpdateValues() {
        RenderSettings.fogDensity = float.Parse(FogDensity.text);

        DamageVFX.EffectHalfDuration = float.Parse(EffectDuration.text) / 2f;
        DamageVFX.VignetteIntensity = VignetteIntensity.value;
        DamageVFX.CameraShakeMagnitude = float.Parse(CameraShakeMag.text);
        DamageVFX.InvincibilityDuration = float.Parse(InvincibilityTime.text);
        TurtleHealth.MaxHealth = float.Parse(MaxHealth.text);
        TurtleHealth.MaxHunger = float.Parse(MaxHunger.text);
        TurtleHealth.HungerStarvation = float.Parse(Starvation.text);
        TurtleHealth.HealthStarvation = float.Parse(HealthLoss.text);
        TurtleHealth.HealingAmount = float.Parse(HealingAmount.text);
        TurtleHealth.HealingThreshold = HealingThreshold.value;
        TurtleState.NormalSpeed = float.Parse(Speed.text);
        TurtleState.StageSettings.TeenConversionTime = float.Parse(TeenDist.text);
        TurtleState.StageSettings.AdultConversionTime = float.Parse(AdultDist.text);
        TurtleState.StageSettings.EssentialDeathTime = float.Parse(DeathDist.text);
        TurtleState.JunkDistribution.HatchlingTeen.Min = float.Parse(HatchlinkJunkMin.text);
        TurtleState.JunkDistribution.HatchlingTeen.Max = float.Parse(HatchlinkJunkMax.text);
        TurtleState.JunkDistribution.TeenAdult.Min = float.Parse(TeenJunkMin.text);
        TurtleState.JunkDistribution.TeenAdult.Max = float.Parse(TeenJunkMax.text);
        TurtleState.JunkDistribution.PostAdult.Min = float.Parse(AdultJunkMin.text);
        TurtleState.JunkDistribution.PostAdult.Max = float.Parse(AdultJunkMax.text);
        TurtleState.ItemSpawningChance.HatchlingTeen.Min = float.Parse(HatchlinkItemChanceMin.text);
        TurtleState.ItemSpawningChance.HatchlingTeen.Max = float.Parse(HatchlinkItemChanceMax.text);
        TurtleState.ItemSpawningChance.TeenAdult.Min = float.Parse(TeenItemChanceMin.text);
        TurtleState.ItemSpawningChance.TeenAdult.Max = float.Parse(TeenItemChanceMax.text);
        TurtleState.ItemSpawningChance.PostAdult.Min = float.Parse(AdultItemChanceMin.text);
        TurtleState.ItemSpawningChance.PostAdult.Max = float.Parse(AdultItemChanceMax.text);
        TurtleMovement.BoostSettings.Speed = float.Parse(BoostSpeed.text);
        TurtleMovement.BoostSettings.Duration = float.Parse(BoostDuration.text);
        TurtleMovement.BoostSettings.Cooldown = float.Parse(BoostCooldown.text);
    }
}