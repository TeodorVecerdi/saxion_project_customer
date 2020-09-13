using System;
using TMPro;
using UnityEngine;

    public class TurtleNameController : MonoBehaviour {
        private TMP_InputField inputField;

        private void Awake() {
            inputField = GetComponent<TMP_InputField>();
        }

        private void Start() {
            inputField.text = PlayerPrefs.GetString("name");
        }

        public void UpdateName(string newName) {
            PlayerPrefs.SetString("name", string.IsNullOrEmpty(newName) ? "Unnamed Turtle" : newName);
            PlayerPrefs.Save();
        }
    }