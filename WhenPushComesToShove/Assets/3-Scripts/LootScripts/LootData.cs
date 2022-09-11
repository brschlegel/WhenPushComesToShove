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
    [HideInInspector]public Transform playerRef;

    public virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    /// <summary>
    /// Function for the loot to modify something on equip
    /// </summary>
    /// <param name="player"></param>
    public virtual void OnEquip( Transform player )
    {
        playerRef = player;
    }

    /// <summary>
    /// Function for the loot to modify something on unequip
    /// </summary>
    public virtual void OnUnequip( Transform player )
    {

    }

    /// <summary>
    /// Function to override or add to the base functionality of a shove
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
