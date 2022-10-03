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
    [SerializeField] GameObject CurveArrows;

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

    public void SetupCurveIndicators (bool facingRight) {
        Vector3 pos = CurveArrows.transform.localPosition;
        pos.x = pos.x * (facingRight ? 1.0f : -1.0f);
        CurveArrows.transform.localPosition = pos;
    }

    public void ShowCurveIndicators () {
        CurveArrows.SetActive (true);
    }

    public void HideCurveIndicators () {
        CurveArrows.SetActive (false);
    }
}
