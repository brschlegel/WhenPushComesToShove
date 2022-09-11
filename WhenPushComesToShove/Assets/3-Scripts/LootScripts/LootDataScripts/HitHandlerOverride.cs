using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandlerOverride : LootData
{
    [SerializeField] private HitHandler handler;
    public override void OnEquip( Transform player )
    {
        base.OnEquip(player);

        player.GetComponentInChildren<PlayerInventory>().AssignHitHandler(lootType, handler);

    }

    public override void OnUnequip(Transform player)
    {
        base.OnUnequip(player);

        player.GetComponentInChildren<PlayerInventory>().RemoveHitHandler(lootType, handler);
    }
}
