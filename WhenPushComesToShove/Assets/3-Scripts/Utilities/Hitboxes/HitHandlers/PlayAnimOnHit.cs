using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimOnHit : HitHandler
{
    public string animName;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private string idleStateName;
    public override void ReceiveHit(HitEvent e)
    {
        StartCoroutine(PlayAnimation());
        
    }
    public IEnumerator PlayAnimation()
    {

        anim.Play(animName, 0);
        yield return new WaitForSeconds(anim.GetCurrentClipLength());
        anim.Play(idleStateName, 0);

    }
}
