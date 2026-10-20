using UnityEngine;

// Holds unlock state that must survive the scene reload on death.
public static class GameProgress
{
    public static bool BirdUnlocked;
    public static bool MouseUnlocked;

    // Explicit reset each play session: with Domain Reload disabled in Enter Play Mode
    // settings, statics would otherwise leak between sessions (owl staying unlocked).
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetOnPlaySessionStart()
    {
        BirdUnlocked = false;
        MouseUnlocked = false;
    }
}
