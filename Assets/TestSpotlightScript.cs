using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpotlightScript : MonoBehaviour {
    // Start is called before the first frame update
    private void Start() {
        Debug.Log("Start");
    }

    private void Awake() {
        Debug.Log("Awake");
    }

    private void OnEnable() {
        Debug.Log("OnEnable");
    }

    private void OnBecameVisible() {
        Debug.Log("OnBecameVisible");
    }

    // Update is called once per frame
    void Update() { }
}