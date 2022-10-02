using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu (fileName = "NewGameMode", menuName = "Scriptable Objects/Game Mode")]
public class GameMode : ScriptableObject
{
    public string Description;
    public int PerTeamPaddleCount;
    public int BallStartCount;
    public bool UsePowerups;
    [ConditionalField ("UsePowerups")] public float PowerupSpawnDelay;

}/*

[CustomEditor (typeof (GameMode))]
public class MyScriptEditor : Editor {
    override void OnInspectorGUI () {
        GameMode gameMode = target as GameMode;

        gameMode.UsePowerups = GUILayout.Toggle (gameMode.UsePowerups, "Use Powerups");

        if (gameMode.UsePowerups) {
            gameMode.PowerupSpawnDelay = EditorGUILayout.Slider ("Powerup Spawn Delay", gameMode.PowerupSpawnDelay, 1, 100);
        }
    }
}*/

// A game mode with 2 players on separate teams, one paddle per player, and one ball at the start.