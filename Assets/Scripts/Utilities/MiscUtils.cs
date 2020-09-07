using System.Collections.Generic;

public static class MiscUtils {
    public static T RandomElement<T>(this List<T> list) {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static bool RandomBool() {
        return UnityEngine.Random.Range(0f, 1f) < 0.5f;
    }

    public static object ChooseBetween(object first, object second) {
        return RandomBool() ? first : second;
    }
    
    public static List<T> ChooseBetween<T>(List<T> first, List<T> second) {
        return RandomBool() ? first : second;
    }
    
    public static List<T> ChooseBetweenWithWeight<T>(List<T> first, List<T> second, float weight) {
        return UnityEngine.Random.Range(0f, 1f) <= weight ? first : second;
    }
}