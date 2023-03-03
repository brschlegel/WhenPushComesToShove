using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayAnimOnce : MonoBehaviour
{ 
    public UnityEvent onAnimFinish;
    [SerializeField]
    [Tooltip("ANIMATION MUST HAVE A LENGTH OF 1 TO BE TIME SCALED CORRECTLY")]
    private string animName;
    [SerializeField]
    private string idleStateName;
    [SerializeField]
    private Animator anim;

    
    public void PlayAnim()
    {
        StartCoroutine(PlayAnimation());
    }
    private IEnumerator PlayAnimation()
    {
        anim.Play(animName, 0);

        Debug.Log(anim.GetCurrentClipLength());
        yield return new WaitForSeconds(anim.GetCurrentClipLength());
        anim.Play(idleStateName, 0);
        onAnimFinish?.Invoke();

    }

    public float Speed 
    {
        get 
        {
            return anim.speed;
        }
        set
        {
            anim.speed = value;
        }
    }
}
