using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeLand : State
{

    private IEnumerator enumerator;

    [HideInInspector]
    public Chase chase;
    [HideInInspector]
    public GameObject hitboxObject;

    [SerializeField]
    private float hitboxActiveTime;

    private void OnEnable()
    {
        enumerator = CoroutineManager.StartGlobalCoroutine(LandCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        chase.UnlockMovement();
        CoroutineManager.StopGlobalCoroutine(enumerator);
        hitboxObject.SetActive(false);
    }

    private IEnumerator LandCoroutine()
    {
        chase.LockMovement();
        anim.Play("Base.Slime_Land", 0);
        yield return new WaitForSeconds(anim.GetCurrentClipLength());
        hitboxObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitboxObject.SetActive(false);

        this.enabled = false;
        InvokeOnStateExit(true);
    }
}
