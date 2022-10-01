using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour {

    public int team;

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "ball") {
            print("hit ball");
            EventSystem.Instance.goalHit.Invoke(team, collider.transform.position);

        }

    }

}
