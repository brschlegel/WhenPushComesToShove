using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveToClick : MonoBehaviour
{
    public float force;
    public Walk subject;
    public float knockbackTime;
    public VelocitySetter vs;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            subject.MoveTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if(Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Rigidbody2D rb = subject.GetComponent<Rigidbody2D>();
            var v = ((Vector2)subject.transform.position - mousePos);

            Vector2 knockback = v.normalized * force;
            Tween t = DOVirtual.Float(force, 0, knockbackTime, f=> vs.UpdateVelocityMagnitude("MouseClick", f)).SetEase(Ease.OutBack);
            vs.AddSourceTween("MouseClick",knockback, t );
        }
    }
}
