using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AegisBlock : State
{
    [HideInInspector]
    public AegisWall wall;
    private Tween t;
    private void OnEnable()
    {
        t = wall.wallTransform.DOShakePosition(.05f, new Vector3(0, .1f, 0), 1, 30);
        t.onComplete += EndBlock;
    }

    private void OnDisable()
    {
        wall.ResetWallPosition();
        t.Kill();
    }

    private void EndBlock()
    {
        if (enabled)
        {
            this.enabled = false;
            InvokeOnStateExit(true);
        }
    }
}
