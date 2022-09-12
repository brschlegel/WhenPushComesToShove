using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    protected EnemyState currentState;
    public Transform target;

}
