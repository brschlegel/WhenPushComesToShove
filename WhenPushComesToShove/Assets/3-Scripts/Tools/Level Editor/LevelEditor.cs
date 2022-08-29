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
    public GameObject placeableLayer;

    //Default Layers
    [Header("Default Layers - DO NOT EDIT")]
    [SerializeField] GameObject defaultFloorLayer;
    [SerializeField] GameObject defaultWallLayer;
    [SerializeField] GameObject defaultPlaceableLayer;

    //Placeable Objects
    [Header("Placeable Objects")]
    [SerializeField] PlaceableObject.ObjectStats[] hazards;
    [SerializeField] PlaceableObject.ObjectStats[] spawnPoints;
    [SerializeField] PlaceableObject.ObjectStats[] decorations;

    [Header("Level Name")]
    public string levelName;

    [Header("Selected File")]
    public GameObject selectedLevel;

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
        if(selectedLevel != null)
        {
            //Update level name
            if (levelName != selectedLevel.name)
                levelName = selectedLevel.name;

            //Update the sprite layers
            if (gameObject.transform.GetChild(0) != selectedLevel.transform.GetChild(0))
            {
                for(int i = gameObject.transform.childCount - 1; i >= 0; i++)
                {
                    DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
                }
            }
        }

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
            Instantiate(level.floorLayer).transform.parent = root.transform;
            Instantiate(level.wallLayer).transform.parent = root.transform;
            Instantiate(level.placeableLayer).transform.parent = root.transform;

            

            string path = "Assets/Resources/Levels/" + root.name + ".prefab";

            GameObject testObj = Resources.Load<GameObject>("Levels/" + root.name);
            if (testObj != null)
            {
                Debug.LogError("Asset already has that name");
                return;
            }

            PrefabUtility.SaveAsPrefabAsset(root, path);

            DestroyImmediate(root);
        }
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
