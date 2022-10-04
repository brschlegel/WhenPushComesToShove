using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hurtbox : MonoBehaviour
{
    private new BoxCollider2D collider;
    public void Start()
    {
        if(HitboxManager.instance == null)
        {
            Debug.LogError("NO HITBOX MANAGER IN SCENE!");
        }
        ownersToIgnore = new List<GameObject>();
        collider = GetComponent<BoxCollider2D>();
    }

    [Tooltip("The component which handles the hit event")]
    public HitHandler handler;
    [Tooltip("The priority in which this hurtbox is chosen to notify the handler")]
    public int priority;
    [Tooltip("Hitboxes will not collide with hurtboxes who have the same owner")]
    public GameObject owner;

    [HideInInspector]
    public List<GameObject> ownersToIgnore;


    public void OnTriggerEnter2D(Collider2D collider)
    {
        Hitbox hitbox = collider.GetComponent<Hitbox>();
        if(hitbox != null)
        {
            HitboxManager.instance.RegisterHit(hitbox, this);

        }
    }

    public Vector2 Center 
    {
        get {return collider.offset + (Vector2) transform.position;}
    }
    
}
