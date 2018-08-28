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
        // if the instance of AudioManager already exists, destroy it
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            // set the static reference to the newly initialized instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // this method activates vibrations in controller2
    // it activates them in the left controller because controller2 is set to left in Unity
    // length is how long the vibration should go for
    // strength is vibration strength from 0-1
    public IEnumerator LongVibrationController2(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            // this causes 3 second vibration in the left hand (controller2 is left)
            SteamVR_Controller.Input((int)controller2.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3000, strength));
            yield return null;
        }
    }

    // this method activates vibrations in controller1
    // it activates them in the right controller because controller1 is set to right in Unity
    // length is how long the vibration should go for
    // strength is vibration strength from 0-1
    public IEnumerator LongVibrationController1(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            // this causes 3 seconds vibration in the right hand
            SteamVR_Controller.Input((int)controller1.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3000, strength));
            yield return null;
        }
    }

    // method to called in LongVibration method to trigger vibration in both controllers
    // length is how long the vibration should go for
    // strength is vibration strength from 0-1
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

    // This method is used for the straight and turn around command
    // straight consists of a single vibration in both controllers
    // turn around consists of 2 subsequent vibrations. 
    // vibrationCount is how many vibrations should be activated subsequently
    // vibrationLength is how long each vibration last
    // gapLength is how long to wait between vibrations
    // strength is vibration strength from 0-1
    public IEnumerator LongVibration(int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        strength = Mathf.Clamp01(strength);
        // repeat as many times as you want the controllers to vibrate
        for (int i = 0; i < vibrationCount; i++)
        {
            // if there is two or more vibrations, wait gapLength after the first vibration
            if (i != 0) yield return new WaitForSeconds(gapLength);
            // vibrate in both controllers
            yield return StartCoroutine(LongVibrationBoth(vibrationLength, strength));
        }
    }
}
