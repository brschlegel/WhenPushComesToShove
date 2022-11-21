using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    private Collider2D collider;

    private void Start()
    {
        if (ownersToIgnore == null)
        {
            Init();
        }
    }
    public void Init()
    {
        if(HitboxManager.instance == null)
        {
            Debug.LogError("NO HITBOX MANAGER IN SCENE!");
        }
        attackData = GetComponent<AttackData>();
        knockbackData = GetComponent<KnockbackData>();
        ownersToIgnore = new List<GameObject>();
        collider = GetComponent<Collider2D>();
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
    private List<GameObject> ownersToIgnore;

    public Vector2 Center 
    {
        get {return collider.offset + (Vector2) transform.position;}
    }

    public List<GameObject> OwnersToIgnore
    {
        get
        {
            if(ownersToIgnore == null)
            {
                Init();
            }
            return ownersToIgnore;
        }
    }

   
}
