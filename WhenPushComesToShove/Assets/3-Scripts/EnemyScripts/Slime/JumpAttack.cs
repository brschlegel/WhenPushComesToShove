using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpAttack : MonoBehaviour
{

    private Vector2 targetLocation;
    [SerializeField]
    private float jumpSpeed;
    [HideInInspector]
    public VelocitySetter vs;

    //Stored for cancelling
    Tween tween;
    Coroutine coroutine;

    public void Attack(Transform target)
    {

        coroutine = CoroutineManager.StartGlobalCoroutine(AttackCoroutine(target));
    }

    private IEnumerator AttackCoroutine(Transform target)
    {
        vs.Halt();
        yield return new WaitForSeconds(.2f);
        targetLocation = target.position;
        tween = transform.parent.DOMove((Vector3)targetLocation, jumpSpeed).SetSpeedBased().SetEase(Ease.InQuad);
        
        yield return tween.WaitForCompletion();
        gameObject.SetActive(true);
        yield return new WaitForSeconds(.2f);
        gameObject.SetActive(false);
        vs.UnHalt();
    }

    public void Cancel()
    {
        tween.Kill();
        StopCoroutine(coroutine);
        vs.UnHalt();
    }
}
