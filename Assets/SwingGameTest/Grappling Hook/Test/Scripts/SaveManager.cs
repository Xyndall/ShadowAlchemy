using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public class SaveManager : MonoBehaviour
{
    [Header("Data Names")]
    public const string playerFallCount = "PlayerFallCount";
    public const string PlayerX = "PlayerX";
    public const string PlayerY = "PlayerY";
    public const string PlayerZ = "PlayerZ";
    public const string CaveLevel = "CaveLevel";
    public const string ForestLevel = "ForestLevel";
    public const string CastleLevel = "CastleLevel";
    public const string PlayersLevel = "PlayersLevel";
    public const string EasyModeOption = "EasyMode";
    public const string DoorSmashedOpen = "DoorSmashedOpen";
    public const string FallCount = "FallCount";
    public const string GameTimer = "ElapsedTime";

    public static SaveManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void DeleteSaveKeys()
    {
        PlayerPrefs.DeleteKey(PlayerX);
        PlayerPrefs.DeleteKey(PlayerY);
        PlayerPrefs.DeleteKey(PlayerZ);
        PlayerPrefs.DeleteKey(PlayersLevel);
        PlayerPrefs.DeleteKey(DoorSmashedOpen);
        PlayerPrefs.DeleteKey(FallCount);
        PlayerPrefs.DeleteKey(GameTimer);
        
        // Delete all BreakableObject keys
        foreach (BreakableObject breakable in FindObjectsOfType<BreakableObject>())
        {
            string key = $"BreakableObject_{breakable.UniqueID}";
            
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, 0);
                PlayerPrefs.DeleteKey(key);
            }
        }

    }

    public void SaveData()
    {
        PlayerPrefs.Save();
    }

    public void SaveIntData(string paramName, int value)
    {
        PlayerPrefs.SetInt(paramName, value);
    }

    public void SaveFloatData(string paramName, float value)
    {
        PlayerPrefs.SetFloat(paramName, value);
    }

    public void SaveStringData(string paramName, string value)
    {
        PlayerPrefs.SetString(paramName, value);
        Debug.Log("saving: " + paramName + " value: " + value);
    }
}
