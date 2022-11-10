using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseEndCondition))]
public class TransitionOnEndCondition : MonoBehaviour
{
    [SerializeField]
    private float delay = 1;
    private BaseEndCondition endCondition; 

    private bool transitioning;


    private void OnEnable()
    {
        endCondition = GetComponent<BaseEndCondition>();
    }
    // Update is called once per frame
    void Update()
    {
        if(endCondition.TestCondition() && !transitioning)
        {
            CoroutineManager.StartGlobalCoroutine(TransitionToLevel());
            //gameObject.SetActive(false);
            transitioning = true;
        }
    }

    private IEnumerator TransitionToLevel()
    {
        yield return new WaitForSeconds(delay);
        LevelManager.onNewRoom.Invoke();
        transitioning = false;
    }
}
