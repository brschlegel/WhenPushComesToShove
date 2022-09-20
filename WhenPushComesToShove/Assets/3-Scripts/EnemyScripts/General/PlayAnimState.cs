using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just plays an animation on enable
/// </summary>
public class PlayAnimState : State
{
    [SerializeField]
    private string animName;
    private void OnEnable()
    {
        StartCoroutine(PlayAnimation(animName));
    }

    public IEnumerator PlayAnimation(string animName)
    {
        anim.Play(animName, 0);
        yield return new WaitForSeconds( anim.GetCurrentClipLength());
        InvokeOnStateExit(true);
    }
}
