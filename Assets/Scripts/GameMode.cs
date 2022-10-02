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

    public float ballStartAngleSpread;
    public bool ballStartsWithRandomDirection;
    [ConditionalField ("ballStartsWithRandomDirection", true)] public bool ballStartsGoingLeft;

    public List<Vector3> BallDefaultPositions;
    public List<Vector3> PaddleDefaultPositions;
}