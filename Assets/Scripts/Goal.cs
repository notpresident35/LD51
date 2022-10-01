using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour {

    public int team;

    private void OnCollisionEnter(Collision collision) {

        ContactPoint contact = collision.contacts[0];
        EventSystem.Instance.goalHit.Invoke(team, contact.point);

    }

}
