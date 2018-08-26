using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour {

    // the static instance that will be used to run the IEnumerator methods
    public static CoroutineManager instance;
    // public references to attach the controllers in Unity
    public SteamVR_TrackedObject controller1, controller2;

    // called when the instance awakes in the programme
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            //set the static reference to the newly initialized instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //length is how long the vibration should go for
    //strength is vibration strength from 0-1

    public IEnumerator LongVibrationLeft(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            // this causes 3 second vibration in the left hand (controller2 is left)
            SteamVR_Controller.Input((int)controller2.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3000, strength));
            yield return null;
        }
    }

    public IEnumerator LongVibrationRight(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            // this causes 3 seconds vibration in the right hand
            SteamVR_Controller.Input((int)controller1.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3000, strength));
            yield return null;
        }
    }

    // method to trigger vibration in both controllers
    public IEnumerator LongVibrationBoth(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            // this causes 1 second vibration in both controllers
            SteamVR_Controller.Input((int)controller1.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 1000, strength));
            SteamVR_Controller.Input((int)controller2.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 1000, strength));
            yield return null;
        }
    }

    // This method is used for the turn around command, which consists of 2 subsequent vibrations. 
    //vibrationCount is how many vibrations
    //vibrationLength is how long each vibration should go for
    //gapLength is how long to wait between vibrations
    //strength is vibration strength from 0-1
    public IEnumerator LongVibration(int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        strength = Mathf.Clamp01(strength);
        for (int i = 0; i < vibrationCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(gapLength);
            yield return StartCoroutine(LongVibrationBoth(vibrationLength, strength));
        }
    }
}
