using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just plays an animation on enable
/// </summary>
public class PlayAnimState : State
{
    [HideInInspector]
    public string animName;
    [HideInInspector] public bool automaticallyExit = true;
    private void OnEnable()
    {
        StartCoroutine(PlayAnimation(animName));
    }

    public IEnumerator PlayAnimation(string animName)
    {
        anim.Play(animName, 0);
        yield return new WaitForSeconds(anim.GetCurrentClipLength());

        if (automaticallyExit)
        {
            InvokeOnStateExit(true);
            this.enabled = false;
        }
    }
}
