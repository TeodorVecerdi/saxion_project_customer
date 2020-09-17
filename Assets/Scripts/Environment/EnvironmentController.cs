using System;
using UnityEngine;

    public class EnvironmentController : MonoBehaviour {
        public float ResetDistance = 5f;
        public Material CausticsMaterial;
        private static readonly int Distance = Shader.PropertyToID("_Distance");

        private void Update() {
            var currentDistance = CausticsMaterial.GetFloat(Distance);
            if (transform.position.z <= -ResetDistance) {
                transform.position += new Vector3(0f, 0f, ResetDistance);
                currentDistance -= ResetDistance;
            }
            
            CausticsMaterial.SetFloat("_Distance", currentDistance + TurtleState.Instance.CurrentSpeed * GameTime.DeltaTime);
            transform.position -= new Vector3(0f, 0f, TurtleState.Instance.CurrentSpeed * GameTime.DeltaTime);
        }
    }