using UnityEngine;

public class Goal : MonoBehaviour {

    public int teamID;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "ball") {
            SingletonManager.EventSystemInstance.OnGoalHit.Invoke(teamID, collider.transform.position);
        }
    }

    private void OnEnable () {
        SingletonManager.TeamManagerInstance.RegisterGoal (this, teamID);
    }

    private void OnDisable () {
        if (SingletonManager.Instance != null) {
            SingletonManager.TeamManagerInstance.DeregisterGoal (this, teamID);
        }
    }
}
