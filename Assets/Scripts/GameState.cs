using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // false, false: between rounds
    // false, true : game just ended
    // true,  false: game is running
    // true,  true : never
    public bool IsBallActive;
    public bool IsGameComplete;

    // paused is independent of ball active and game complete. When unpaused, the game's behavior
    // determined by IsBallActive and IsGameComplete.
    public bool paused;
    
    public GameState(bool IsBallActive, bool IsGameComplete)
    {
        this.IsBallActive = IsBallActive;
        this.IsGameComplete = IsGameComplete;
    }
}
