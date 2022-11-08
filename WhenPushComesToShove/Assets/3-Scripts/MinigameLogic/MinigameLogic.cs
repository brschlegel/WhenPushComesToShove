using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameLogic : MonoBehaviour
{
    private List<BaseEndCondition> endConditions;
    private void OnEnable()
    {
        if(endConditions == null)
        {
           Init();
        }
    } 

    public virtual void Init()
    {
        endConditions = new List<BaseEndCondition>(GetComponentsInChildren<BaseEndCondition>());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
