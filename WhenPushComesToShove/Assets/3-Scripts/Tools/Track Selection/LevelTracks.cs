using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class HazardPathDetails
{
    public string hazard;
    public List<EnemyPathDetails> enemyTracks;
}
public class EnemyPathDetails
{
    public bool selected;
    public string enemies;
    public List<GameObject> levels;
}
public class ArenaDetails
{
    public bool selected;
    public string hazard;
    public List<GameObject> levels;
}

public class LevelTracks : MonoBehaviour
{
    Object[] dungeons;
    Object[] arenas;

    public List<HazardPathDetails> dungeonPaths;
    public List<ArenaDetails> arenaPaths;

    public LevelTracks()
    {
        
        Init();
    }

    public void Init()
    {
        if(dungeonPaths == null)
            dungeonPaths = new List<HazardPathDetails>();

        if(arenaPaths == null)
            arenaPaths = new List<ArenaDetails>();

        dungeonPaths.Clear();
        arenaPaths.Clear();

        dungeons = Resources.LoadAll("Dungeons");
        PopulateDungeonDetails();

        arenas = Resources.LoadAll("Arenas");
        PopulateArenaDetails();
    }
    void PopulateDungeonDetails()
    {
        foreach(Object dun in dungeons)
        {
            //Split the hazards and enemies IDs and take out the numbers
            //https://stackoverflow.com/questions/4792242/regex-to-get-number-only-from-string
            string[] levelID = Regex.Replace(dun.name, @"\d+", "").Split('_');

            //Put the dungeon in the correct hazard track
            HazardPathDetails currentHazPath = CompareHazards(levelID[0]);

            //Put the dungeon in the correct enemy track
            CompareEnemies(currentHazPath.enemyTracks, levelID[1], dun);         
        }
    }

    void PopulateArenaDetails()
    {
        foreach (Object arena in arenas)
        {
            string[] levelID = arena.name.Split('_');
            CompareArenas(levelID[0], arena);
        }
    }

    //Helper Function

    /// <summary>
    /// Checks to see if a track for the HazardID already exists
    /// </summary>
    /// <param name="hazardID">Type of hazard</param>
    /// <returns></returns>
    HazardPathDetails CompareHazards(string hazardID)
    {
        //If there's already a path with that hazard, return that path
        foreach(HazardPathDetails haz in dungeonPaths)
        {
            if (haz.hazard == hazardID)
                return haz;
        }

        //Otherwise, create a new path
        HazardPathDetails newPath = new HazardPathDetails();
        newPath.hazard = hazardID;
        newPath.enemyTracks = new List<EnemyPathDetails>();
        Debug.Log("Make new Path");
        return newPath;
    }

    /// <summary>
    /// Checks to see if there's already a path for the type of enemy
    /// </summary>
    /// <param name="enemyPaths">The enemy paths from the selected hazard path</param>
    /// <param name="enemyID">Type of enemy</param>
    /// <param name="dungeon">The level prefab</param>
    void CompareEnemies(List<EnemyPathDetails> enemyPaths, string enemyID, Object dungeon)
    {
        //Add the designated dungeon to the enemy track
        foreach (EnemyPathDetails enm in enemyPaths)
        {
            Debug.Log(enm.enemies);
            if (enm.enemies == enemyID)
            {
                //Ensures the same level isn't being added twice
                foreach(GameObject level in enm.levels)
                {
                    if (level == dungeon as GameObject)
                        return;
                }
                enm.levels.Add(dungeon as GameObject);
                return;
            }
                
        }

        //Make a new one if one doesn't exist
        EnemyPathDetails newPath = new EnemyPathDetails();
        newPath.selected = true; 
        newPath.enemies = enemyID;
        newPath.levels = new List<GameObject>();
        newPath.levels.Add(dungeon as GameObject);
        Debug.Log("Getting here");
    }

    void CompareEnemies(string enemyID)
    {

    }
    //Add the designated arena to the arena track

    /// <summary>
    /// Checks to see if there's a hazard track for the given arena
    /// </summary>
    /// <param name="hazardID">Type of hazard</param>
    /// <param name="arena">The level prefab</param>
    void CompareArenas(string hazardID, Object arena)
    {
        //Add the designated arena to the arena track
        foreach (ArenaDetails ar in arenaPaths)
        {
            if (ar.hazard == hazardID)
            {
                //Ensures the same arena isn't being added twice
                foreach(GameObject level in ar.levels)
                {
                    if (level == arena as GameObject)
                        break;
                }

                ar.levels.Add(arena as GameObject);
                break;
            }
                
        }

        //Make a new one if one doesn't exist
        ArenaDetails newArena = new ArenaDetails();
        newArena.selected = true;
        newArena.hazard = hazardID;
        newArena.levels = new List<GameObject>();
        newArena.levels.Add(arena as GameObject);

        arenaPaths.Add(newArena);
    }

    public void ToggleSelect(bool select, ArenaDetails arena)
    {
        arena.selected = select;
        Debug.Log(arena.selected);
    }
    public void ToggleSelect(bool select, EnemyPathDetails enm)
    {
        enm.selected = select;
    }
}
