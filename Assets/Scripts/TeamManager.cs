using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public class Team {
        public float Score;
        public List<Paddle> Paddles;
        public List<Goal> Goals;
    }

    public List<Team> Teams = new List<Team>();

    public void RegisterPaddle (Paddle paddle, int teamID) {
        Teams [teamID].Paddles.Add (paddle);
    }

    public void RegisterGoal (Goal goal, int teamID) {
        Teams [teamID].Goals.Add (goal);
    }

    public void ScorePoint (int teamID) {
        Teams [teamID].Score++;
    }
}
