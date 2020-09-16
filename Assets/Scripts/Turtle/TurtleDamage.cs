using System;
using UnityEngine;

[RequireComponent(typeof(TurtleMovement))]
public class TurtleDamage : MonoBehaviour {
    public DamageVFX DamageVFX;
    [HideInInspector] public bool IsInvincible;
    private bool ateTrashOneFrameTrigger;
    private bool gotByPoachersOneFrameTrigger;
    
    private TurtleMovement turtleMovement;
    private TurtleHealth turtleHealth;

    private void Awake() {
        turtleMovement = GetComponent<TurtleMovement>();
    }

    private void Start() {
        turtleHealth = TurtleHealth.Instance;
    }

    private void Update() {
        if (TurtleState.Instance.JustAteTrash) {
            TurtleState.Instance.JustAteTrash = false;
        }

        if (TurtleState.Instance.JustGotByPoachers) {
            TurtleState.Instance.JustGotByPoachers = false;
        }
        if (ateTrashOneFrameTrigger) {
            ateTrashOneFrameTrigger = false;
            TurtleState.Instance.JustAteTrash = true;
        }
        if (gotByPoachersOneFrameTrigger) {
            gotByPoachersOneFrameTrigger = false;
            TurtleState.Instance.JustGotByPoachers = true;
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
            SoundManager.PlaySound("damage");
        } else if (other.gameObject.CompareTag("Poacher")) {
            DamageVFX.Trigger(this, extreme: true);
            turtleMovement.TriggerSlowness();
            gotByPoachersOneFrameTrigger = true;
            SoundManager.PlaySound("damage");
        }
        Destroy(other.gameObject);
    }
}