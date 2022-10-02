using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    // false, false: between rounds
    // false, true : game just ended
    // true,  false: game is running
    // true,  true : never
    public static bool IsBallActive = false;
    public static bool IsGameComplete = false;

    // paused is independent of ball active and game complete. When unpaused, the game's behavior
    // determined by IsBallActive and IsGameComplete.
    public static bool Paused = false;

    public static GameMode CurrentMode = null;
}
