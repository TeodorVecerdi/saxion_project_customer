using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Taken from https://forum.unity.com/threads/ui-inverse-mask.342324/#post-5691217
public class InvertedMaskImage : Image {
    private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");
    
    public override Material materialForRendering {
        get {
            // Copy material so you don't fuck up everything
            var result = new Material(base.materialForRendering);
            
            result.SetInt(StencilComp, (int) CompareFunction.NotEqual);
            return result;
        }
    }
}