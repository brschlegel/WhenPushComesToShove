using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSelectionCondition : BaseEndCondition
{
    [HideInInspector]
    public ModifierSelectionLogic logic;
    public override void Init()
    {
        
    }

    public override bool TestCondition()
    {
        return logic.selectedModifier != null;
    }
}
