using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] HazardDifficulty.HazardStats[] hazardLevels;
    Object[] levels;
    List<LevelProperties> levelProps = new List<LevelProperties>();

    [Header("Path properties")]
    [SerializeField] int numOfRoom;
    int currentPathNum = 0;
    public List<GameObject> path = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        levels = Resources.LoadAll("Levels");
        for(int i = 0; i < levels.Length; i++)
        {
            GameObject obj = (GameObject)levels[i];
            levelProps.Add(obj.GetComponent<LevelProperties>());
        }

        GeneratePath();

        for(int i = 0; i < path.Count; i++)
        {
            GameObject room = Instantiate(path[i]);
            room.transform.parent = transform;
            room.SetActive(false);
        }

        LevelManager.onNewRoom.Invoke();
    }

    void GeneratePath()
    {
        LevelProperties[] shuffledRooms = ShuffleRooms();

        while(currentPathNum < numOfRoom)
        {
            //Go through the rooms and see if the hazard levels match 
            for (int i = 0; i < shuffledRooms.Length; i++)
            {
                if(shuffledRooms[i] != null)
                {
                    if(IsCompatibleRoom(shuffledRooms[i]))
                    {
                        path.Add(shuffledRooms[i].gameObject);

                        //Ups the levels this rooms hazards in the path
                        foreach(HazardDifficulty.HazardStats stat in shuffledRooms[i].hazards)
                        {
                            for(int j = 0; j < hazardLevels.Length; j++)
                            {
                                if (stat.hazard == hazardLevels[j].hazard)
                                    hazardLevels[j].level++;
                            }
                        }

                        //Ensures the same room ins't spawned twice
                        shuffledRooms[i] = null;
                        break;
                    }
                }

                //Will stop the path generation if there aren't any rooms to add to the path
                if(i == shuffledRooms.Length - 1)
                {
                    Debug.Log("No remaining rooms for this path");
                    currentPathNum = numOfRoom;
                }
            }

            currentPathNum++;
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
    bool IsCompatibleRoom(LevelProperties prop)
    {
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

        return true;
    }
}
