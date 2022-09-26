using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    public void Start()
    {
        if(HitboxManager.instance == null)
        {
            Debug.LogError("NO HITBOX MANAGER IN SCENE!");
        }
        attackData = GetComponent<AttackData>();
        knockbackData = GetComponent<KnockbackData>();
        ownersToIgnore = new List<GameObject>();
    }

    [Tooltip("Hit Handler for hitbox, OPTIONAL")]
    public HitHandler handler;
    [Tooltip("Hitbox priority is superceded by hurtbox priority")]
    public int priority;
    [Tooltip("Hitboxes will not collide with hurtboxes who have the same owner")]
    public GameObject owner;

    [HideInInspector]
    public AttackData attackData;
    [HideInInspector]
    public KnockbackData knockbackData;
    [HideInInspector]
    public List<GameObject> ownersToIgnore;

   
}
