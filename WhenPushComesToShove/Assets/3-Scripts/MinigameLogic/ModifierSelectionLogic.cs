using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSelectionLogic : MinigameLogic
{
    [SerializeField]
    private AreaSelector selector;

    private ModifierManager modifierManager;
    private List<ModifierSettings> modifiers;
    
    [HideInInspector]
    public ModifierSettings selectedModifier;
    private int numberOfModifiers = 3;

    //Parts
    private List<Transform> areas;
    private List<Transform> barrels;
    private Transform picker;

    public override void StartGame()
    {
        StartCoroutine(Introduction());
        base.StartGame();
    }

    public override void Init()
    {
        ((ModifierSelectionCondition)endCondition).logic = this;
        selectedModifier = null;

        
        selector.Init();
        modifierManager = GameState.ModifierManager;
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

        //Set parts and disable them for introduction
        areas = selector.areaDivider.areas;
        foreach(Transform area in areas)
        {
            area.gameObject.SetActive(false);
        }

        barrels = new List<Transform>(selector.areaDivider.dividers);


        

        base.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelectionFinished(int index)
    {
        selectedModifier = modifiers[index];
        modifierManager.AddModifier(modifiers[index]);
        modifierManager.RemoveModifierFromPool(modifiers[index]);
        BaseModifier modifierScript = selectedModifier.modifierPrefab.GetComponent<BaseModifier>();
        GameState.pathGenerator.PopulateAvailableLevels(modifierScript.minigamesAffected);
        ((ModifierSelectedUIDisplay)endingUIDisplay).modifier = modifierScript;
        selector.gameObject.SetActive(false);
        EndGame();
    }

    public IEnumerator Introduction()
    {
        //Areas
        yield return new WaitForSeconds(.5f);
        //Barrels

        yield return new WaitForSeconds(.5f);
        //Picker

        yield return new WaitForSeconds(.5f);
        //Make Selection
        selector.BeginSelection();
    }
}
