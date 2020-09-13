using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorActivation : MonoBehaviour {
    private void Start() {
        CursorController.Instance.Game();
    }
}
