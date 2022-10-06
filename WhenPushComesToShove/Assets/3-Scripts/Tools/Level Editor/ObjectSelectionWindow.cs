using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class ObjectSelectionWindow : EditorWindow
{
    public enum TypeOfObject {hazards, spawnPoints, endConditions, decorations};
    public TypeOfObject typeOfObject;

    public static PlaceableObject.ObjectStats[] hazards;
    public static PlaceableObject.ObjectStats[] spawnPoints;
    public static PlaceableObject.ObjectStats[] endConditions;
    public static PlaceableObject.ObjectStats[] decorations;

    List<string> hazardContent = new List<string>();
    List<string> spawnContent = new List<string>();
    List<string> endContent = new List<string>();
    List<string> decoContent = new List<string>();

    public GameObject selectedObject;

    int toolBarSelect = 0;

    static GUIStyle style;

    [MenuItem("Window/Object Selection Window")]
    public static ObjectSelectionWindow ShowWindow(GUIStyle _style)
    {
        style = _style;
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
            case TypeOfObject.endConditions:
                ShowEndConditionsMenu();
                selectedObject = endConditions[toolBarSelect].objectPrefab;
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
            toolBarSelect = GUILayout.SelectionGrid(toolBarSelect, hazardContent.ToArray(), 3, style);
    }

    void ShowSpawnPointsMenu()
    {
        PopulateContentList(spawnContent, spawnPoints);

        if(spawnContent.Count > 0)
            toolBarSelect = GUILayout.SelectionGrid(toolBarSelect, spawnContent.ToArray(), 3, style);
    }

    void ShowEndConditionsMenu()
    {
        PopulateContentList(endContent, endConditions);
        if (endContent.Count > 0)
            toolBarSelect = GUILayout.SelectionGrid(toolBarSelect, endContent.ToArray(), 3, style);
    }

    void ShowDecorationsMenu()
    {
        PopulateContentList(decoContent, decorations);

        if(decoContent.Count > 0)
            toolBarSelect = GUILayout.SelectionGrid(toolBarSelect, decoContent.ToArray(), 3,style);
    }

    /// <summary>
    /// Clears the given list and repopulates with plugged in content
    /// </summary>
    /// <param name="list"></param>
    /// <param name="content"></param>
    void PopulateContentList(List<string> list, PlaceableObject.ObjectStats[] content)
    {
        list.Clear();

        for (int i = 0; i < content.Length; i++)
        {
            list.Add(content[i].objectName);
        }
    }
}
#endif
