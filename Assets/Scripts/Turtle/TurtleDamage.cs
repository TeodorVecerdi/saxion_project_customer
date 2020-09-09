using UnityEngine;

public class TurtleDamage : MonoBehaviour {
    public DamageVFX DamageVFX;
    [HideInInspector] public bool IsInvincible;
    
    private TurtleHealth turtleHealth;

    private void Start() {
        turtleHealth = TurtleHealth.Instance;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Pickup")) 
            return;

        var spawnableItem = other.GetComponent<SpawnableItem>();
        if(!IsInvincible) turtleHealth.ChangeHealth(spawnableItem.HealthAmount);
        turtleHealth.ChangeHunger(spawnableItem.FoodAmount);
        if(other.gameObject.CompareTag("Junk")) 
            DamageVFX.Trigger(this);
        
        Destroy(other.gameObject);
    }
}