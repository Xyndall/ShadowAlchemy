using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public bool CaveLevel = false;
    public bool ForestLevel = false;
    public bool CastleLevel = false;
    [SerializeField] private string LevelName = SaveManager.CaveLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player (or assign a specific tag to the player)
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //OnGameWin();
            ChangeLevel();
        }
    }

    private void Start()
    {
        if (CaveLevel) 
        {
        LevelName = SaveManager.CaveLevel;
        }
        if(CastleLevel)
        {
            LevelName = SaveManager.CastleLevel;
        }
        if(ForestLevel)
        {
            LevelName = SaveManager.ForestLevel;
        }
    }
    void ChangeLevel()
    {
        SaveManager.instance.SaveStringData(SaveManager.PlayersLevel, LevelName);
        AudioManager.Instance.StartGameMusic();
    }

    void OnGameWin()
    {
        Debug.Log("Game Win Goal Reached hayaasdasasa");
        SteamAchievements.UnlockAchievement("Ach_EndGoal");
    }

}
