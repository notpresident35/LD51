using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour {

    public UnityEvent<int, Vector3> goalHit;
    public UnityEvent<Vector3> paddleHit;

}
