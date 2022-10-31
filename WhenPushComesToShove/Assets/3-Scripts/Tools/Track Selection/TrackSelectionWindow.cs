using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.IO;

#if UNITY_EDITOR
public class TrackSelectionWindow : EditorWindow
{
    public LevelType levelType;

    //Hazard Selection
    int hazardIndex = 0;
    static List<string> hazardList = new List<string>();
    static public LevelTracks levelTracks;


    [MenuItem("Window/Track Selection Window")]
    public static void SelectionWindow()
    {
        //Tests if the LevelTrack json file exists
        if(File.Exists(Application.dataPath + "/Resources/LevelTrack.json"))
        {
            //https://allison-liem.medium.com/unity-reading-external-json-files-878ed0978977
            TextAsset textAsset = Resources.Load<TextAsset>("LevelTrack");
            levelTracks = JsonUtility.FromJson<LevelTracks>(textAsset.text);
        }
        //Make a new LevelTracks if one doesn't exist
        else
        {
            levelTracks = new LevelTracks();
            levelTracks.Init();
        }

        //Make a new window if one isn't already being shown
        hazardList.Clear();
        foreach(HazardPathDetails haz in levelTracks.dungeonPaths)
        {
            hazardList.Add(SplitID(haz.hazard));
        }

        TrackSelectionWindow trackWindow = (TrackSelectionWindow)EditorWindow.GetWindow(typeof(TrackSelectionWindow));
        trackWindow.Show();
    }


    private void OnDestroy()
    {
        //https://prasetion.medium.com/saving-data-as-json-in-unity-4419042d1334
        string levelTrackString = JsonUtility.ToJson(levelTracks);
        File.WriteAllText(Application.dataPath + "/Resources/LevelTrack.json", levelTrackString);
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
#endif
