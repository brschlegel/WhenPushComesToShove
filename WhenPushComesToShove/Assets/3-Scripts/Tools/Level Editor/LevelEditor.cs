using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class LevelEditor : MonoBehaviour
{
    //Layers
    [Header("Layers - DO NOT EDIT")]
    public GameObject floorLayer;
    public GameObject wallLayer;
    public GameObject fadeablelayer;
    public GameObject placeableLayer;

    //Default Layers
    [Header("Default Layers - DO NOT EDIT")]
    [SerializeField] GameObject defaultFloorLayer;
    [SerializeField] GameObject defaultWallLayer;
    [SerializeField] GameObject defaultFadeableLayer;
    [SerializeField] GameObject defaultPlaceableLayer;

    //Placeable Objects
    [Header("Placeable Objects - DO NOT EDIT")]
    [SerializeField] PlaceableObject.ObjectStats[] hazards;
    [SerializeField] PlaceableObject.ObjectStats[] spawnPoints;
    [SerializeField] PlaceableObject.ObjectStats[] endConditions;
    [SerializeField] PlaceableObject.ObjectStats[] decorations;

    //Object Window Style
    [Header("Object Window Style - DO NOT EDIT")]
    [SerializeField] GUIStyle style;

    [Header("Level Properties")]
    public string levelName;
    public HazardDifficulty.HazardStats[] hazardStats;
    public EnemyDifficulty.EnemyLevelStats[] enemyStats;
    public LevelType levelType;

    [Header("Selected File")]
    public GameObject selectedLevel;
    GameObject previousSelectedLevel;
    GameObject selectedLevelFirstChild;

    ObjectSelectionWindow currentWindow;

    [HideInInspector] public Vector3 mousePos;

    private void Awake()
    {
        previousSelectedLevel = selectedLevel;
    }

    // Start is called before the first frame update
    void Update()
    {
        //Closes the Object Slection Window if the Level Editor is deselected
        if (!Selection.Contains(gameObject))
        {
            currentWindow?.Close();
        }

        //Update the level editor to the selected level
        if(selectedLevel != null && previousSelectedLevel != selectedLevel)
        {
            //Update level name
            if (levelName != selectedLevel.name)
                levelName = selectedLevel.name;

            //Update stats
            LevelProperties selectedProps = selectedLevel.GetComponent<LevelProperties>();

            //Hazard stats
            hazardStats = selectedProps.hazards;

            //Enemy stats
            enemyStats = selectedProps.enemyStats;

            //Wave Manager
            WaveManager selectedWaveManager = selectedLevel.GetComponent<WaveManager>();

            if(selectedWaveManager != null)
                GetComponent<WaveManager>().waveDelays = selectedWaveManager.waveDelays;

            levelType = selectedProps.levelType;

            //Update the sprite layers to match the selected Level

            //Compares the first layer to see if it matches
            if (gameObject.transform.GetChild(0) != selectedLevelFirstChild)
            {
                UnpackLevel(selectedLevel);
            }
                
        }
        previousSelectedLevel = selectedLevel;
    }

    public void OpenWindow()
    {
        if(!Selection.Contains(gameObject))
        {
            Debug.LogWarning("Please select Level Editor to open this window");
            return;
        }

        currentWindow = ObjectSelectionWindow.ShowWindow(style);

        //Assign and update placeable objects
        if (hazards != ObjectSelectionWindow.hazards)
            ObjectSelectionWindow.hazards = hazards;

        if (spawnPoints != ObjectSelectionWindow.spawnPoints)
            ObjectSelectionWindow.spawnPoints = spawnPoints;

        if (endConditions != ObjectSelectionWindow.endConditions)
            ObjectSelectionWindow.endConditions = endConditions;

        if (decorations != ObjectSelectionWindow.decorations)
            ObjectSelectionWindow.decorations = decorations;
    }

    //Spawns the selected object at the current mouse position
    public void SpawnObject()
    {
        //Will only spawn an object if the Object Selection Window is open
        if (EditorWindow.HasOpenInstances<ObjectSelectionWindow>())
        {
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(currentWindow.selectedObject);
            newObject.transform.position = mousePos;
            newObject.transform.parent = placeableLayer.transform;
        }         
    }

    public void DeleteChildren(GameObject parent, int startingIndex = -1, int endIndex = 0)
    {
        if (startingIndex == -1)
            startingIndex = parent.transform.childCount;
        for (int i = startingIndex - 1; i >= endIndex; i--)
        {
            DestroyImmediate(parent.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Spawns a prefabed level, unpacks it, and assigns the appropriate layers into the level editor
    /// </summary>
    /// <param name="level">A prefabed level gameObject</param>
    public void UnpackLevel(GameObject level)
    {
        if (!PrefabUtility.IsPartOfAnyPrefab(level))
        {
            Debug.LogError("Object is not a prefab");
            return;
        }

        DeleteChildren(gameObject, gameObject.transform.childCount);

        GameObject levelRoot = (GameObject)PrefabUtility.InstantiatePrefab(level);
        PrefabUtility.UnpackPrefabInstance(levelRoot, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);


        //Add the selected levels sprite layers to the editor
        selectedLevelFirstChild = levelRoot.transform.GetChild(0).gameObject;
        selectedLevelFirstChild.transform.parent = transform;
        floorLayer = selectedLevelFirstChild;

        GameObject secondLayer = levelRoot.transform.GetChild(0).gameObject;
        secondLayer.transform.parent = transform;
        wallLayer = secondLayer;

        GameObject thirdLayer = levelRoot.transform.GetChild(0).gameObject;
        thirdLayer.transform.parent = transform;
        fadeablelayer = thirdLayer;

        GameObject fourthLayer = levelRoot.transform.GetChild(0).gameObject;
        fourthLayer.transform.parent = transform;
        placeableLayer = fourthLayer;

        DestroyImmediate(levelRoot);
    }

    /// <summary>
    /// Resets the level editor to the default layers
    /// </summary>
    public void Reset()
    {
        //Resets the editor if a level is loaded in
        if(gameObject.transform.childCount > 4)
        {
            DeleteChildren(gameObject, gameObject.transform.childCount, gameObject.transform.childCount - 4);
            //SetActiveChildren(gameObject, true, 4);

            floorLayer = gameObject.transform.GetChild(0).gameObject;
            wallLayer = gameObject.transform.GetChild(1).gameObject;
            fadeablelayer = gameObject.transform.GetChild(2).gameObject;
            placeableLayer = gameObject.transform.GetChild(3).gameObject;
        }
        //Resets the editor from scrath
        else
        {
            DeleteChildren(gameObject, gameObject.transform.childCount);

            floorLayer = Instantiate(defaultFloorLayer);
            floorLayer.transform.parent = transform;
            floorLayer.name = "Floor Tile Map";

            wallLayer = Instantiate(defaultWallLayer);
            wallLayer.transform.parent = transform;
            wallLayer.name = "Wall Tile Map";

            fadeablelayer = Instantiate(defaultFadeableLayer);
            fadeablelayer.transform.parent = transform;
            fadeablelayer.name = "Fadeable Object Tile Map";

            placeableLayer = Instantiate(defaultPlaceableLayer);
            placeableLayer.transform.parent = transform;
            placeableLayer.name = "Placeable Objects";
        }

        levelName = "";
        selectedLevel = null;
        hazardStats = null;
        enemyStats = null;
        GetComponent<WaveManager>().waveDelays.Clear();

    }
}

[CustomEditor(typeof(LevelEditor))]
public class CustomLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LevelEditor level = (LevelEditor)target;

        //Open the Object Selection Window
        if (GUILayout.Button("Open Object Selection Window"))
        {
            level.OpenWindow();
        }

        //Save the current level
        if (GUILayout.Button("Save Level"))
        {
            //Spawn the object
            GameObject root = new GameObject(level.levelName);
            LevelProperties levelProp = root.AddComponent<LevelProperties>();
            root.AddComponent<Grid>();

            WaveManager rootWaveManager = root.AddComponent<WaveManager>();
            WaveManager levelWaveManager = level.GetComponent<WaveManager>();
            levelProp.waveManager = rootWaveManager;

            //Will only add the WaveManager if there's delays set
            if(levelWaveManager.waveDelays.Count > 0)
                rootWaveManager.waveDelays = levelWaveManager.waveDelays;

            //GameObject floorLayer = new GameObject(level.levelName + " Floor Tile Map");
            //floorLayer = level.floorLayer;
            level.floorLayer.transform.parent = root.transform;
            level.floorLayer.name = level.levelName + " Floor Tile Map";

            level.wallLayer.transform.parent = root.transform;
            level.wallLayer.name = level.levelName + " Wall Tile Map";

            level.fadeablelayer.transform.parent = root.transform;
            level.fadeablelayer.name = level.levelName + " Fadeable Object Tile Map";

            level.placeableLayer.transform.parent = root.transform;
            level.placeableLayer.name = level.levelName + " Placeable Objects";

            //Check to make sure it has enough spawn points
            int numOfPlayerSpawn = 0;
            List<GameObject> playerSpawns = new List<GameObject>();

            for(int i = 0; i < level.placeableLayer.transform.childCount; i++)
            {
                if (level.placeableLayer.transform.GetChild(i).CompareTag("PlayerSpawn"))
                {
                    numOfPlayerSpawn++;
                    playerSpawns.Add(level.placeableLayer.transform.GetChild(i).gameObject);
                }
            }

            if(numOfPlayerSpawn < 4)
            {
                Debug.LogError("Not enough player spawn positions. 4 are required.");
                playerSpawns.Clear();
                return;
            }
            else if(numOfPlayerSpawn > 4)
            {
                Debug.LogError("Too many player spawn positions. 4 are required.");
                playerSpawns.Clear();
                return;
            }
            else
            {
                levelProp.playerSpawns = playerSpawns.ToArray();
                playerSpawns.Clear();
            }

            //Check to make sure there's at least one enemy spawn point

            List<GameObject> enemySpawns = new List<GameObject>();
            for(int i = 0; i < level.placeableLayer.transform.childCount; i++)
            {
                if (level.placeableLayer.transform.GetChild(i).CompareTag("EnemySpawn"))
                    enemySpawns.Add(level.placeableLayer.transform.GetChild(i).gameObject);
            }

            if(enemySpawns.Count <= 0)
            {
                Debug.LogError("There must be at least 1 enemy spawn point");
                return;
            }
            else
            {
                levelProp.enemySpawns = enemySpawns.ToArray();
                enemySpawns.Clear();
            }

            //Check to make sure that there's at least one hazard stat
            if (level.hazardStats.Length <= 0)
            {
                Debug.LogError("There needs to be at least 1 hazard stat.");
                return;
            }
            else
                levelProp.hazards = level.hazardStats;

            if (level.enemyStats.Length <= 0)
            {
                Debug.LogError("There needs to be at least 1 enemy stat.");
                return;
            }
            else
                levelProp.enemyStats = level.enemyStats;

            if(root.name == "")
            {
                Debug.LogError("Level name can't be blank.");
                return;
            }

            levelProp.levelType = level.levelType;

            string path = "";
            GameObject testObj = null;
            if (level.levelType == LevelType.Dungeon)
            {
                path = "Assets/2-Prefabs/Levels/Resources/Dungeons/" + root.name + ".prefab";
                testObj = Resources.Load<GameObject>("Levels/" + root.name);
            }              
            else if(level.levelType == LevelType.Arena)
            {
                path = "Assets/2-Prefabs/Levels/Resources/Arenas/" + root.name + ".prefab";
                testObj = Resources.Load<GameObject>("Levels/Arenas/" + root.name);
            }
                

            
            if (testObj != null && level.selectedLevel == null)
            {
                Debug.LogError("Asset already has that name");
                DestroyImmediate(root);
                return;
            }

            GameObject savedLevel = PrefabUtility.SaveAsPrefabAsset(root, path);

            //Resets the level editor layers with the current saved level
            level.UnpackLevel(savedLevel);

            DestroyImmediate(root);
        }

        if (GUILayout.Button("Reset to Default"))
            level.Reset();
    }

    public void OnSceneGUI()
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        LevelEditor level = (LevelEditor)target;

        //Gets the mouse's world position
        Ray ray;
        ray = HandleUtility.GUIPointToWorldRay(new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y));
        Vector2 mousePos = ray.origin;

       
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            level.mousePos = mousePos;
            level.mousePos.z = 0.0f;

            level.SpawnObject();
        }
    }
}
#endif
