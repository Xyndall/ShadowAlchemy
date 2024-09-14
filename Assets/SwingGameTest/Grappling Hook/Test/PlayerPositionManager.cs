using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{

    private void Start()
    {
        LoadPlayerPosition();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Player has exited the game.");
        SavePlayerPosition();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Debug.Log("Application lost focus (Alt+Tab or minimized).");
            SavePlayerPosition();
        }
    }

    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            Debug.Log("Application paused (Alt+Tab or minimized).");
            SavePlayerPosition();
        }
    }

    public void SavePlayerPosition()
    {
        Vector3 playerPosition = transform.position;

        // Saving player's position
        PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerZ", playerPosition.z); // If you're in 2D, you might not need Z

        PlayerPrefs.Save(); // Ensure data is written to disk
    }

   

    public void LoadPlayerPosition()
    {
        // Check if the data exists
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ"); // Optional for 2D

            // Set the player's position
            transform.position = new Vector3(x, y, z);
        }
    }

}
