using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthShove : LootData
{
    [SerializeField] private GameObject earthWallPrefab;
    private GameObject wallRef;

    [SerializeField] private float distanceFromPlayer = 5;

    public override void Action()
    {
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

    private void DestroyWallOnNewRoom()
    {
        if (wallRef != null)
        {
            Destroy(wallRef);
        }
    }

}