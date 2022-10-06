using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AegisAttack : State
{
    
    [HideInInspector]
    public AegisWall wall;
    [HideInInspector]
    public GameObject hitboxObject;
    [HideInInspector]
    public Chase chase;

    [SerializeField]
    private float bashDistance;
    [SerializeField]
    private float bashDuration;
    [SerializeField]
    private float timeBeforeAttack;
    


    private void OnEnable()
    {
        //Vector3 punch = wallTransform.right * bashDistance;
        Vector3 punch = Vector3.right * bashDistance;
        float wallOffset = wall.wallTransform.position.x;
        
       StartCoroutine(Attack());
    }

    private void EndAttack()
    {
        this.enabled = false;
        InvokeOnStateExit(true);
    }

    public IEnumerator Attack()
    {
        chase.LockMovement();
        yield return new WaitForSeconds(timeBeforeAttack);
        hitboxObject.SetActive(true);
        Tween inTween = wall.wallTransform.DOLocalMoveX(wall.offset + bashDistance, bashDuration/2);
        yield return inTween.WaitForCompletion();
        hitboxObject.SetActive(false);
        Tween outTween = wall.wallTransform.DOLocalMoveX(wall.offset, bashDuration/2);
        yield return outTween.WaitForCompletion();

        EndAttack();
    }

    private void OnDisable()
    {
        chase.UnlockMovement();
    }

  
}
