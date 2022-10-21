using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlimeJump : State
{
    private IEnumerator enumerator;
    private Tween tween;
    private Vector2 targetLocation;

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Chase chase;

    [SerializeField]
    private float secondsStationary;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float jumpOffset;
    private void OnEnable()
    {
        enumerator = CoroutineManager.StartGlobalCoroutine(AttackCoroutine(target));
        anim.Play("Base.Slime_Jump", 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        CoroutineManager.StopGlobalCoroutine(enumerator);
        tween.Kill();
        chase.UnlockMovement();
    }

    private IEnumerator AttackCoroutine(Transform target)
    {
        if (target != null)
        {
            chase.LockMovement();
            yield return new WaitForSeconds(secondsStationary);
            targetLocation = Vector2.Lerp(transform.position, target.position, jumpOffset);
            Debug.DrawLine(transform.position, targetLocation, Color.green, 1.0f);
            tween = transform.parent.GetComponent<Rigidbody2D>().DOMove((Vector3)targetLocation, jumpSpeed).SetSpeedBased().SetEase(Ease.InQuad);
            tween.SetUpdate(UpdateType.Fixed);
            yield return tween.WaitForCompletion();
            this.enabled = false;
            InvokeOnStateExit(true);
        }
    }
}
