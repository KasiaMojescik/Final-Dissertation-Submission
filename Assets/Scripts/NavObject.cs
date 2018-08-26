using UnityEngine;
using System.Collections;

public class NavObject : MonoBehaviour
{
    public enum Direction { Left, Right, Straight, Around };

    public bool isArrow;
    public bool hasSound;
    public bool isHaptic;
    public Direction direction;


    void Start()
    {
        if (!isArrow)
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
        
}
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasSound)
            {
                activateSounds();
            }

            if (isHaptic)
            {
                activateVibrations();
            }
        }

    }

    private void activateSounds()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        switch (direction)
        {
            case Direction.Left:
                {
                    audioManager.Play("left");
                    break;
                }
            case Direction.Right:
                {
                    audioManager.Play("right");
                    break;
                }
            case Direction.Straight:
                {
                    audioManager.Play("straight");
                    break;
                }
            case Direction.Around:
                {
                    audioManager.Play("around");
                    break;
                }
        }
    }

    private void activateVibrations()
    {
        CoroutineManager coroutineManager = FindObjectOfType<CoroutineManager>();
        switch (direction)
        {
            case Direction.Left:
                {
                    coroutineManager.StartCoroutine(coroutineManager.LongVibrationLeft(1, 1));
                    break;
                }
            case Direction.Right:

                {
                    coroutineManager.StartCoroutine(coroutineManager.LongVibrationRight(1, 1));
                    break;
                }
            case Direction.Straight:
                {
                    coroutineManager.StartCoroutine(coroutineManager.LongVibration(1, 1, 1, 1));
                    break;
                }
            case Direction.Around:
                {
                    coroutineManager.StartCoroutine(coroutineManager.LongVibration(2, 1, 1, 1));
                    break;
                }
        }
    }
}

