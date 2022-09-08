using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public LootData lightShoveLoot;
    public LootData heavyShoveLoot;
    public List<LootData> passiveLoot;

    [SerializeField] private PlayerLightShoveScript lightShoveScript;
    [SerializeField] private PlayerHeavyShoveScript heavyShoveScript;

    private PlayerInputHandler inputHandler;
    private List<LootData> inRangeLoot;

    public void Awake()
    {
        inputHandler = transform.parent.GetComponentInChildren<PlayerInputHandler>();

        passiveLoot = new List<LootData>();
        inRangeLoot = new List<LootData>();
    }

    public void EquipItem(LootData loot) //TODO Prompt player for loot switch in some way
    {
        switch(loot.lootType)
        {
            case LootData.LootType.Light:
                //Immediately Equip if Slot is Empty
                if (lightShoveLoot == null)
                {
                    lightShoveLoot = loot;
                    loot.player = transform.parent;

                    if (loot.overrideBaseAction)
                    {
                        lightShoveScript.DisableBaseLightShove();
                    }
                    else if (lightShoveScript.onLightShove == null)
                    {
                        lightShoveScript.EnableBaseLightShove();
                    }

                    lightShoveScript.onLightShove += loot.Action;
                    loot.gameObject.SetActive(false);
                }
                //Otherwise assign equiping the loot to the select action
                else
                {
                    inputHandler.ClearSelectAction();
                    inRangeLoot.Add(loot);
                    inputHandler.onSelect += TradeLoot;
                }
                break;
            case LootData.LootType.Heavy:
                //Immediately Equip if Slot is Empty
                if (heavyShoveLoot == null)
                {
                    heavyShoveLoot = loot;
                    loot.player = transform.parent;

                    if (loot.overrideBaseAction)
                    {
                        heavyShoveScript.DisableBaseHeavyShove();
                    }

                    heavyShoveScript.onHeavyShove += loot.Action;
                    loot.gameObject.SetActive(false);
                }
                //Otherwise assign equiping the loot to the select action
                else
                {
                    inputHandler.ClearSelectAction();
                    inRangeLoot.Add(loot);
                    inputHandler.onSelect += TradeLoot;
                }
                break;
            case LootData.LootType.Passive:
                //Add the loot
                passiveLoot.Add(loot);
                loot.player = transform.parent;

                //Trigger the Pasive Action
                loot.Action();

                loot.gameObject.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Function that equips new loot and drops existing loot
    /// </summary>
    public void TradeLoot()
    {
        //Try to pickup the 1st object in the list
        if (inRangeLoot.Count > 0)
        {
            LootData lootToEquip = inRangeLoot[0];
            DropLoot(lootToEquip.lootType);
            EquipItem(lootToEquip);            
        }

    }

    /// <summary>
    /// Drops the current loot in the existing category
    /// </summary>
    /// <param name="type"></param>
    public void DropLoot(LootData.LootType type)
    {
        switch (type)
        {
            case LootData.LootType.Light:
                lightShoveScript.onLightShove = null;
                lightShoveScript.EnableBaseLightShove();

                lightShoveLoot.transform.position = transform.parent.position;
                lightShoveLoot.gameObject.SetActive(true);

                lightShoveLoot = null;
                break;
            case LootData.LootType.Heavy:
                heavyShoveScript.onHeavyShove = null;
                heavyShoveScript.EnableBaseHeavyShove();

                heavyShoveLoot.transform.position = transform.parent.position;
                heavyShoveLoot.gameObject.SetActive(true);

                heavyShoveLoot = null;
                break;
            case LootData.LootType.Passive:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Removes the loot from being able to be picked up
    /// </summary>
    /// <param name="loot"></param>
    public void RemoveLootFromRange(LootData loot)
    {
        if (inRangeLoot.Contains(loot))
        {
            inRangeLoot.Remove(loot);
        }

        //If no items in range, be sure to clear the select action
        if (inRangeLoot.Count == 0)
        {
            inputHandler.ClearSelectAction();
        }
    }
}
