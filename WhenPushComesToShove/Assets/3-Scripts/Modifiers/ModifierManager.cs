using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{

    private List<BaseModifier> modifiers;
    
    public void AddModifier(GameObject prefab)
    {
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
}
