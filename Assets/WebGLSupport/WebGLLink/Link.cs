// SOURCE: https://github.com/valyard/UnityWebGLOpenLink

using System.Runtime.InteropServices;
using UnityEngine;

public class Link : MonoBehaviour {
    public string URL;

    public void OpenLink() {
        #if UNITY_EDITOR
            Application.OpenURL(URL);
        #else
            openWindow(URL);
        #endif
    }
    
    [DllImport("__Internal")]
    private static extern void openWindow(string url);
}