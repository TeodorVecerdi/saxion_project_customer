using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingNetReflectionProbeFix : MonoBehaviour {
    private void Start() {
        GetComponent<MeshRenderer>().probeAnchor = GameObject.Find("Reflection Probe - Environment").transform;
        Destroy(this);
    }

}
