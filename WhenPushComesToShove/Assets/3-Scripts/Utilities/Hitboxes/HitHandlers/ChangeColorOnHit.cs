using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnHit : HitHandler
{
    [SerializeField] private List<Material> colorsToChangeBasedOnIndex = new List<Material>();
    [SerializeField] private SpriteRenderer spriteToChange;
    [SerializeField] private Slime slime;
    public override void ReceiveHit(HitEvent e)
    {
        PlayerInputHandler handler = e.hitbox.owner.GetComponentInChildren<PlayerInputHandler>();

        //Check for split
        if (slime.slimeTeamIndex != -1 && (slime.slimeTeamIndex != handler.playerConfig.TeamIndex))
        {
            if (slime.slimeSize > 1)
            {
                if (e.hitbox.TryGetComponent<KnockbackData>(out KnockbackData data))
                {
                    //Pass in num of slimes to split based on Force
                    if (data.strength < handler.heavyShoveScript.lowTierChargeForce)
                    {
                        slime.SplitSlime(1);
                    }
                    else if (data.strength < handler.heavyShoveScript.midTierChargeForce)
                    {
                        slime.SplitSlime(2);
                    }
                    else if (data.strength < handler.heavyShoveScript.highTierChargeForce)
                    {
                        slime.SplitSlime(3);
                    }
                    else if (data.strength >= handler.heavyShoveScript.highTierChargeForce)
                    {
                        slime.SplitSlime(4);
                    }
                }
            }
        }
        else if (slime.slimeTeamIndex == -1)
        {
            spriteToChange.material = colorsToChangeBasedOnIndex[handler.playerConfig.TeamIndex];
            slime.slimeTeamIndex = handler.playerConfig.TeamIndex;
            slime.logic.UpdateTeamScore(slime.slimeTeamIndex, slime.pointWorth, true);
        }

    }
}
