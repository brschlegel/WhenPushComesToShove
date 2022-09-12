using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusAilments : MonoBehaviour
{
    public Dictionary<string, Coroutine> statusAilments;

    public void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        statusAilments = new Dictionary<string, Coroutine>();
    }
}
