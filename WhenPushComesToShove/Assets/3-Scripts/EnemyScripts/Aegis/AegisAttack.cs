using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AegisAttack : State
{
    
    [SerializeField]
    private Transform wallTransform;
    [SerializeField]
    private float bashDistance;
    [SerializeField]
    private float bashDuration;

    private void OnEnable()
    {
        //Vector3 punch = wallTransform.right * bashDistance;
        Vector3 punch = Vector3.right * bashDistance;
        
        wallTransform.DOPunchPosition(punch, bashDuration, 1,0).onComplete += EndAttack;
    }

    private void EndAttack()
    {
        this.enabled = false;
        InvokeOnStateExit(true);
    }
}
