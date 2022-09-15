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
        chase.LockMovement();
        yield return new WaitForSeconds(secondsStationary);
        targetLocation = target.position;
        tween = transform.parent.DOMove((Vector3)targetLocation, jumpSpeed).SetSpeedBased().SetEase(Ease.InQuad);
        yield return tween.WaitForCompletion();
        this.enabled = false;
        InvokeOnStateExit(true);
    }
}
