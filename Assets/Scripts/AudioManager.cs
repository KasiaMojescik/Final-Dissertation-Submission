using UnityEngine.Audio;
using System;
using UnityEngine;

// created following tutorial https://www.youtube.com/watch?v=6OT43pvUyfY
// this class provides functionality for the sounds to be played in the experiment
public class AudioManager : MonoBehaviour
{
    // a static reference to the current AudioManager
	public static AudioManager instance;
    // get an array of sounds
	public Sound[] sounds;
    // when the script instance is loaded..
	void Awake()
	{
        // if there is an instane of AudioManager already, destroy it
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

		foreach (Sound s in sounds)
		{
            // at the start of each trial, the AudioSource will be added to the AudioManager object in the inspector

            // add the AudioSource to each sound in the array 
			s.source = gameObject.AddComponent<AudioSource>();
            // clip of the AudioSource (clip is the actual recording)
			s.source.clip = s.clip;
		}
	}

    // this method activates the sound with the correct name
	public void Play(string sound)
	{
        // find the sound with the correct name in the sounds array
		Sound s = Array.Find(sounds, item => item.name == sound);
        // if there is no sound with such name, throw a warning
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

		s.source.Play();
	}

}
