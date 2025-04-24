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


    void Start()
    {
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

}
