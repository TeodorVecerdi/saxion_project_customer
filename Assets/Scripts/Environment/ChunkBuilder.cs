using System.Collections.Generic;
using UnityEngine;

public class ChunkBuilder : MonoBehaviour {
    public List<SpawnableItem> FoodItems;
    public List<SpawnableItem> JunkItems;
    public List<SpawnableItem> PoacherItems;

    public List<Vector3> SpawnRows;
    public float MaxSpawnWidth = 4f;
    public float MaxSpawnHeight = 2f;
    public int MaxItemsPerChunk = 8;

    private void Awake() {
        // Find all available spawning rows (to be later used to spawn objects)
        SpawnRows = new List<Vector3>();
        foreach (Transform child in transform) {
            if (child.CompareTag("SpawnRow"))
                SpawnRows.Add(child.position);
        }
    }

    public void BuildChunk() {
        TurtleState.Instance.UpdateMinMaxes();
        for (var i = 0; i < MaxItemsPerChunk; i++) {
            if (!MiscUtils.RandomBoolWeighted(TurtleState.Instance.ItemSpawningChance.Current)) continue;
            var row = SpawnRows.RandomElement();

            var junkOrFood = MiscUtils.ChooseBetweenWithWeight(JunkItems, FoodItems, TurtleState.Instance.JunkDistribution.Current);
            var orPoacher = MiscUtils.ChooseBetweenWithWeight(PoacherItems, junkOrFood, TurtleState.Instance.PoacherChance.Current);
            var item = orPoacher.RandomElement();
            var position = new Vector3(UnityEngine.Random.Range(-MaxSpawnWidth / 2f, MaxSpawnWidth / 2f), UnityEngine.Random.Range(0f, MaxSpawnHeight), row.z);
            Instantiate(item, position, Quaternion.identity, transform);
        }
    }
}