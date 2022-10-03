using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleVisuals : MonoBehaviour
{
    private Color BaseColor;
    [SerializeField]
    private Gradient ChargingColorGradient;
    [SerializeField]
    private Color FullChargedColor;

    private float charge = 0;
    private SpriteRenderer sprite;

    private void Awake () {
        sprite = GetComponent<SpriteRenderer> ();
    }

    private void Start () {
        BaseColor = sprite.color;
    }

    private void Update () {
        if (charge < Mathf.Epsilon) {
            sprite.color = BaseColor;
        } else if (charge < 1f) {
            sprite.color = ChargingColorGradient.Evaluate (charge);
        } else {
            sprite.color = FullChargedColor;
        }
    }

    public void SetCharge (float newCharge) {
        charge = newCharge;
    }

    public void StrikeBall (bool hardHit, bool curved) {
        if (hardHit) {

        } else {

        }

        if (curved) {

        } else {

        }
    }
}
