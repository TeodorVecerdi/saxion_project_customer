using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform Target;
    public Vector3 Offset;
    public float SmoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void Start() {
        Offset = transform.position - Target.position;
    }

    private void LateUpdate() {
        var targetPosition = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
    }
}