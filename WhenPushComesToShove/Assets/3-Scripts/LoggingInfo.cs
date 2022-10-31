using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingInfo : MonoBehaviour
{
    public static LoggingInfo instance;

    private const string surveyURL = "https://rit.az1.qualtrics.com/jfe/form/SV_6L0Wx4A2i7XIdcG";

    //Variables to log
    [HideInInspector] public string[] playerDeaths = new string[4];
    [HideInInspector] public int[] heavyShoveUses = new int[4] { 0, 0, 0, 0 };
    [HideInInspector] public int[] heavyShoveInterrupts = new int[4] { 0, 0, 0, 0 };
    [HideInInspector] public int[] dashUses = new int[4] { 0, 0, 0, 0 };
    [HideInInspector] public int numOfRoomsTraveled = 0;
    [HideInInspector] public int numOfRunsFailed = 0;
    [HideInInspector] public int numOfRunsCompleted = 0;

    [HideInInspector] public string currentRoomName = "";

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("An instance of LoggingInfo already exists");
        }
    }

    private void Update()
    {

#if UNITY_STANDALONE && !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SendInfoToSurvey();
            Application.Quit();
        }
#endif
    }

    public void AddPlayerDeath(int playerIndex, string causeOfDeath)
    {
        playerDeaths[playerIndex] += "_" + causeOfDeath +"-InRoom:" + currentRoomName;
    }

    public void SendInfoToSurvey()
    {
        //Woo lets make a string
        string fullURL = surveyURL + "?gameName=WhenPushComesToShove-";

        //Player Death
        for (int i = 0; i < playerDeaths.Length; i++)
        {
            if (playerDeaths[i] != "")
            {
                fullURL += "&playerDeaths_" + (i + 1).ToString() + "=" + playerDeaths[i] + "\n";
            }
        }

        //Heavy Shove Uses
        for (int i = 0; i < heavyShoveUses.Length; i++)
        {
            fullURL += "&numOfHeavyShoves_" + (i + 1).ToString() + "=" + heavyShoveUses[i];
        }

        //Heavy Shove Interrupts
        for (int i = 0; i < heavyShoveInterrupts.Length; i++)
        {
            fullURL += "&numOfHeavyShoveInterrupts_" + (i + 1).ToString() + "=" + heavyShoveInterrupts[i];
        }

        //Dash Uses
        for (int i = 0; i < dashUses.Length; i++)
        {
            fullURL += "&numOfDash_" + (i + 1).ToString() + "=" + dashUses[i];
        }

        fullURL += "&numOfRoomsTraveled=" + numOfRoomsTraveled;
        fullURL += "&numOfRunsFailed=" + numOfRunsFailed;
        fullURL += "&numOfRunsCompleted=" + numOfRunsCompleted;

        Application.OpenURL(fullURL);
        Debug.Log(fullURL);
    }
}
