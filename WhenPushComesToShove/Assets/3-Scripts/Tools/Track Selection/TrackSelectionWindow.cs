using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class TrackSelectionWindow : EditorWindow
{
    public LevelType levelType;

    //Hazard Selection
    int hazardIndex = 0;
    static List<string> hazardList = new List<string>();
    static public LevelTracks levelTracks;

    [MenuItem("Window/Track Selection Window")]
    public static TrackSelectionWindow SelectionWindow()
    {
        //Make a new window if one isn't already being shown
        if(levelTracks == null)
            levelTracks = new LevelTracks();

        hazardList.Clear();
        foreach(HazardPathDetails haz in levelTracks.dungeonPaths)
        {
            hazardList.Add(SplitID(haz.hazard));
        }

        return (TrackSelectionWindow)EditorWindow.GetWindow(typeof(TrackSelectionWindow));
    }

    public void OnGUI()
    {
        GUILayout.Label("Tracks", EditorStyles.boldLabel);
        levelType = (LevelType)EditorGUILayout.EnumPopup("Level Type", levelType);

        if (levelType == LevelType.Arena)
        {
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < levelTracks.arenaPaths.Count; i++)
            {
               levelTracks.arenaPaths[i].selected = GUILayout.Toggle(levelTracks.arenaPaths[i].selected, SplitID(levelTracks.arenaPaths[i].hazard));
            }
            EditorGUILayout.EndVertical();
        }
        else if(levelType == LevelType.Dungeon)
        {
            hazardIndex = EditorGUILayout.Popup(hazardIndex, hazardList.ToArray());


            for(int i = 0; i < levelTracks.dungeonPaths.Count; i++)
            {
                if (SplitID(levelTracks.dungeonPaths[i].hazard) == hazardList[hazardIndex])
                {
                    EditorGUILayout.BeginVertical();
                    for (int j = 0; j < levelTracks.dungeonPaths[i].enemyTracks.Count; j++)
                    {
                        Debug.Log(levelTracks.dungeonPaths[i].enemyTracks[j].enemies);
                        levelTracks.dungeonPaths[i].enemyTracks[j].selected = GUILayout.Toggle(levelTracks.dungeonPaths[i].enemyTracks[j].selected, SplitID(levelTracks.dungeonPaths[i].enemyTracks[j].enemies));
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            
            

        }
    }

    static string SplitID(string ID)
    {
        string[] ids = Regex.Split(ID, @"(?<!^)(?=[A-Z])");
        string newID = "";

        for(int i = 0; i < ids.Length; i++)
        {
            newID += ids[i] += " ";
        }

        return newID;
    }
}
