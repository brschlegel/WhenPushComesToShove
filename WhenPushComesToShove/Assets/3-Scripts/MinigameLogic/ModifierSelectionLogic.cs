using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSelectionLogic : MinigameLogic
{
    [SerializeField]
    private AreaSelector selector;

    private ModifierManager modifierManager;
    private List<ModifierSettings> modifiers;
    
    private int numberOfModifiers = 3;
    public override void StartGame()
    {
        selector.BeginSelection();
        base.StartGame();
    }

    public override void Init()
    {
        modifierManager = GameObject.FindGameObjectWithTag("ModifierManager").GetComponent<ModifierManager>();
        modifiers = modifierManager.GetRandomModifiers(numberOfModifiers);
        for(int i = 0; i < numberOfModifiers; i++)
        {
            selector.areaDivider.icons[i] = modifiers[i].modifierPrefab.GetComponent<BaseModifier>().icon;
        }
        base.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelectionFinished(int index)
    {
        modifierManager.AddModifier(modifiers[index]);
        modifierManager.RemoveModifierFromPool(modifiers[index]);
    }
}
