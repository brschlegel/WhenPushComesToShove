using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnHit : HitHandler
{
    [SerializeField] private List<Color> colorsToChangeBasedOnIndex = new List<Color>();
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
                slime.SplitSlime();
            }
        }

        spriteToChange.color = colorsToChangeBasedOnIndex[handler.playerConfig.TeamIndex];
        slime.slimeTeamIndex = handler.playerConfig.TeamIndex;
    }
}
