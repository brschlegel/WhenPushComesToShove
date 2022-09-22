using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EarthShove : LootData
{
    [SerializeField] private GameObject earthWallPrefab;
    private GameObject wallRef;

    [SerializeField] private float distanceFromPlayer = 5;

    [SerializeField] private VisualEffect groundPillars;
    private VisualEffect pillarRef;

    public override void Action()
    {
        SpawnVFX();
        StartCoroutine("WaitForVFX", 1f);
        SpawnWall();
    }
    public override void OnEquip(Transform player)
    {
        base.OnEquip(player);

        LevelManager.onNewRoom += DestroyWallOnNewRoom;
        LevelManager.onEndGame += DestroyWallOnNewRoom;
    }

    public override void OnUnequip(Transform player)
    {
        base.OnUnequip(player);

        LevelManager.onNewRoom -= DestroyWallOnNewRoom;
        LevelManager.onEndGame -= DestroyWallOnNewRoom;
    }

    /// <summary>
    /// Spawns the wall prefab
    /// </summary>
    private void SpawnWall()
    {
        if (wallRef != null)
        {
            Destroy(wallRef);
        }
        Vector3 dir = playerRef.GetChild(0).transform.right;
        wallRef = Instantiate(earthWallPrefab, playerRef.transform.position + (dir * distanceFromPlayer), Quaternion.identity);
        wallRef.GetComponentInChildren<Hitbox>().owner = playerRef.gameObject;
    }

    /// <summary>
    /// Spawns the Ground Pillars VFX prefab
    /// </summary>
    private void SpawnVFX()
    {
        if (pillarRef != null)
        {
            Destroy(pillarRef);
        }
        Vector3 dir = playerRef.GetChild(0).transform.right;
        pillarRef = Instantiate(groundPillars, playerRef.transform.position + (dir * (distanceFromPlayer - 2.75f)), Quaternion.identity);
    }

    private void DestroyWallOnNewRoom()
    {
        if (wallRef != null)
        {
            Destroy(wallRef);
        }
    }

    private IEnumerator WaitForVFX(float t)
    {
        yield return new WaitForSeconds(t);
    }
}