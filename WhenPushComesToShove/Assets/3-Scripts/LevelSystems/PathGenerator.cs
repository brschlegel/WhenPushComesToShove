using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    public GameObject lobby;
    public GameObject modifierRoom;
    public GameObject victoryRoom;

    //All available levels based on modifers
    public List<LevelProperties> availableLevels = new List<LevelProperties>();
    //Any games that have already been played that can be filter out of the available levels
    public List<LevelProperties> playedLevels = new List<LevelProperties>();

    public List<GameObject> path = new List<GameObject>();

    [HideInInspector] public int numOfRooms;

    [SerializeField] private int numOfGames;
    //All possible minigames
    private List<LevelProperties> allLevels = new List<LevelProperties>();
    private int currentPathNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameState.pathGenerator = this;
        numOfRooms = (numOfGames * 2) - 1;

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
    
    /// <summary>
    /// Populates the avaiable levels list with a modifier's games and filters out any that have been played
    /// </summary>
    /// <param name="games">Modifier's list of games it can effect</param>
    public void PopulateAvailableLevels(List<Minigame> games)
    {
        availableLevels.Clear();

        //Adds all minigames to the avaialble list
        if(games.Count == 1 && games[0] == Minigame.All)
        {
            availableLevels = new List<LevelProperties>(allLevels);
        }
        //If "All Minigames" hasn't been selected, then add the rest of the list
        else
        {          
            foreach (Minigame type in games)
            {
                foreach (LevelProperties prop in allLevels)
                {
                    if (prop.game == type)
                    {
                        availableLevels.Add(prop);
                        break;
                    }
                }
            }
        }

        RemovePlayedGames();

    }

    //Helper function to clear out any played games from the possible available levels
    private void RemovePlayedGames()
    {
        foreach (LevelProperties prop in playedLevels)
        {
            availableLevels.Remove(prop);
        }
    }

    public LevelProperties AssignLevel()
    {
        LevelProperties newLevel = null;

        //Grab a random level from the avaiable levels if there ins't a set path
        if(path.Count <= 0 || currentPathNum >= path.Count)
        {
            int rng = Random.Range(0, availableLevels.Count);
            newLevel = availableLevels[rng];
        }
        //Otherwisses, go through the premade path until it runs out of games
        else
        {
            newLevel = path[currentPathNum].GetComponent<LevelProperties>();
            currentPathNum++;
        }
        
        //Add the selected game to the played levels list
        playedLevels.Add(newLevel);
        return newLevel;
    }

    //Instantaites the selected room
    public GameObject SpawnRoom(GameObject level)
    {
        GameObject newLevel = Instantiate(level);
        newLevel.transform.parent = transform;
        return newLevel;
    }

    //Resets the current path to the beginning
    public void ResetPath()
    {
        currentPathNum = 0;

        GameState.playerScores = new int[4] { 0, 0, 0, 0 };
        
        Destroy(transform.GetChild(0).gameObject);

        availableLevels.Clear();
        playedLevels.Clear();

        foreach(LevelProperties level in allLevels)
        {
            availableLevels.Add(level);
        }

        LevelManager.onNewRoom.Invoke();
    }
}
