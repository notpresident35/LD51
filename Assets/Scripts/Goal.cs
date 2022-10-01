using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour {

    //private BoxCollider2D goalCollider;
    public int team;

    /*
    private void Start() {

        goalCollider = gameObject.GetComponent<BoxCollider2D>();

    }
    */

    private void OnCollisionEnter(Collision collision) {

        ContactPoint contact = collision.contacts[0];
        // do event shit here (pass in team # and collision point)

    }

}
