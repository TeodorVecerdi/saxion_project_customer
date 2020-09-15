using UnityEngine;

public static class MathFunctions {
    public static float EaseInOut(float start, float end, float value) {
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    public static float EaseIn(float start, float end, float value) {
        return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
    }

    public static float EaseOut(float start, float end, float value) {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }
}