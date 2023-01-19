using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    public GameObject lobby;
    public GameObject modifierRoom;

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

        LevelProperties newLevel = null;

        //Grab a random level from the avaiable levels if there ins't a set path
        if(path.Count <= 0 || currentPathNum >= path.Count)
        {
            int rng = Random.Range(0, availableLevels.Count);
            newLevel = availableLevels[rng];
        }
        else
        {
            newLevel = path[currentPathNum].GetComponent<LevelProperties>();
            currentPathNum++;
        }
        


        playedLevels.Add(newLevel);
        return newLevel;
    }

    public GameObject SpawnRoom(GameObject level)
    {
        GameObject newLevel = Instantiate(level);
        newLevel.transform.parent = transform;
        return newLevel;
    }


    public void ResetPath()
    {
        //Clean out the current path
        currentPathNum = 0;
        
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
