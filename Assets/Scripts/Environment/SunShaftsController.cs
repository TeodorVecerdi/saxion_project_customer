using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class SunShaftsController : MonoBehaviour {
    [Header("Settings")]
    public SunShaft SunShaftPrefab;
    [Space]
    public float StartZ = 40f;
    public float StopFadeZ = 30f;
    public float EndZ = -25f;
    [Space]
    public float StartX = -2;
    public float EndX = 17f;
    [Space]
    public float StartFadeAlpha = 0f;
    public float EndFadeAlpha = 10f;
    [Space]
    public int TotalSunShafts = 10;
    public float SunShaftZVariation = 2f;

    private readonly Queue<SunShaft> sunShafts = new Queue<SunShaft>();
    
    private void Start() {
        var spacing = (StartZ - EndZ) / TotalSunShafts;
        var currentPosition = EndZ;
        for (var i = 0; i < TotalSunShafts; i++) {
            var sunShaft = CreateSunShaft(currentPosition);
            sunShafts.Enqueue(sunShaft);
            currentPosition += spacing;
        }

    }

    // Update is called once per frame
    private void Update() {
        var frontSunShaft = sunShafts.Peek();
        if (frontSunShaft.transform.position.z <= EndZ) {
            sunShafts.Dequeue();
            Destroy(frontSunShaft.gameObject);
            var newSunShaft = CreateSunShaft(StartZ);
            sunShafts.Enqueue(newSunShaft);
        }
    }

    private SunShaft CreateSunShaft(float position) {
        var x = UnityEngine.Random.Range(StartX, EndX);
        var z = position + UnityEngine.Random.Range(-SunShaftZVariation, SunShaftZVariation);
        var sunShaft = Instantiate(SunShaftPrefab, new Vector3(x, 25f, z), Quaternion.Euler(50, -30, 0), transform);
        sunShaft.Controller = this;
        return sunShaft;
    }
}
