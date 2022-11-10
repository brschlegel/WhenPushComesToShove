using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    HazardDifficulty.HazardStats[] hazardLevels;
    EnemyDifficulty.EnemyLevelStats[] enemyStatLevels;

    List<LevelProperties> levelProps = new List<LevelProperties>();

    [Header("Path properties")]
    [SerializeField] int numOfDungeonRooms;
    int currentPathNum = 0;
    public List<GameObject> path = new List<GameObject>();
    LevelTracks levelTracks;

    // Start is called before the first frame update
    void Start()
    {
        Object[] allMinigames = Resources.LoadAll<Object>("Minigames/");

        foreach(Object obj in allMinigames)
        {
            GameObject level = (GameObject)obj;
            levelProps.Add(level.GetComponent<LevelProperties>());
        }
        //TextAsset textAsset = Resources.Load<TextAsset>("LevelTrack");

        //if(textAsset != null)
        //{
        //    levelTracks = JsonUtility.FromJson<LevelTracks>(textAsset.text);

        //    //Add all of the selected dungeons to the level pool
        //    foreach (HazardPathDetails haz in levelTracks.dungeonPaths)
        //    {
        //        foreach (EnemyPathDetails ene in haz.enemyTracks)
        //        {
        //            if (ene.selected)
        //            {
        //                foreach (GameObject level in ene.levels)
        //                {
        //                    Debug.Log(level.name);
        //                    levelProps.Add(level.GetComponent<LevelProperties>());
        //                }
        //            }
        //        }
        //    }

        //    //Add all of the selected arenas to the level pool
        //    foreach (ArenaDetails arena in levelTracks.arenaPaths)
        //    {
        //        if (arena.selected)
        //        {
        //            foreach (GameObject level in arena.levels)
        //            {
        //                levelProps.Add(level.GetComponent<LevelProperties>());
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    Object[] allDungeons = Resources.LoadAll<Object>("Dungeons/");
        //    Object[] allArenas = Resources.LoadAll<Object>("Arenas/");

        //    foreach(Object obj in allDungeons)
        //    {
        //        GameObject level = (GameObject)obj;
        //        levelProps.Add(level.GetComponent<LevelProperties>());
        //    }

        //    foreach(Object obj in allArenas)
        //    {
        //        GameObject arena = (GameObject)obj;
        //        levelProps.Add(arena.GetComponent<LevelProperties>());
        //    }
        //}


        if (path.Count <= 0)
            GeneratePath();

        InstantiatePathRooms();

        LevelManager.onNewRoom.Invoke();
    }

    void GeneratePath()
    {
        LevelProperties[] shuffledRooms = ShuffleRooms();

        while(currentPathNum < numOfDungeonRooms)
        {
            if(currentPathNum == 0)
            {
                path.Add(Resources.Load<GameObject>("Lobby"));
                //for (int i = 0; i < hazardLevels.Length; i++)
                //{
                //    hazardLevels[i].level++;
                //}
                //for (int i = 0; i < enemyStatLevels.Length; i++)
                //{
                //    enemyStatLevels[i].level++;
                //}
            }
            else
            {
                //Go through the rooms and see if the hazard levels match 
                for (int i = 0; i < shuffledRooms.Length; i++)
                {
                    path.Add(shuffledRooms[i].gameObject);
                    Debug.Log(path.Count);
                    //if (shuffledRooms[i] != null)
                    //{
                        
                    //    if (IsCompatibleRoom(shuffledRooms[i]) && shuffledRooms[i].levelType == LevelType.Dungeon)
                    //    {
                    //        path.Add(shuffledRooms[i].gameObject);

                    //        //Ups the levels this rooms hazards in the path
                    //        foreach (HazardDifficulty.HazardStats stat in shuffledRooms[i].hazards)
                    //        {
                    //            for (int j = 0; j < hazardLevels.Length; j++)
                    //            {
                    //                if (stat.hazard == hazardLevels[j].hazard)
                    //                    hazardLevels[j].level++;
                    //            }
                    //        }

                    //        //Ups the enemy levels in the path
                    //        foreach (EnemyDifficulty.EnemyLevelStats stat in shuffledRooms[i].enemyStats)
                    //        {
                    //            for (int j = 0; j < enemyStatLevels.Length; j++)
                    //            {
                    //                if (stat.enemy == enemyStatLevels[j].enemy)
                    //                    enemyStatLevels[j].level++;
                    //            }
                    //        }

                    //        //Ensures the same room ins't spawned twice
                    //        shuffledRooms[i] = null;
                    //        break;
                    //    }
                    //}

                    //Will stop the path generation if there aren't any rooms to add to the path
                    if (i == shuffledRooms.Length - 1)
                    {
                        Debug.Log("No remaining rooms for this path");
                        currentPathNum = numOfDungeonRooms;
                    }
                }
            }
            

            currentPathNum++;
        }

        //Spawn Arena Room
        //LevelProperties arena = null;
        //for (int i = 0; i < shuffledRooms.Length; i++)
        //{
        //    if (shuffledRooms[i] != null)
        //    {
        //        if(shuffledRooms[i].levelType == LevelType.Arena)
        //        {
        //            if (IsCompatibleRoom(shuffledRooms[i], true))
        //            {
        //                arena = shuffledRooms[i];
        //                path.Add(arena.gameObject);
        //            }                       
        //            break;
        //        }
        //    }
        //}

        //if (arena == null)
        //{
        //    Debug.LogWarning("No arena available for this path. Loading in default arena");
        //    path.Add(Resources.Load<GameObject>("DefaultArena"));
        //}
            
    }

    public void ResetPath()
    {
        //Clean out the current path
        currentPathNum = 0;
        path.Clear();
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        //Reset levels for hazards and enemies
        //for(int i = 0; i < hazardLevels.Length; i++)
        //{
        //    hazardLevels[i].level = 0;
        //}

        //for(int i = 0; i < enemyStatLevels.Length; i++)
        //{
        //    enemyStatLevels[i].level = 0;
        //}

        //Generate and spawn new path
        GeneratePath();
        InstantiatePathRooms();
    }

    void InstantiatePathRooms()
    {
        for (int i = 0; i < path.Count; i++)
        {
            GameObject room = Instantiate(path[i]);
            room.transform.parent = transform;
            room.SetActive(false);
            path[i] = room;
        }
    }

    //Helper Functions

    /// <summary>
    /// Puts the rooms in a random order
    /// </summary>
    /// <returns></returns>
    LevelProperties[] ShuffleRooms()
    {
        LevelProperties[] shuffledProps = levelProps.ToArray();
        //https://answers.unity.com/questions/1189736/im-trying-to-shuffle-an-arrays-order.html by Loise-N-D

        for(int i = 0; i < shuffledProps.Length; i++)
        {
            int rnd = Random.Range(i, shuffledProps.Length);
            LevelProperties prop = shuffledProps[rnd];
            shuffledProps[rnd] = shuffledProps[i];
            shuffledProps[i] = prop;
        }

        return shuffledProps;
    }

    //Tests to see if a room's hazards are the correct difficulty
    bool IsCompatibleRoom(LevelProperties prop, bool onlyHazards = false)
    {
        //Check if the level's hazards match the path's
        foreach (HazardDifficulty.HazardStats haz in prop.hazards)
        {
            foreach(HazardDifficulty.HazardStats hazLevel in hazardLevels)
            {
                //Makes sure that the same hazards are being compared
                if(haz.hazard == hazLevel.hazard)
                {
                    //Make sure that hazards are the same level
                    if (haz.level != hazLevel.level)
                        return false;
                }
            }
        }

        if (!onlyHazards)
        {
            //Check if the level's enemies match the path's
            foreach (EnemyDifficulty.EnemyLevelStats enm in prop.enemyStats)
            {
                foreach (EnemyDifficulty.EnemyLevelStats enmLevel in enemyStatLevels)
                {
                    //Makes sure that the same enemies are being compared
                    if (enm.enemy == enmLevel.enemy)
                    {
                        //Make sure that enemies are the same level
                        if (enm.level != enmLevel.level)
                            return false;
                    }
                }
            }
        }
        
        return true;
    }
}
