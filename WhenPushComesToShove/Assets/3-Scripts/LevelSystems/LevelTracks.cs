using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public struct HazardPathDetails
{
    public string hazard;
    public List<EnemyPathDetails> enemyTracks;
}
public struct EnemyPathDetails
{
    public bool selected;
    public string enemies;
    public List<GameObject> levels;
}
public struct ArenaDetails
{
    public bool selected;
    public string hazard;
    public List<GameObject> levels;
}

public class LevelTracks : MonoBehaviour
{
    Object[] dungeons;
    Object[] arenas;

    static List<HazardPathDetails> dungeonPaths = new List<HazardPathDetails>();
    static List<ArenaDetails> arenaPaths = new List<ArenaDetails>();

    // Start is called before the first frame update
    void Start()
    {
        Init();    
    }

    public void Init()
    {
        dungeons = Resources.LoadAll("Dungeons");
        PopulateDungeonDetails();

        arenas = Resources.LoadAll("Arenas");
        PopulateArenaDetails();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateDungeonDetails()
    {
        foreach(Object dun in dungeons)
        {
            //Split the hazards and enemies IDs and take out the numbers
            //https://stackoverflow.com/questions/4792242/regex-to-get-number-only-from-string
            string[] levelID = Regex.Replace(dun.name, @"\d+", "").Split('_');

            Debug.Log(levelID[0] + " " + levelID[1]);
            foreach(HazardPathDetails hazardPath in dungeonPaths)
            {
             //   if()
            }
            
        }
    }

    void PopulateArenaDetails()
    {
        foreach (Object arena in arenas)
        {
            string[] levelID = arena.name.Split('_');
        }
    }

    //Helper Function
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
        return newPath;
    }

    void CompareEnemies(List<EnemyPathDetails> enemyPaths, string enemyID, Object dungeon)
    {
        //Add the designated dungeon to the enemy track
        foreach(EnemyPathDetails enm in enemyPaths)
        {
            if (enm.enemies == enemyID)
                enm.levels.Add(dungeon as GameObject);
        }

        //Make a new one if one doesn't exist
        EnemyPathDetails newPath = new EnemyPathDetails();
        newPath.selected = true; 
        newPath.enemies = enemyID;
        newPath.levels = new List<GameObject>();
        newPath.levels.Add(dungeon as GameObject);
    }

    void CompareArenas(string hazardID, Object arena)
    {
        foreach(ArenaDetails ar in arenaPaths)
        {
            if (ar.hazard == hazardID)
                ar.levels.Add(arena as GameObject);
        }

        ArenaDetails newArena = new ArenaDetails();
        newArena.selected = true;
        newArena.hazard = hazardID;
        newArena.levels.Add(arena as GameObject);

        arenaPaths.Add(newArena);
    }
}
