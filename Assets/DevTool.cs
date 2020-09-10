using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevTool : MonoBehaviour {
    public static DevTool Instance;

    [Header("References")]
    public RectTransform TextInputTransform;
    public TMP_InputField ExportInput;
    
    public DamageVFX DamageVFX;
    public TurtleHealth TurtleHealth;
    public TurtleStats TurtleStats;
    public TurtleBoost TurtleBoost;
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
        if(Instance != null) DestroyImmediate(transform.parent.gameObject);
        Instance = this;
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    private void Start() {
        Setup();
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
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
        sb.AppendLine($"TurtleStats.NormalSpeed: {TurtleStats.NormalSpeed}");
        sb.AppendLine($"TurtleStats.TeenDistance: {TurtleStats.TeenDistance}");
        sb.AppendLine($"TurtleStats.AdultDistance: {TurtleStats.AdultDistance}");
        sb.AppendLine($"TurtleStats.EssentialDeathDistance: {TurtleStats.EssentialDeathDistance}");
        sb.AppendLine($"TurtleStats.HatchlingTeenJunkDistribution.Min: {TurtleStats.HatchlingTeenJunkDistribution.Min}");
        sb.AppendLine($"TurtleStats.HatchlingTeenJunkDistribution.Max: {TurtleStats.HatchlingTeenJunkDistribution.Max}");
        sb.AppendLine($"TurtleStats.TeenAdultJunkDistribution.Min: {TurtleStats.TeenAdultJunkDistribution.Min}");
        sb.AppendLine($"TurtleStats.TeenAdultJunkDistribution.Max: {TurtleStats.TeenAdultJunkDistribution.Max}");
        sb.AppendLine($"TurtleStats.PostAdultJunkDistribution.Min: {TurtleStats.PostAdultJunkDistribution.Min}");
        sb.AppendLine($"TurtleStats.PostAdultJunkDistribution.Max: {TurtleStats.PostAdultJunkDistribution.Max}");
        sb.AppendLine($"TurtleStats.HatchlingTeenItemChance.Min: {TurtleStats.HatchlingTeenItemChance.Min}");
        sb.AppendLine($"TurtleStats.HatchlingTeenItemChance.Max: {TurtleStats.HatchlingTeenItemChance.Max}");
        sb.AppendLine($"TurtleStats.TeenAdultItemChance.Min: {TurtleStats.TeenAdultItemChance.Min}");
        sb.AppendLine($"TurtleStats.TeenAdultItemChance.Max: {TurtleStats.TeenAdultItemChance.Max}");
        sb.AppendLine($"TurtleStats.PostAdultItemChance.Min: {TurtleStats.PostAdultItemChance.Min}");
        sb.AppendLine($"TurtleStats.PostAdultItemChance.Max: {TurtleStats.PostAdultItemChance.Max}");
        sb.AppendLine($"TurtleBoost.BoostSpeed: {TurtleBoost.BoostSpeed}");
        sb.AppendLine($"TurtleBoost.BoostDuration: {TurtleBoost.BoostDuration}");
        sb.AppendLine($"TurtleBoost.BoostCooldown: {TurtleBoost.BoostCooldown}");
        ExportInput.text = sb.ToString();
    }

    public void OnBack() {
        TextInputTransform.localScale = Vector3.zero;
        ExportInput.text = "";
    }

    private void Setup() {
        EffectDuration.text = $"{DamageVFX.EffectHalfDuration * 2f:.00}";
        VignetteIntensity.value = DamageVFX.VignetteIntensity;
        CameraShakeMag.text = $"{DamageVFX.CameraShakeMagnitude:.00}";
        InvincibilityTime.text = $"{DamageVFX.InvincibilityDuration:.00}";

        MaxHealth.text = $"{TurtleHealth.MaxHealth:.00}";
        MaxHunger.text = $"{TurtleHealth.MaxHunger:.00}";
        Starvation.text = $"{TurtleHealth.HungerStarvation:.00}";
        HealthLoss.text = $"{TurtleHealth.HealthStarvation:.00}";
        HealingAmount.text = $"{TurtleHealth.HealingAmount:.00}";
        HealingThreshold.value = TurtleHealth.HealingThreshold;

        Speed.text = $"{TurtleStats.NormalSpeed:.00}";
        TeenDist.text = $"{TurtleStats.TeenDistance:.00}";
        AdultDist.text = $"{TurtleStats.AdultDistance:.00}";
        DeathDist.text = $"{TurtleStats.EssentialDeathDistance:.00}";
        HatchlinkJunkMin.text = $"{TurtleStats.HatchlingTeenJunkDistribution.Min:0.00}";
        HatchlinkJunkMax.text = $"{TurtleStats.HatchlingTeenJunkDistribution.Max:0.00}";
        TeenJunkMin.text = $"{TurtleStats.TeenAdultJunkDistribution.Min:0.00}";
        TeenJunkMax.text = $"{TurtleStats.TeenAdultJunkDistribution.Max:0.00}";
        AdultJunkMin.text = $"{TurtleStats.PostAdultJunkDistribution.Min:0.00}";
        AdultJunkMax.text = $"{TurtleStats.PostAdultJunkDistribution.Max:0.00}";
        HatchlinkItemChanceMin.text = $"{TurtleStats.HatchlingTeenItemChance.Min:0.00}";
        HatchlinkItemChanceMax.text = $"{TurtleStats.HatchlingTeenItemChance.Max:0.00}";
        TeenItemChanceMin.text = $"{TurtleStats.TeenAdultItemChance.Min:0.00}";
        TeenItemChanceMax.text = $"{TurtleStats.TeenAdultItemChance.Max:0.00}";
        AdultItemChanceMin.text = $"{TurtleStats.PostAdultItemChance.Min:0.00}";
        AdultItemChanceMax.text = $"{TurtleStats.PostAdultItemChance.Max:0.00}";

        BoostSpeed.text = $"{TurtleBoost.BoostSpeed:.00}";
        BoostDuration.text = $"{TurtleBoost.BoostDuration:.00}";
        BoostCooldown.text = $"{TurtleBoost.BoostCooldown:.00}";
    }

    private void UpdateValues() {
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
        TurtleStats.NormalSpeed = float.Parse(Speed.text);
        TurtleStats.TeenDistance = float.Parse(TeenDist.text);
        TurtleStats.AdultDistance = float.Parse(AdultDist.text);
        TurtleStats.EssentialDeathDistance = float.Parse(DeathDist.text);
        TurtleStats.HatchlingTeenJunkDistribution.Min = float.Parse(HatchlinkJunkMin.text);
        TurtleStats.HatchlingTeenJunkDistribution.Max = float.Parse(HatchlinkJunkMax.text);
        TurtleStats.TeenAdultJunkDistribution.Min = float.Parse(TeenJunkMin.text);
        TurtleStats.TeenAdultJunkDistribution.Max = float.Parse(TeenJunkMax.text);
        TurtleStats.PostAdultJunkDistribution.Min = float.Parse(AdultJunkMin.text);
        TurtleStats.PostAdultJunkDistribution.Max = float.Parse(AdultJunkMax.text);
        TurtleStats.HatchlingTeenItemChance.Min = float.Parse(HatchlinkItemChanceMin.text);
        TurtleStats.HatchlingTeenItemChance.Max = float.Parse(HatchlinkItemChanceMax.text);
        TurtleStats.TeenAdultItemChance.Min = float.Parse(TeenItemChanceMin.text);
        TurtleStats.TeenAdultItemChance.Max = float.Parse(TeenItemChanceMax.text);
        TurtleStats.PostAdultItemChance.Min = float.Parse(AdultItemChanceMin.text);
        TurtleStats.PostAdultItemChance.Max = float.Parse(AdultItemChanceMax.text);
        TurtleBoost.BoostSpeed = float.Parse(BoostSpeed.text);
        TurtleBoost.BoostDuration = float.Parse(BoostDuration.text);
        TurtleBoost.BoostCooldown = float.Parse(BoostCooldown.text);
    }
}