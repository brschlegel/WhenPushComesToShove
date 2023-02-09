using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    public PlayerInputHandler playerInputRef;
    [SerializeField] private PlayerCollisions collider;
    [SerializeField] private Transform playerGroundUIRef;

    [SerializeField] private Transform halo;

    public UnityEvent onDeath;
    private Material playerMat;

    private void OnEnable()
    {
        LevelManager.onNewRoom += ResetHealth;
        LevelManager.onEndGame += ResetHealth;
    }

    private void OnDisable()
    {
        LevelManager.onNewRoom -= ResetHealth;
        LevelManager.onEndGame -= ResetHealth;
    }

    //gets reference to player Material
    public override void TakeDamage(float damage, string source)
    {
        if (source == "PlayerPrefab(Clone)" || dead || GameState.currentRoomType == LevelType.Lobby)
        {
            return;
        }

        base.TakeDamage(damage, source);
        playerMat = this.gameObject.GetComponentInParent<SpriteRenderer>().material;
        //Debug.Log("turning white woo");
        playerMat.SetInt("_IsDamaged", 1);

        playerInputRef.rumble.RumbleLinear(.4f, 0, .8f, 0, .2f, false);

        StartCoroutine(DamageFlash());
    }

    //momentarily turns the variable of playerMat on and then off.
    IEnumerator DamageFlash()
    {        
        yield return new WaitForSeconds(0.2f);
        playerMat.SetInt("_IsDamaged", 0);
        
    }


    public override void Die(string source)
    {
        //Log death
        LoggingInfo.instance.AddPlayerDeath(playerInputRef.playerConfig.PlayerIndex, source);

        playerInputRef.sr.color = new Color(playerInputRef.sr.color.r, playerInputRef.sr.color.g, playerInputRef.sr.color.b, .3f);

        dead = true;
        playerInputRef.playerConfig.IsDead = true;

        //Fade Ground UI Too
        foreach (SpriteRenderer sr in playerGroundUIRef.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);
        }


        //Unassign Ghost Shove Tags
        KnockbackHitHandler kbReciever = transform.parent.GetComponentInChildren<KnockbackHitHandler>();
        HitEventSplitter hitHandler = transform.parent.GetComponentInChildren<HitEventSplitter>();
        if (kbReciever.tagsToIgnore.Contains("GhostShove"))
        {
            kbReciever.tagsToIgnore.Remove("GhostShove");
            kbReciever.tagsToIgnore.Add("Shove");
            hitHandler.tagsToIgnore.Add("Hazard");
        }

        KnockbackAlongAxis[] kbHit = transform.parent.GetComponentsInChildren<KnockbackAlongAxis>();
        foreach (KnockbackAlongAxis kb in kbHit)
        {
            kb.gameObject.tag = "GhostShove";
        }

        //Allow passage through players and enemies
        collider.tagsToIgnoreCollision.Add("Player");
        collider.tagsToIgnoreCollision.Add("Enemy");
        collider.tagsToIgnoreCollision.Add("Hazard");

        //Enable Halo
        halo.gameObject.SetActive(true);

        onDeath?.Invoke();
    }

    public void ResetHealth()
    {
        //Disable Halo
        halo.gameObject.SetActive(false);

        //Prevent passage through players and enemies
        if (dead)
        {
            collider.tagsToIgnoreCollision.Remove("Player");
            collider.tagsToIgnoreCollision.Remove("Enemy");
            collider.tagsToIgnoreCollision.Remove("Hazard");
        }

        dead = false;
        currentHealth = maxHealth;
        playerInputRef.sr.color = new Color(playerInputRef.sr.color.r, playerInputRef.sr.color.g, playerInputRef.sr.color.b, 1f);

        //Reset Ground UI Too
        foreach (SpriteRenderer sr in playerGroundUIRef.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        }

        //Reassign Ghost Shove Tags
        KnockbackHitHandler kbReciever = transform.parent.GetComponentInChildren<KnockbackHitHandler>();
        HitEventSplitter hitHandler = transform.parent.GetComponentInChildren<HitEventSplitter>();
        if (!kbReciever.tagsToIgnore.Contains("GhostShove"))
        {
            kbReciever.tagsToIgnore.Add("GhostShove");
            kbReciever.tagsToIgnore.Remove("Shove");
            hitHandler.tagsToIgnore.Remove("Hazard");
        }

        KnockbackAlongAxis[] kbHit = transform.parent.GetComponentsInChildren<KnockbackAlongAxis>();
        foreach (KnockbackAlongAxis kb in kbHit)
        {
            kb.gameObject.tag = "Shove";
        }
    }

    private void Update()
    {
        if (dead)
        {
            currentHealth = 0;
        }
    }
}
