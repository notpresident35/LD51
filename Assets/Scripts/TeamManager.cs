using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public class Team {
        // Note: This stores inverse score, or times other team(s) have scored against this team
        public float Score = 0;
        public List<Paddle> Paddles = new List<Paddle>();
        public List<Goal> Goals = new List<Goal>();
    }

    public List<Team> Teams = new List<Team>();

    public void RegisterPaddle (Paddle paddle, int teamID) {
        while (Teams.Count <= teamID) {
            Teams.Add (new Team ());
        }
        Teams [teamID].Paddles.Add (paddle);
    }

    public void DeregisterPaddle (Paddle paddle, int teamID) {
        Teams [teamID].Paddles.Remove (paddle);
    }

    public void RegisterGoal (Goal goal, int teamID) {
        while (Teams.Count <= teamID) {
            Teams.Add (new Team ());
        }
        Teams [teamID].Goals.Add (goal);
    }
    
    public void DeregisterGoal (Goal goal, int teamID) {
        Teams [teamID].Goals.Remove (goal);
    }

    public void ScorePoint (int teamID, Vector3 pos) {
        if (Teams.Count <= teamID) {
            Debug.LogError ("Tried to score for a team that doesn't exist");
            return;
        }
        Teams [teamID].Score++;
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener (ScorePoint);
    }
    
    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener (ScorePoint);
    }
}
