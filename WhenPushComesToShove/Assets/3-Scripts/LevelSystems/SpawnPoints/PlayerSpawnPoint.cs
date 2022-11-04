using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerSpawnProps
{
    public Transform transform;
    public int teamIndex;
    public bool filled;

}
public class PlayerSpawnPoint : MonoBehaviour
{
    public int teamIndex;
}

