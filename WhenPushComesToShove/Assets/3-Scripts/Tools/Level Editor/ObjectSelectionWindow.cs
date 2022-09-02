using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class ObjectSelectionWindow : EditorWindow
{
    public enum TypeOfObject {hazards, spawnPoints, decorations};
    public TypeOfObject typeOfObject;

    public static PlaceableObject.ObjectStats[] hazards;
    public static PlaceableObject.ObjectStats[] spawnPoints;
    public static PlaceableObject.ObjectStats[] decorations;

    List<GUIContent> hazardContent = new List<GUIContent>();
    List<GUIContent> spawnContent = new List<GUIContent>();
    List<GUIContent> decoContent = new List<GUIContent>();

    public GameObject selectedObject;

    int toolBarSelect = 0;

    [MenuItem("Window/Object Selection Window")]
    public static ObjectSelectionWindow ShowWindow()
    {
        //Make a new window if one isn't already being shown
        return (ObjectSelectionWindow)EditorWindow.GetWindow(typeof(ObjectSelectionWindow));
    }

    //Anything that needs to be displayed on the window
    public void OnGUI()
    {
        GUILayout.Label("Type of Object", EditorStyles.boldLabel);
        typeOfObject = (TypeOfObject)EditorGUILayout.EnumPopup("Object", typeOfObject);

        switch (typeOfObject)
        {
            case TypeOfObject.hazards:
                ShowHazardsMenu();
                selectedObject = hazards[toolBarSelect].objectPrefab;
                break;
            case TypeOfObject.spawnPoints:
                ShowSpawnPointsMenu();
                selectedObject = spawnPoints[toolBarSelect].objectPrefab;
                break;
            case TypeOfObject.decorations:
                ShowDecorationsMenu();
                selectedObject = decorations[toolBarSelect].objectPrefab;
                break;
        }
    }

    //Updates the toolbar based on what menu is selected
    void ShowHazardsMenu()
    {
        PopulateContentList(hazardContent, hazards);
        
        if(hazardContent.Count > 0)
            toolBarSelect = GUILayout.Toolbar(toolBarSelect, hazardContent.ToArray());
    }

    void ShowSpawnPointsMenu()
    {
        PopulateContentList(spawnContent, spawnPoints);

        if(spawnContent.Count > 0)
            toolBarSelect = GUILayout.Toolbar(toolBarSelect, spawnContent.ToArray());
    }

    void ShowDecorationsMenu()
    {
        PopulateContentList(decoContent, decorations);

        if(decoContent.Count > 0)
            toolBarSelect = GUILayout.Toolbar(toolBarSelect, decoContent.ToArray());
    }

    /// <summary>
    /// Clears the given list and repopulates with plugged in content
    /// </summary>
    /// <param name="list"></param>
    /// <param name="content"></param>
    void PopulateContentList(List<GUIContent> list, PlaceableObject.ObjectStats[] content)
    {
        list.Clear();

        for (int i = 0; i < content.Length; i++)
        {
            list.Add(new GUIContent(content[i].objectName, content[i].objectSprite.texture));
        }
    }
}
#endif
