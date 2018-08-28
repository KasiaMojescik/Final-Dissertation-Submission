using UnityEngine;
using System.Collections;

public class NavObject : MonoBehaviour
{
    // Enumerator to list possible directions
    public enum Direction { Left, Right, Straight, Around };
    // To specify the type of cue in Unity
    public bool isArrow;
    public bool isVoice;
    public bool isHaptic;
    // To specify the direction in Unity
    public Direction direction;


    void Start()
    {
        // if you specify that the cue is not an arrow, make the arrows invisible
        if (!isArrow)
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
        
    }
   
    // this method specifies what happens during collision
    private void OnTriggerEnter(Collider other)
    {
        // if the cue is colliding with a Player (through CollectorObject)
        if (other.CompareTag("Player"))
        {
            // if the cue was specified to be vocal, activate the sound for this cue
            if (isVoice)
            {
                activateSounds();
            }
            // if the cue was specified to be haptic, activate the haptics for this cue
            if (isHaptic)
            {
                activateVibrations();
            }
        }

    }
    // this method checks which sound to activate for a vocal cue
    private void activateSounds()
    {
        // find an instance of the AudioManager object
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        // check which direction was specified for this cue in Unity
        switch (direction)
        {
            case Direction.Left:
                {
                    // if the direction specified was left, play the left command
                    audioManager.Play("left");
                    break;
                }
            case Direction.Right:
                {
                    // if the direction specified was right, play the right command
                    audioManager.Play("right");
                    break;
                }
            case Direction.Straight:
                {
                    // if the direction specified was straight, play the straight Sound object
                    audioManager.Play("straight");
                    break;
                }
            case Direction.Around:
                {
                    // if the direction specified was to turn around, play the turn around Sound object
                    // this sound was used in the pilot study
                    audioManager.Play("around");
                    break;
                }
        }
    }

    // this method specifies how to activate the tactile vibrations on the controllers
    private void activateVibrations()
    {
        // find an instance of the CoroutineManager
        CoroutineManager coroutineManager = FindObjectOfType<CoroutineManager>();
        // check the direction specified in Unity
        switch (direction)
        {
            // if the direction is left, make controller2 vibrate
            // because controller2 is set to be the left controller in Unity
            // by calling LongVibrationController2 method in CoroutineManager
            case Direction.Left:
                {
                    coroutineManager.StartCoroutine(coroutineManager.LongVibrationController2(1, 1));
                    break;
                }
            case Direction.Right:

                {
                    // if the direction is right, make controller1 vibrate
                    // because controller1 is set to be the right controller in Unity
                    // by calling LongVibrationController1 method in CoroutineManager
                    coroutineManager.StartCoroutine(coroutineManager.LongVibrationController1(1, 1));
                    break;
                }
            case Direction.Straight:
                {
                    // if the direction is straight, make both controllers vibrate
                    // by calling the LongVibration method in CoroutineManager
                    coroutineManager.StartCoroutine(coroutineManager.LongVibration(1, 1, 1, 1));
                    break;
                }
            case Direction.Around:
                {
                    // if the direction is turn around, make both controllers vibrate twice
                    // by calling the LongVibration method in CoroutineManager
                    // this direction was only used in the pilot study
                    coroutineManager.StartCoroutine(coroutineManager.LongVibration(2, 1, 1, 1));
                    break;
                }
        }
    }
}

