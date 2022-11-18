using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    public GameObject lobby;

    //All possible minigames
    List<LevelProperties> allLevels = new List<LevelProperties>();

    //All available levels based on modifers
    public List<LevelProperties> availableLevels = new List<LevelProperties>();

    //Any games that have already been played that can be filter out of the available levels
    List<LevelProperties> playedLevels = new List<LevelProperties>();

    [Header("Path properties")]
    public int numOfDungeonRooms;
    int currentPathNum = 0;
    public List<GameObject> path = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Object[] allMinigames = Resources.LoadAll<Object>("Minigames/");

        foreach(Object obj in allMinigames)
        {
            GameObject level = (GameObject)obj;
            LevelProperties prop = level.GetComponent<LevelProperties>();
            allLevels.Add(prop);
            availableLevels.Add(prop);
        }

        SpawnRoom(lobby);
        LevelManager.onNewRoom.Invoke();
    }
    
    public LevelProperties AssignLevel()
    {
        //Remove any levels already played
        foreach(LevelProperties level in playedLevels)
        {
            availableLevels.Remove(level);
        }

        //Grab a random level from the avaiable levels
        int rng = Random.Range(0, availableLevels.Count);
        LevelProperties newLevel = availableLevels[rng];


        playedLevels.Add(newLevel);
        return newLevel;
    }

    public GameObject SpawnRoom(GameObject level)
    {
        GameObject newLevel = Instantiate(level);
        newLevel.transform.parent = transform;
        return newLevel;
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
            
    }

    public void ResetPath()
    {
        //Clean out the current path
        currentPathNum = 0;
        //path.Clear();
        Destroy(transform.GetChild(0).gameObject);

        //Generate and spawn new path
        //GeneratePath();
        //InstantiatePathRooms();
        availableLevels.Clear();
        playedLevels.Clear();

        foreach(LevelProperties level in allLevels)
        {
            availableLevels.Add(level);
        }

        LevelManager.onNewRoom.Invoke();
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
        LevelProperties[] shuffledProps = allLevels.ToArray();
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
    //bool IsCompatibleRoom(LevelProperties prop, bool onlyHazards = false)
    //{
    //    //Check if the level's hazards match the path's
    //    foreach (HazardDifficulty.HazardStats haz in prop.hazards)
    //    {
    //        foreach(HazardDifficulty.HazardStats hazLevel in hazardLevels)
    //        {
    //            //Makes sure that the same hazards are being compared
    //            if(haz.hazard == hazLevel.hazard)
    //            {
    //                //Make sure that hazards are the same level
    //                if (haz.level != hazLevel.level)
    //                    return false;
    //            }
    //        }
    //    }

    //    if (!onlyHazards)
    //    {
    //        //Check if the level's enemies match the path's
    //        foreach (EnemyDifficulty.EnemyLevelStats enm in prop.enemyStats)
    //        {
    //            foreach (EnemyDifficulty.EnemyLevelStats enmLevel in enemyStatLevels)
    //            {
    //                //Makes sure that the same enemies are being compared
    //                if (enm.enemy == enmLevel.enemy)
    //                {
    //                    //Make sure that enemies are the same level
    //                    if (enm.level != enmLevel.level)
    //                        return false;
    //                }
    //            }
    //        }
    //    }
        
    //    return true;
    //}
}
