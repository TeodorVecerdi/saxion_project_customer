using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TurtleHealth : MonoBehaviour {
    [Header("Health")]
    public float Health;
    public float MaxHealth = 100;
    public float HealthStarvation = 10f;

    [Range(0f, 1f)] public float HealingThreshold = 0.95f;
    public float HealingAmount = 5f;

    [Header("Hunger")]
    public float Hunger;
    public float MaxHunger = 100;
    public float HungerStarvation = 3f;

    private float runScore = 0f;

    public static TurtleHealth Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else throw new Exception("There can only be one TurtleHealth in a scene!");
    }

    private void OnDestroy() {
        Instance = null;
    }

    private void Start() {
        Health = MaxHealth;
        Hunger = MaxHunger;
    }

    private void Update() {
        if (Hunger / MaxHunger >= HealingThreshold) {
            Health += HealingAmount * GameTime.DeltaTime;
        }

        Starvation(HealthStarvation, HungerStarvation);

        Health = Mathf.Clamp(Health, 0f, MaxHealth);
        Hunger = Mathf.Clamp(Hunger, 0f, MaxHunger);
    }

    private void LateUpdate() {
        CheckDeath();
    }

    public void ChangeHealth(float amount) {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
    }

    public void ChangeHunger(float amount) {
        Hunger = Mathf.Clamp(Hunger + amount, 0, MaxHunger);
    }

    private void Starvation(float health, float hunger) {
        if (Hunger == 0f) {
            ChangeHealth(-health * GameTime.DeltaTime);
        } else
            ChangeHunger(-hunger * GameTime.DeltaTime);
    }

    private void CheckDeath() {
        if (Health > 0f)
            return;
        
        runScore = TurtleState.Instance.DistanceTravelled;
        PlayerPrefs.SetFloat("CurrentScore", runScore);
        if (TurtleState.Instance.JustAteTrash) {
            PlayerPrefs.SetString("deathScenario", "Choked on Plastic");
        } else if (TurtleState.Instance.JustGotByPoachers) {
            PlayerPrefs.SetString("deathScenario", "Caught by poachers");
        } else {
            PlayerPrefs.SetString("deathScenario", "Starved to death");
        } 
        PlayerPrefs.Save();
        Debug.Log("u ded");
        SceneManager.LoadScene("DeathMenu", LoadSceneMode.Single);
    }
}