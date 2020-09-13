using System;
using UnityEngine;

public class TurtleDamage : MonoBehaviour {
    public DamageVFX DamageVFX;
    [HideInInspector] public bool IsInvincible;
    private bool ateTrashOneFrameTrigger;
    private bool gotByPoachersOneFrameTrigger;
    
    private TurtleHealth turtleHealth;

    private void Start() {
        turtleHealth = TurtleHealth.Instance;
    }

    private void Update() {
        if (TurtleStats.Instance.JustAteTrash) {
            TurtleStats.Instance.JustAteTrash = false;
        }

        if (TurtleStats.Instance.JustGotByPoachers) {
            TurtleStats.Instance.JustGotByPoachers = false;
        }
        if (ateTrashOneFrameTrigger) {
            ateTrashOneFrameTrigger = false;
            TurtleStats.Instance.JustAteTrash = true;
        }
        if (gotByPoachersOneFrameTrigger) {
            gotByPoachersOneFrameTrigger = false;
            TurtleStats.Instance.JustGotByPoachers = true;
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
            ateTrashOneFrameTrigger = true;
        } else if (other.gameObject.CompareTag("Poacher")) {
            DamageVFX.Trigger(this);
            gotByPoachersOneFrameTrigger = true;
        }
        Destroy(other.gameObject);
    }
}