using System.Collections.Generic;
using UnityEngine;

public class ChunkBuilder : MonoBehaviour {
    public List<SpawnableItem> FoodItems;
    public List<SpawnableItem> JunkItems;
    
    public List<Vector3> SpawnRows;
    public float MaxSpawnWidth = 7f;
    public float MaxSpawnHeight = 3f;
    public int MaxItems = 4;
    
    private void Awake() {
        // Find all available spawning rows (to be later used to spawn objects)
        SpawnRows = new List<Vector3>();
        foreach (Transform child in transform) {
            if(child.CompareTag("SpawnRow"))
                SpawnRows.Add(child.position);
        }
    }

    public void BuildChunk() {
        TurtleStats.Instance.UpdateChances();
        foreach (var row in SpawnRows) {
            for (var i = 0; i < MaxItems; i++) {
                if(!MiscUtils.RandomBoolWeighted(TurtleStats.Instance.CurrentItemSpawningChance)) continue;
                
                var itemToSpawn = MiscUtils.ChooseBetweenWithWeight(JunkItems, FoodItems, TurtleStats.Instance.CurrentJunkDistribution).RandomElement();
                var position = new Vector3(UnityEngine.Random.Range(-MaxSpawnWidth/2f, MaxSpawnWidth/2f), UnityEngine.Random.Range(0f, MaxSpawnHeight), row.z);
                Instantiate(itemToSpawn, position, Quaternion.identity, transform);
            }
        }
    }
}