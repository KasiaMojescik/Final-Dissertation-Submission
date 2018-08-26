using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // For scene 1
    public void PlayArrow()
    {
        SceneManager.LoadScene("Just Arrow");
    }

    
    public void PlayVoice()
    {
        SceneManager.LoadScene("Just Sound");
    }

    public void PlayHaptic()
    {
        SceneManager.LoadScene("Just Haptic");
    }

    public void PlayArrowVoice()
    {
        SceneManager.LoadScene("Arrow and Voice");
    }

    public void PlayArrowHaptic()
    {
        SceneManager.LoadScene("Arrow and Haptic");
    }
    
    public void PlaySingleArrow()
    {
        SceneManager.LoadScene("Single Arrow");
    }

    public void PlayLine()
    {
        SceneManager.LoadScene("Line");
    }
}
