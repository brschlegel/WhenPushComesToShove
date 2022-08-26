using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ObjectSelectionWindow : EditorWindow
{
    public enum TypeOfObject {hazards, spawnPoints, decorations};
    public TypeOfObject typeOfObject;

    [MenuItem("Window/Object Selection Window")]
    public static void ShowWindow()
    {
        //Make a new window if one isn't already being shown
        EditorWindow.GetWindow(typeof(ObjectSelectionWindow));
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
                break;
            case TypeOfObject.spawnPoints:
                ShowSpawnPointsMenu();
                break;
            case TypeOfObject.decorations:
                ShowDecorationsMenu();
                break;
        }
    }

    void ShowHazardsMenu()
    {
        string[] names = { "1", "2", "3" };
        GUILayout.Toolbar(0, names);
    }

    void ShowSpawnPointsMenu()
    {

    }

    void ShowDecorationsMenu()
    {

    }
}
