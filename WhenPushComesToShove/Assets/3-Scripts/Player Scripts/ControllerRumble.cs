using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum RumbleType
{
    Constant,
    Pulse,
    Linear
}

public class ControllerRumble : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler input;

    private RumbleType currentRumbleType;
    private float rumbleTime;
    private float pulseTime;
    private float lowFreq;
    private float highFreq;
    private float rumbleStep;
    private bool isMotorActive = false;
    private Coroutine stopRoutine;
    private Gamepad gamepad;
    private float lowEndFreq;
    private float highEndFreq;
    private float lowStep;
    private float highStep;

    private void Start()
    {
        if (gamepad == null)
        {
            gamepad = GetGamepad();
        }
    }

    private bool CanRumble 
    {
        get {
            if(input.playerConfig == null)
                return false;
            return !(input.playerConfig.Input.currentControlScheme != "Xbox Controller Scheme");
            }
    }

    private void Update()
    {
        //Ensure that we should be rumbling
        if (Time.time > rumbleTime)
        {
            return;
        }

        if (gamepad !=  null)
        {
            gamepad = GetGamepad();
           
        }
        Debug.Log( input.playerConfig.Input.currentControlScheme);
        if (gamepad == null || !CanRumble )
        {
            
            return;
        }

        switch (currentRumbleType)
        {
            case RumbleType.Constant:
                //gamepad.SetMotorSpeeds(lowFreq, highFreq);
                break;
            case RumbleType.Pulse:

                if (Time.time > pulseTime)
                {
                    isMotorActive = !isMotorActive;
                    pulseTime = Time.time + rumbleStep;

                    if (!isMotorActive)
                    {
                        gamepad.SetMotorSpeeds(0, 0);
                    }
                    else
                    {
                        gamepad.SetMotorSpeeds(lowFreq, highFreq);
                    }
                }
                break;
            case RumbleType.Linear:
                gamepad.SetMotorSpeeds(lowFreq, highFreq);
                lowFreq += (lowStep * Time.deltaTime);
                highFreq += (highStep * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    #region RumbleTriggers
    /// <summary>
    /// Sets a constant rumble for a duration
    /// </summary>
    /// <param name="low">Low Rumble Frequency</param>
    /// <param name="high">High Rumble Frequency</param>
    /// <param name="duration">Time For Rumble</param>
    public void RumbleConstant(float low, float high, float duration)
    {
        #if UNITY_STANDALONE && !UNITY_EDITOR
        if(!CanRumble)
            return;

        currentRumbleType = RumbleType.Constant;
        lowFreq = low;
        highFreq = high;
        rumbleTime = Time.time + duration;

        if (stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
        }

        gamepad.SetMotorSpeeds(lowFreq, highFreq);

        stopRoutine = StartCoroutine(StopRumbleOverTime(duration));
        #endif
    }

    /// <summary>
    /// Sets a rumble to pulse with breaks in between for a set time
    /// </summary>
    /// <param name="low">Low Rumble Frequency</param>
    /// <param name="high">High Rumble Frequency</param>
    /// <param name="burstTime">Time For Pulse</param>
    /// <param name="duration">Time For Rumble</param>
    public void RumblePulse(float low, float high, float burstTime, float duration)
    {
#if UNITY_STANDALONE && !UNITY_EDITOR
    if(!CanRumble)
        return 
        currentRumbleType = RumbleType.Pulse;
        lowFreq = low;
        highFreq = high;
        rumbleStep = burstTime;
        pulseTime = Time.time + burstTime;
        rumbleTime = Time.time + duration;

        if (stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
        }

        isMotorActive = true;
        gamepad.SetMotorSpeeds(lowFreq, highFreq);

        stopRoutine = StartCoroutine(StopRumbleOverTime(duration));
#endif
    }

    /// <summary>
    /// Sets a rumble to change intensity over the duration
    /// </summary>
    /// <param name="low">Low Rumble Frequency</param>
    /// <param name="high">High Rumble Frequency</param>
    /// <param name="duration">Time For Rumble</param>
    public void RumbleLinear(float lowStart, float lowEnd, float highStart, float highEnd, float duration, bool stayConstantAfter)
    {
        if(!CanRumble)
            return;
#if UNITY_STANDALONE && !UNITY_EDITOR
        currentRumbleType = RumbleType.Linear;
        lowFreq = lowStart;
        highFreq = highStart;
        lowEndFreq = lowEnd;
        highEndFreq = highEnd;

        lowStep = (lowEnd - lowStart) / duration;
        highStep = (highEnd - highStart) / duration;

        rumbleTime = Time.time + duration;

        if (stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
        }

        //isMotorActive = true;
        //gamepad.SetMotorSpeeds(lowFreq, highFreq);

        stopRoutine = StartCoroutine(StopRumbleOverTime(duration, stayConstantAfter));
#endif
    }
#endregion

    /// <summary>
    /// Stops the rumble after a set time
    /// </summary>
    /// <param name="timeBeforeStop"></param>
    /// <returns></returns>
    public IEnumerator StopRumbleOverTime(float timeBeforeStop, bool goToConstant = false)
    {
        
        yield return new WaitForSeconds(timeBeforeStop);
  
        if (gamepad != null && CanRumble)
        {
            if (goToConstant)
            {
                currentRumbleType = RumbleType.Constant;
                gamepad.SetMotorSpeeds(lowFreq, highFreq);
            }
            else
            {
                gamepad.SetMotorSpeeds(0, 0);
            }
        }

    }

    /// <summary>
    /// Helper Function to Force Stop the Rumble
    /// </summary>
    public void ForceStopRumble()
    {
        if(!CanRumble)
            return;
        if (stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
        }

        gamepad.SetMotorSpeeds(0, 0);
        stopRoutine = null;

    }

    /// <summary>
    /// Helper function to find the players current controller
    /// </summary>
    /// <returns></returns>
    private Gamepad GetGamepad()
    {
        Gamepad gamepad = null;
        foreach (Gamepad g in Gamepad.all)
        {
            foreach (InputDevice d in input.playerConfig.Input.devices)
            {
                if (d.deviceId == g.deviceId)
                {
                    gamepad = g;
                    break;
                }
            }

            if (gamepad != null)
            {
                break;
            }
        }

        return gamepad;
    }

    public void OnApplicationQuit()
    {
        foreach (Gamepad g in Gamepad.all)
        {
            g.SetMotorSpeeds(0, 0);
        }
    }
}
