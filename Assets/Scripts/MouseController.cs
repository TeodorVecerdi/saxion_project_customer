using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    public float MaxHorizontal = 4.5f;
    public float DeadZoneRatio = 0.1f;
    public float Ratio = 0f;

    void Update() {
        var screenSize = Screen.width;
        var deadZoneSize = screenSize * DeadZoneRatio;
        var activeScreenZone = screenSize - 2 * deadZoneSize;
        var mousePosition = Mathf.Clamp(Input.mousePosition.x - screenSize / 2f, -screenSize/2f + deadZoneSize, screenSize/2f - deadZoneSize);
        // var mouseOffset = mousePosition - activeScreenZone / 2f;
        Ratio = mousePosition / (activeScreenZone / 2f);
        var position = transform.position;
        position.x = Ratio * MaxHorizontal;
        transform.position = position;
    }
}