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
    public List<ModifierSettings> modifiers;

    private int numberOfModifiers = 3;

    //Parts
    private List<Transform> areas;
    private List<Transform> barrels;
    private Transform picker;

    public override void StartGame()
    {
        StartCoroutine(selector.Introduction());
        base.StartGame();
    }

    public override void Init()
    {
        ((ModifierSelectionCondition)endCondition).logic = this;
        selectedModifier = null;
        modifierManager = GameState.ModifierManager;

        //Grab potential modifiers
        modifiers = modifierManager.GetRandomModifiers(numberOfModifiers);

        //Getting icons and animations for the areas
        List<Sprite> modifierSprites = new List<Sprite>();
        List<RuntimeAnimatorController> modifierAnimations = new List<RuntimeAnimatorController>();
        for (int i = 0; i < numberOfModifiers; i++)
        {
            BaseModifier mod = modifiers[i].modifierPrefab.GetComponent<BaseModifier>();
            modifierSprites.Add(modifiers[i].modifierPrefab.GetComponent<BaseModifier>().icon);
            modifierAnimations.Add(modifiers[i].modifierPrefab.GetComponent<BaseModifier>().iconAnimator);
        }

        selector.Init(modifierSprites, modifierAnimations);
        selector.onSelection += OnSelectionFinished;
        base.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelectionFinished(int index)
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
