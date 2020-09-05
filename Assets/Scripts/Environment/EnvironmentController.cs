using System;
using UnityEngine;

    public class EnvironmentController : MonoBehaviour {
        public float ResetDistance = 5f;

        private void Update() {
            if (transform.position.z <= -ResetDistance) {
                transform.position += new Vector3(0f, 0f, ResetDistance);
            }
            transform.position -= new Vector3(0f, 0f, TurtleStats.Instance.CurrentSpeed * Time.deltaTime);
        }
    }