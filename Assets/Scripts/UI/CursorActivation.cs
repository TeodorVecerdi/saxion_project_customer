using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorActivation : MonoBehaviour {
    public Texture2D CursorTexture;

    private void Start() {
        Cursor.SetCursor(CursorTexture, new Vector2(CursorTexture.width/2f, CursorTexture.height/2f), CursorMode.ForceSoftware);
    }
}
