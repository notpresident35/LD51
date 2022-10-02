using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;

    private void Awake () {
        if (Instance) {
            Destroy (gameObject);
            return;
        } else {
            DontDestroyOnLoad (gameObject);
            Instance = this;
        }
    }

    public class Team {
        public float Score;
        public List<Paddle> Paddles;
        public List<Goal> Goals;
    }

    public List<Team> Teams = new List<Team>();

    public void RegisterPaddle (Paddle paddle, int teamID) {
        Teams [teamID].Paddles.Add (paddle);
    }

    public void DeregisterPaddle (Paddle paddle, int teamID) {
        Teams [teamID].Paddles.Remove (paddle);
    }

    public void RegisterGoal (Goal goal, int teamID) {
        Teams [teamID].Goals.Add (goal);
    }
    
    public void DeregisterGoal (Goal goal, int teamID) {
        Teams [teamID].Goals.Remove (goal);
    }

    public void ScorePoint (int teamID) {
        Teams [teamID].Score++;
    }
}
