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
    [SerializeField] PlaceableObject.ObjectStats[] decorations;

    [Header("Level Properties")]
    public string levelName;
    public HazardDifficulty.HazardStats[] hazardStats;

    [Header("Selected File")]
    public GameObject selectedLevel;
    GameObject previousSelectedLevel;
    GameObject selectedLevelFirstChild;

    ObjectSelectionWindow currentWindow;

    [HideInInspector] public Vector3 mousePos;
    
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
            Debug.Log(previousSelectedLevel);
            //Update level name
            if (levelName != selectedLevel.name)
                levelName = selectedLevel.name;

            //Update hazard stats
            hazardStats = selectedLevel.GetComponent<LevelProperties>().hazards;

            //Update the sprite layers to match the selected Level
            if (gameObject.transform.GetChild(0) != selectedLevelFirstChild)
            {
                //Add the selected levels sprite layers to the editor
                selectedLevelFirstChild = Instantiate(selectedLevel.transform.GetChild(0).gameObject);
                selectedLevelFirstChild.transform.parent = transform;
                floorLayer = selectedLevelFirstChild;

                GameObject secondLayer = Instantiate(selectedLevel.transform.GetChild(1).gameObject);
                secondLayer.transform.parent = transform;
                wallLayer = secondLayer;

                GameObject thirdLayer = Instantiate(selectedLevel.transform.GetChild(2).gameObject);
                thirdLayer.transform.parent = transform;
                placeableLayer = thirdLayer;

                GameObject forthLayer = Instantiate(selectedLevel.transform.GetChild(3).gameObject);
                forthLayer.transform.parent = transform;
                placeableLayer = forthLayer;

                if (previousSelectedLevel == null)
                    SetActiveChildren(gameObject, false, gameObject.transform.childCount - 4);
                else
                    DeleteChildren(gameObject, gameObject.transform.childCount - 4);               
            }
                
        }
        previousSelectedLevel = selectedLevel;
    }

    public void OpenWindow()
    {
        currentWindow = ObjectSelectionWindow.ShowWindow();

        //Assign and update placeable objects
        if (hazards != ObjectSelectionWindow.hazards)
            ObjectSelectionWindow.hazards = hazards;

        if (spawnPoints != ObjectSelectionWindow.spawnPoints)
            ObjectSelectionWindow.spawnPoints = spawnPoints;

        if (decorations != ObjectSelectionWindow.decorations)
            ObjectSelectionWindow.decorations = decorations;
    }

    //Spawns the selected object at the current mouse position
    public void SpawnObject()
    {
        if (EditorWindow.HasOpenInstances<ObjectSelectionWindow>())
        {
            GameObject newObject = Instantiate(currentWindow.selectedObject, mousePos, Quaternion.identity);
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

    public void SetActiveChildren(GameObject parent, bool active, int startingIndex = -1)
    {
        if (startingIndex == -1)
            startingIndex = parent.transform.childCount;
        for (int i = startingIndex - 1; i >= 0; i--)
        {
            parent.transform.GetChild(i).gameObject.SetActive(active);
        }
    }

    public void Reset()
    {
        //Resets the editor if a level is loaded in
        if(gameObject.transform.childCount > 4)
        {
            DeleteChildren(gameObject, gameObject.transform.childCount, gameObject.transform.childCount - 4);
            SetActiveChildren(gameObject, true, 4);

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

            Instantiate(level.floorLayer).transform.parent = root.transform;
            Instantiate(level.wallLayer).transform.parent = root.transform;
            Instantiate(level.fadeablelayer).transform.parent = root.transform;

            GameObject objLayer = Instantiate(level.placeableLayer);
            objLayer.transform.parent = root.transform;

            //Check to make sure it has enough spawn points
            int numOfPlayerSpawn = 0;
            List<GameObject> playerSpawns = new List<GameObject>();

            for(int i = 0; i < objLayer.transform.childCount; i++)
            {
                if (objLayer.transform.GetChild(i).CompareTag("PlayerSpawn"))
                {
                    numOfPlayerSpawn++;
                    playerSpawns.Add(objLayer.transform.GetChild(i).gameObject);
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
            for(int i = 0; i < objLayer.transform.childCount; i++)
            {
                if (objLayer.transform.GetChild(i).CompareTag("EnemySpawn"))
                    enemySpawns.Add(objLayer.transform.GetChild(i).gameObject);
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
                Debug.Log("There needs to be at least 1 hazard stat.");
                return;
            }
            else
                levelProp.hazards = level.hazardStats;

            if(root.name == "")
            {
                Debug.LogError("Level name can't be blank.");
                return;
            }    
            string path = "Assets/Resources/Levels/" + root.name + ".prefab";

            GameObject testObj = Resources.Load<GameObject>("Levels/" + root.name);
            if (testObj != null)
            {
                Debug.LogError("Asset already has that name");
                DestroyImmediate(root);
                return;
            }

            PrefabUtility.SaveAsPrefabAsset(root, path);

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
