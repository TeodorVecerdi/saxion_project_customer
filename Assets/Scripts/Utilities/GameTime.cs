using UnityEngine;

public static class GameTime {
    public static bool IsPaused = false;
    public static float TimeScale = 1f;

    public static float DeltaTime => IsPaused ? 0f : Time.deltaTime * TimeScale;
    public static float FixedDeltaTime => IsPaused ? 0f : Time.fixedDeltaTime * TimeScale;
    public static float UnscaledDeltaTime => IsPaused ? 0f : Time.deltaTime;
    public static float UnscaledFixedDeltaTime => IsPaused ? 0f : Time.fixedDeltaTime;

    public static float UnpausedDeltaTime => Time.deltaTime * TimeScale;
    public static float UnpausedFixedDeltaTime => Time.fixedDeltaTime * TimeScale;
    public static float UnpausedUnscaledDeltaTime => Time.deltaTime;
    public static float UnpausedUnscaledFixedDeltaTime => Time.fixedDeltaTime;
}