using UnityEngine;

public class Goal : MonoBehaviour {

    public int teamID;

    private void Start () {
        SingletonManager.Instance.GetComponentInChildren<TeamManager> ().RegisterGoal (this, teamID);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "ball") {
            SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnGoalHit.Invoke(teamID, collider.transform.position);
        }
    }

    private void OnDisable () {
        SingletonManager.Instance.GetComponentInChildren<TeamManager> ().DeregisterGoal (this, teamID);
    }
}
