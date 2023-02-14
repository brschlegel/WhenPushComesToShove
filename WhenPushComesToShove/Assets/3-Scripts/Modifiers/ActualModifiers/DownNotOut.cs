using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownNotOut : BaseModifier
{
    public override void Init()
    {
        foreach(Transform t in GameState.players)
        {
            t.GetComponentInChildren<PlayerHealth>().onDeath += OnDeath;
        }
        base.Init();
    }

    private void OnDeath(int index)
    {
        GameObject player = PlayerConfigManager.Instance.GetPlayerConfigs()[index].PlayerObject;
        KnockbackAlongAxis[] kbHit = player.GetComponentsInChildren<KnockbackAlongAxis>();
        foreach(KnockbackAlongAxis k in kbHit)
        {
            k.gameObject.tag = "Shove";
        }
    }

    public override void CleanUp()
    {
        foreach(Transform t in GameState.players)
        {
            t.GetComponentInChildren<PlayerHealth>().onDeath -= OnDeath;
        }
        base.CleanUp();
    }
}
