using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{

    private List<BaseModifier> modifiers;

    [Tooltip("Base List of modifiers to choose from")]
    public ModifierList modifierPoolSO;
    [Tooltip("Any modifiers put in here will be added on start")]
    public ModifierList debugList;

    private List<ModifierSettings> modifierPool;

    void Start()
    {
        modifiers = new List<BaseModifier>();
        if (debugList != null)
        {
            foreach (ModifierSettings m in debugList.modifiers)
            {
                AddModifier(m);
            }
        }

        //Copy into modifier pool, so we aren't editting actual scriptable object
        modifierPool = new List<ModifierSettings>(modifierPoolSO.modifiers);
    }
    
    public void AddModifier(ModifierSettings settings)
    {
        GameObject prefab = settings.modifierPrefab;

        Instantiate(prefab, this.transform);
        BaseModifier mod = prefab.GetComponent<BaseModifier>();
        if(mod == null)
            Debug.LogError("Can't find modifier to add!");

        modifiers.Add(mod);
        mod.Init();
    }
 
    public void RemoveAllModifiers()
    {
        foreach(BaseModifier mod in modifiers)
        {
            mod.CleanUp();
            Destroy(mod.gameObject);

        }
    }

    public void InitMinigame(Transform minigameRoot)
    {
        foreach(BaseModifier mod in modifiers)
        {
           mod.currentGameRoot = minigameRoot;
           mod.GameInit();
        }
    }

    public void StartMinigame()
    {
        foreach(BaseModifier mod in modifiers)
        {
            mod.GameStart();
        }
    }

    public List<ModifierSettings> GetRandomModifiers(int num)
    {
         foreach (LevelProperties p in GameState.pathGenerator.playedLevels)
                {
                    Debug.Log("played: " + p.game);
                }
        List<ModifierSettings> copy = new List<ModifierSettings>(modifierPool);
        List<ModifierSettings> r = new List<ModifierSettings>();
        copy.Shuffle();

        while(r.Count < num)
        {
            if(copy.Count == 0)
            {
                Debug.LogError("No more valid modifiers");
            }
            ModifierSettings chosen = CheckModifier(copy);
            copy.RemoveAt(0);
            if(chosen != null)
            { 
                r.Add(chosen);
            }
        }

        return r;

    }

    //Checks modifier, returns if should be added. In its own method so i can return out of it easy
    public ModifierSettings CheckModifier(List<ModifierSettings> copy)
    {
        ModifierSettings candidate = copy[0];
        List<Minigame> affected = candidate.modifierPrefab.GetComponent<BaseModifier>().minigamesAffected;
        if (affected.Contains(Minigame.All))
        {
            return candidate;
        }
        else
        {

            foreach (Minigame m in affected)
            {
                bool contains = false;
                foreach (LevelProperties p in GameState.pathGenerator.playedLevels)
                {
                    if(p.game == m)
                    {
                       contains = true;
                    }
                }
                //If we go through the whole played level list and it does not contain one of the minigames affected, then we can select that modifier
                if(!contains)
                {
                    return candidate;
                }
            }
                return null;
        }
    }

    public void RemoveModifierFromPool(ModifierSettings settings)
    {
        modifierPool.Remove(settings);
    }
}
