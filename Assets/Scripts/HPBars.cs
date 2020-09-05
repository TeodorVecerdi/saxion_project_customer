using UnityEngine;
using UnityEngine.UI;
using System;

public class HPBars : MonoBehaviour
{
    public Image healthBar;
    public Image hungerBar;
    [Range(0, 100)]
    public float maxHealth = 100;
    [Range(0, 100)]
    public float maxHunger = 100;
    public float starvationHealthSec;
    public float starvationHungerSec;

    private float currentHunger {
        get => TurtleStats.Instance.Hunger;
        set => TurtleStats.Instance.Hunger = value;
    }
    private float currentHealth {
        get => TurtleStats.Instance.Health;
        set => TurtleStats.Instance.Health = value;
    }

    public static HPBars GetInstance() {
        return instance;
    }
    private static HPBars instance = null;


    private void Awake() {
        if (instance == null)
            instance = this;
        else
            throw new Exception("There can only be one HPBars in a scene!");
    }

    private void OnDestroy(){
        instance = null;
    }

    private void Start() {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        UpdateBars();
    }

    private void Update()
    {
        Starvation(starvationHealthSec, starvationHungerSec);
    }

    public void ChangeHealth(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateBars();
        CheckDeath();
    }

    public void ChangeHunger(float amount) {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
        UpdateBars();
    }

    private void UpdateBars() {
        healthBar.fillAmount = currentHealth / maxHealth;
        hungerBar.fillAmount = currentHunger / maxHunger;
    }

    private void Starvation(float health, float hunger) {
        if (currentHunger == 0f) {
            ChangeHealth(-health * Time.deltaTime);
        }
        else
            ChangeHunger(-hunger * Time.deltaTime);
    }

    private void CheckDeath() {
        if (currentHealth == 0) {
            Debug.Log("u ded");
            Destroy(gameObject);
        }
    }

}
