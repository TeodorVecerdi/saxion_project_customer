using UnityEngine;
using UnityEditor;
using System.Collections;

public class CameraCubemap : MonoBehaviour
{
    public Cubemap cubemap;

    private Camera camera;

    public CameraCubemap()
    {
    }

    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
        camera.RenderToCubemap(cubemap);
    }

}
