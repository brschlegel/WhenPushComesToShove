using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public LootData lightShoveLoot;
    public LootData heavyShoveLoot;
    public List<LootData> passiveLoot;

    [SerializeField] private PlayerLightShoveScript lightShoveScript;
    [SerializeField] private PlayerHeavyShoveScript heavyShoveScript;

    private PlayerInputHandler inputHandler;
    private List<LootData> inRangeLoot;

    private LootManager lootManager;
    private void OnEnable()
    {
        LevelManager.onEndGame += ResetLoot;
    }

    private void OnDisable()
    {
        LevelManager.onEndGame -= ResetLoot;
    }

    [HideInInspector] public GameObject UIRef;

    public void Awake()
    {
        inputHandler = transform.parent.GetComponentInChildren<PlayerInputHandler>();

        passiveLoot = new List<LootData>();
        inRangeLoot = new List<LootData>();

        lootManager = GameObject.Find("LootManager").GetComponent<LootManager>();
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
                    loot.OnEquip(transform.parent);

                    //Assign Sprite to UI
                    UIRef.transform.GetChild(0).GetComponent<Image>().sprite = loot.sprite;

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
                    lootManager.droppedLoot.Remove(loot);
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
                    loot.OnEquip(transform.parent);

                    //Assign Sprite to UI
                    UIRef.transform.GetChild(1).GetComponent<Image>().sprite = loot.sprite;

                    if (loot.overrideBaseAction)
                    {
                        heavyShoveScript.DisableBaseHeavyShove();
                    }

                    heavyShoveScript.onHeavyShove += loot.Action;
                    loot.gameObject.SetActive(false);
                    lootManager.droppedLoot.Remove(loot);
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
                loot.OnEquip(transform.parent);
                lootManager.droppedLoot.Remove(loot);
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
    public void DropLoot(LootData.LootType type, bool dropOnGround = true)
    {
        switch (type)
        {
            
            case LootData.LootType.Light:
                
                lightShoveLoot.OnUnequip(transform.parent);
                lightShoveScript.onLightShove -= lightShoveLoot.Action;
                if (lightShoveLoot.overrideBaseAction)
                {
                    lightShoveScript.EnableBaseLightShove();
                }

                if (dropOnGround)
                {
                    lightShoveLoot.transform.position = transform.parent.position;
                    lightShoveLoot.gameObject.SetActive(true);
                }

                UIRef.transform.GetChild(0).GetComponent<Image>().sprite = null;

                lootManager.droppedLoot.Add(lightShoveLoot);
                lightShoveLoot = null;
                break;
            case LootData.LootType.Heavy:
                heavyShoveLoot.OnUnequip(transform.parent);
                heavyShoveScript.onHeavyShove -= heavyShoveLoot.Action;
                if (heavyShoveLoot.overrideBaseAction)
                {
                    heavyShoveScript.EnableBaseHeavyShove();
                }

                if (dropOnGround)
                {
                    heavyShoveLoot.transform.position = transform.parent.position;
                    heavyShoveLoot.gameObject.SetActive(true);
                }
                lootManager.droppedLoot.Add(heavyShoveLoot);
                UIRef.transform.GetChild(1).GetComponent<Image>().sprite = null;

                heavyShoveLoot = null;
                break;
            case LootData.LootType.Passive: //Not sure if we want players to drop passive loot
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Gets rid of all players loop
    /// </summary>
    void ResetLoot()
    {
        if(lightShoveLoot != null)
            DropLoot(lightShoveLoot.lootType, false);

        if(heavyShoveLoot != null)
        {
            Debug.Log(heavyShoveLoot.name);
            DropLoot(heavyShoveLoot.lootType, false);
        }
            

        foreach(LootData loot in passiveLoot)
        {
            loot.OnUnequip(transform.parent);
        }

        passiveLoot.Clear();
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

    public void AssignHitHandler(LootData.LootType lootType, HitHandler handler)
    {
        switch(lootType)
        {
            case LootData.LootType.Light:
                lightShoveScript.hitbox.gameObject.AddComponent(handler.GetType());
                HitHandler newHandler = lightShoveScript.hitbox.gameObject.GetComponent<HitHandler>();
                newHandler = Extensions.GetCopyOf(newHandler, handler);
                lightShoveScript.hitbox.handler = newHandler;
                newHandler.tagsToIgnore.Add("PlayerHurtbox");
                lightShoveScript.hitbox.gameObject.name = "LightHitbox";
                break;
            case LootData.LootType.Heavy:
                heavyShoveScript.hitbox.gameObject.AddComponent(handler.GetType());
                HitHandler newHeavyHandler = heavyShoveScript.hitbox.gameObject.GetComponent<HitHandler>();
                newHandler = Extensions.GetCopyOf(newHeavyHandler, handler);
                newHandler.tagsToIgnore.Add("PlayerHurtbox");
                heavyShoveScript.hitbox.handler = newHandler;                
                heavyShoveScript.hitbox.gameObject.name = "HeavyHitbox";
                break;
            default:
                Debug.LogError("Cannot override HitHandler with this loot type.");
                break;
        }
    }

    public void RemoveHitHandler(LootData.LootType lootType, HitHandler handler)
    {
        switch (lootType)
        {
            case LootData.LootType.Light:
                Destroy(lightShoveScript.hitbox.gameObject.GetComponent(handler.GetType()));
                lightShoveScript.hitbox.handler = null;
                break;
            case LootData.LootType.Heavy:
                Destroy(heavyShoveScript.hitbox.gameObject.GetComponent(handler.GetType()));
                heavyShoveScript.hitbox.handler = null;
                break;
            default:
                Debug.LogError("Cannot remove HitHandler with this loot type.");
                break;
        }
    }
}
