using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Vector3 StartingPos;
    public PlayableDirector playableDirector;
    public GameObject player;
    public GameObject vcam;
    public GameObject Mcamera;
    public GameObject EndCanvas;

    void Start()
    {
        EndCanvas.SetActive(false);

        // Check if the data exists
        if (PlayerPrefs.HasKey(SaveManager.PlayerX))
        {
            player.SetActive(true);
            vcam.SetActive(true);
            Mcamera.SetActive(true);
        }
        else
        {
            playableDirector.Play();
        }
    }

    public void EndGame()
    {

        if (NewGrappleTest.instance != null)
        {
            NewGrappleTest.instance.enabled = false;
        }
        else
        {
            Debug.LogWarning("NewGrappleTest script not found!");
        }


        if (UIManager.instance != null)
        {
            UIManager.instance.DisableInputs();
        }
        else
        {
            Debug.LogWarning("UIManager not found!");
        }


        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllMusic();
        }
        else
        {
            Debug.LogWarning("AudioManager not found!");
        }


        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.StopTimer();
        }
        else
        {
            Debug.LogWarning("GameTimer not found!");
        }

        EndCanvas.SetActive(true);

    }

}
