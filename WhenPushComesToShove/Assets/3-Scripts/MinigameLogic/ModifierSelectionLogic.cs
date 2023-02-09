using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSelectionLogic : MinigameLogic
{
    [HideInInspector]
    public ModifierSettings selectedModifier;

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
        ((ModifierSelectionCondition)endCondition).logic = this;
        selectedModifier = null;

        
        selector.Init();
        modifierManager = GameState.ModifierManager;

        //Grab potential modifiers
        modifiers = modifierManager.GetRandomModifiers(numberOfModifiers);
        selector.onSelection += OnSelectionFinished;

        //Setting up divider
        selector.areaDivider.iconAnimations = new List<RuntimeAnimatorController>();
        selector.areaDivider.iconSprites = new List<Sprite>();
        for (int i = 0; i < numberOfModifiers; i++)
        {
            selector.areaDivider.iconSprites.Add(modifiers[i].modifierPrefab.GetComponent<BaseModifier>().icon);
            selector.areaDivider.iconAnimations.Add(modifiers[i].modifierPrefab.GetComponent<BaseModifier>().iconAnimator);
        }
        selector.areaDivider.Init();

        base.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelectionFinished(int index)
    {
        selectedModifier = modifiers[index];
        //Update ModifierManager
        modifierManager.AddModifier(modifiers[index]);
        modifierManager.RemoveModifierFromPool(modifiers[index]);
        //Update Path
        BaseModifier modifierScript = selectedModifier.modifierPrefab.GetComponent<BaseModifier>();
        GameState.pathGenerator.PopulateAvailableLevels(modifierScript.minigamesAffected);
        //Update UI
        ((ModifierSelectedUIDisplay)endingUIDisplay).modifier = modifierScript;
        selector.gameObject.SetActive(false);
        EndGame();
    }
}
