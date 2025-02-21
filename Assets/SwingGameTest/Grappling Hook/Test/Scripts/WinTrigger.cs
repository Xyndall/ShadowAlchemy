using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player (or assign a specific tag to the player)
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnGameWin();
        }
    }

    void OnGameWin()
    {
        Debug.Log("Game Win Goal Reached hayaasdasasa");
        SteamAchievements.UnlockAchievement("Ach_EndGoal");
    }

}
