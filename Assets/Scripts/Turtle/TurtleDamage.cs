using System;
using UnityEngine;

public class TurtleDamage : MonoBehaviour {
    public DamageVFX DamageVFX;
    [HideInInspector] public bool IsInvincible;
    private bool AteTrashOneFrameTrigger;
    public bool JustAteTrash;
    
    private TurtleHealth turtleHealth;

    private void Start() {
        turtleHealth = TurtleHealth.Instance;
    }

    private void Update() {
        if (TurtleStats.Instance.JustAteTrash) {
            TurtleStats.Instance.JustAteTrash = false;
        }
        if (AteTrashOneFrameTrigger) {
            AteTrashOneFrameTrigger = false;
            TurtleStats.Instance.JustAteTrash = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Pickup")) 
            return;

        var spawnableItem = other.GetComponent<SpawnableItem>();
        if(!IsInvincible) turtleHealth.ChangeHealth(spawnableItem.HealthAmount);
        turtleHealth.ChangeHunger(spawnableItem.FoodAmount);
        if (other.gameObject.CompareTag("Junk")) {
            DamageVFX.Trigger(this);
            AteTrashOneFrameTrigger = true;
        }
        
        Destroy(other.gameObject);
    }
}