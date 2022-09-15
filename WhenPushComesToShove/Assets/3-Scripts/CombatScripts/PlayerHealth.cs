using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public PlayerInputHandler playerRef;
    public override void Die()
    {
        playerRef.sr.color = new Color(playerRef.sr.color.r, playerRef.sr.color.g, playerRef.sr.color.b, .5f);

        dead = true;
        playerRef.playerConfig.IsDead = true;

        if (PlayerConfigManager.Instance.CheckAllPlayerDeath())
        {
            GameObject.Find("Path").GetComponent<LevelManager>().ResetPath();
        }
    }

    public void ResetHealth()
    {
        dead = false;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (dead)
        {
            currentHealth = 0;
        }
    }
}
