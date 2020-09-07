using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(FloatingAnimation))]
public class SpawnableItem : MonoBehaviour {
    public float FoodAmount = 10f;
    public float HealthAmount = 10f;
}