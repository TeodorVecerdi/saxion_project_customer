using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleController : MonoBehaviour {
    public float Speed = 10f;
    void Start() {
        
    }

    void Update() {
        var position = transform.position;
        position += Vector3.forward * (Speed * Time.deltaTime);
        transform.position = position;
    }
}
