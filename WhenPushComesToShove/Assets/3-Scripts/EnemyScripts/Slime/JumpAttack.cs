using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpAttack : EnemyAttack
{

    private Vector2 targetLocation;
    [SerializeField]
    private float jumpSpeed;
    [HideInInspector]
    public VelocitySetter vs;
    [SerializeField]
    private Animator anim;
    [HideInInspector]
    public MovementController movement;


    //Stored for cancelling
    Tween tween;


    protected override IEnumerator AttackCoroutine(Transform target)
    {
        Debug.Log("stat");
        movement.LockMovement();
        yield return new WaitForSeconds(.2f);
        targetLocation = target.position;
        anim.SetTrigger("Jump");
        tween = transform.parent.DOMove((Vector3)targetLocation, jumpSpeed).SetSpeedBased().SetEase(Ease.InQuad);
        
        yield return tween.WaitForCompletion();
        anim.ResetTrigger("Jump");
        anim.SetTrigger("Land");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        anim.ResetTrigger("Land");
        gameObject.SetActive(true);
        yield return new WaitForSeconds(.2f);
        gameObject.SetActive(false);
        movement.UnlockMovement();

        InvokeAttackEnd(target);
        Debug.Log("Finished");
    }

    public override void Cancel()
    {
        Debug.Log("Cancelling");
        tween.Kill();
        CoroutineManager.StopGlobalCoroutine(coroutine);
        gameObject.SetActive(false);
        movement.UnlockMovement();
        InvokeAttackEnd(null);
    }

}
