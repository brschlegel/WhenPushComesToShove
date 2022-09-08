using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class LootData : MonoBehaviour
{
    public enum LootType 
    { 
        Light,
        Heavy,
        Passive
    }

    [HideInInspector]public Sprite sprite;
    public LootType lootType;
    public bool overrideBaseAction = true;
    [HideInInspector]public Transform player;

    public virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    /// <summary>
    /// The function for the powerup to override. If the item is passive it will be called on equip otherwise it will be attached to the shove action
    /// </summary>
    public virtual void Action() 
    {
        Debug.Log("shove");
    }

    /// <summary>
    /// Pickup Object On Collision
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInChildren<PlayerInventory>().EquipItem(this);
        }
    }

    /// <summary>
    /// Remove from inRangeLoot if not colliding
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInChildren<PlayerInventory>().RemoveLootFromRange(this);
        }
    }
}
