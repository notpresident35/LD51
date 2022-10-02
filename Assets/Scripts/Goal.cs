using UnityEngine;

public class Goal : MonoBehaviour {

    public int teamID;

    private void Start () {
        TeamManager.Instance.RegisterGoal (this, teamID);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "ball") {
            EventSystem.Instance.OnGoalHit.Invoke(teamID, collider.transform.position);
        }
    }

    private void OnDisable () {
        TeamManager.Instance.DeregisterGoal (this, teamID);
    }
}
