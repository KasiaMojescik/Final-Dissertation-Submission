using UnityEngine;
using UnityEngine.UI;

public class CollectorController : MonoBehaviour
{
    // to display pick up objects
    public Transform pickupHolder;
    private int currentPickupIndex = 0;
    private int numberOfPickups;

    // to display navigation cues
    public Transform cueHolder;
    private int currentCueIndex = 0;
    private int numberOfCues;

    // to show results
    private int count;
    public Text winText;
    public Text countText;
    public Text distanceText;

    // to calculate distance travelled
    float distanceTravelled = 0;
    Vector3 lastPosition;

    // to calculate time spent playing
    float playedTime;
    public Text time;

    void Start()
    {
        SetTextFields();

        // activating first arrow and first PickUp object
        if (cueHolder != null)
        {
            ActivateArrows();
        }

        if (pickupHolder != null)
        {
            ActivatePickups();
        }
    }

    private void FixedUpdate()
    {
        // to fix the CollectorObject in right position
        transform.eulerAngles = Vector3.zero;
        // for measurement of the time
        playedTime += Time.deltaTime;
        // for measurement of the distance walked
        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        // to measure most recent position
        lastPosition = transform.position;
        // if the participants take longer than 3 minutes on the maze, 
        // the trial is considered unsuccessful
        if (playedTime >= 360)
        {
            FinishGame();
        }
    }

    // setting all text fields at the beginning of the condition
    void SetTextFields()
    {
        playedTime = 0;
        lastPosition = transform.position;
        count = 0;
        SetCountText();
        time.text = "";
        winText.text = "";
        distanceText.text = "";
    }

    // activate first Pick Up object
    void ActivatePickups()
    {
        numberOfPickups = pickupHolder.childCount;
        Transform currentChild = pickupHolder.GetChild(currentPickupIndex);
        currentChild.gameObject.SetActive(true);
    }

    // activate first cue
    void ActivateArrows()
    {
        numberOfCues = cueHolder.childCount;
        Transform currentChild = cueHolder.GetChild(currentCueIndex);
        currentChild.gameObject.SetActive(true);

    }


    // this method defines what happens during collisions between CollectorObject and cues or Pick Up objects
    private void OnTriggerEnter(Collider other)
    {
        // when the collector collides with a cue, activate next cue
        if (other.CompareTag("Arrow"))
        {
            DisplayNextCue();
        }
        // when the collector collides with a Pick Up object, active next Pick Up
        else if (other.CompareTag("Pick Up"))
        {
            DisplayNextPickUp();
        }
    }

    // method that specifes which cue to activate next
    private void DisplayNextCue()
    {
        // until the last but one cue
        if (currentCueIndex < numberOfCues - 1)
        {
            // activate the next cue
            cueHolder.GetChild(currentCueIndex + 1).gameObject.SetActive(true);
            // disactivate the current cue
            cueHolder.GetChild(currentCueIndex).gameObject.SetActive(false);
            // increment the index to then access the next cue
            currentCueIndex++;
        }
    }

    // method which specifies which Pick Up object to activate next
    private void DisplayNextPickUp()
    {
        // until the last Pick Up object
        if (currentPickupIndex < numberOfPickups)
        {
            // disactive the current object
            pickupHolder.GetChild(currentPickupIndex).gameObject.SetActive(false);
            // increase the counter
            count = count + 1;
            // set the text to update the counter
            SetCountText();
            // increment the index of the Pick Up objects
            currentPickupIndex++;
            // activate the Pick Up object at the current index
            pickupHolder.GetChild(currentPickupIndex).gameObject.SetActive(true);
        }
    }


    private void SetCountText()
    {
        // this method displays the number of collected objects 
        // so that the experimenter can follow the score easily
        countText.text = "Count: " + count.ToString();
        // once the participant collected all 7 objects, finish the game
        if (count == 7)
        {
            FinishGame();
        }
    }

    // to display results at the end of the maze
    private void FinishGame()
    {
        // for time played
        time.text = "Time played: " + Mathf.RoundToInt(playedTime).ToString();
        // big text to catch the experimenter's attention that the maze is done
        winText.text = "Finished!";
        // for distance walked
        distanceText.text = (distanceTravelled.ToString());
        // sound to inform the participant that the maze is finished
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("ding_sound");
        // to print out the results in the console
        Debug.Log("my final position is" + lastPosition);
        Debug.Log("i walked" + distanceTravelled);
        Debug.Log("time taken " + Mathf.RoundToInt(playedTime).ToString());
    }
}