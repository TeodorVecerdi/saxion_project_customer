using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    [Header("Chunk Settings")]
    public ChunkBuilder ChunkPrefab;
    public Vector2 ChunkSize = new Vector2(7, 5);
    public float MinZ = -5;
    public float MaxZ = 50;
    [Tooltip("Number of chunks to skip at the beginning. (When first starting the game)")] public int SkipChunks = 4;

    public Queue<ChunkBuilder> ActiveChunks;
    private ChunkBuilder lastChunk;

    private void OnDrawGizmos() {
        var totalZ = Mathf.Abs(MinZ) + Mathf.Abs(MaxZ);
        var totalChunks = (int) (totalZ / ChunkSize.y);

        // draw extremities
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(new Vector3(0, 0, MinZ - ChunkSize.y / 2f), new Vector3(ChunkSize.x, 1f, .5f));
        Gizmos.DrawCube(new Vector3(0, 0, MaxZ - ChunkSize.y / 2f), new Vector3(ChunkSize.x, 1f, .5f));
        Gizmos.color = Color.red;
        for (var z = MinZ; z < MaxZ; z += ChunkSize.y) {
            Gizmos.DrawWireCube(new Vector3(0, 0, z), new Vector3(ChunkSize.x, 1f, ChunkSize.y));
        }
    }

    private void Start() {
        RebuildChunks();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DestroyAllChunks();
            RebuildChunks();
            return;
        }
        
        // Move all chunks towards the player
        foreach (var chunk in ActiveChunks) {
            var currentPosition = chunk.transform.position;
            currentPosition.z += -TurtleStats.Instance.CurrentSpeed * GameTime.DeltaTime;
            chunk.transform.position = currentPosition;
        }

        // Destroy the first chunk if it's out of bounds
        var firstChunk = ActiveChunks.Peek();
        if (firstChunk.transform.position.z > MinZ - ChunkSize.y)
            return;
        Destroy(ActiveChunks.Dequeue().gameObject);
        
        // Spawn new chunk at the end
        var lastChunkPosition = lastChunk.transform.position;
        var newChunk = Instantiate(ChunkPrefab,new Vector3(0, 0, ChunkSize.y) + lastChunkPosition, Quaternion.identity, transform);
        newChunk.BuildChunk();
        ActiveChunks.Enqueue(newChunk);
        lastChunk = newChunk;

    }

    private void DestroyAllChunks() {
        foreach (var chunk in ActiveChunks) {
            Destroy(chunk.gameObject);
        }
        ActiveChunks.Clear();
    }

    private void RebuildChunks() {
        ActiveChunks = new Queue<ChunkBuilder>();
        for (var z = MinZ + SkipChunks * ChunkSize.y; z < MaxZ + SkipChunks * ChunkSize.y; z += ChunkSize.y) {
            var chunk = Instantiate(ChunkPrefab, new Vector3(0, 0, z), Quaternion.identity, transform);
            chunk.BuildChunk();
            ActiveChunks.Enqueue(chunk);
            lastChunk = chunk;
        }
    }
}