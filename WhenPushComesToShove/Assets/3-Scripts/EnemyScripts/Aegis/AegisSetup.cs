using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisSetup : State
{

   
    private void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PlayAnimations()
    {
        anim.Play("Base Layer.Enemy Ability");
        yield return new WaitForSeconds( anim.GetCurrentClipLength()); 
        InvokeOnStateExit(true);
        this.enabled = false;
    }
}
