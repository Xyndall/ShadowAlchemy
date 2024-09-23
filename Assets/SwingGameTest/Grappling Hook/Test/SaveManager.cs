using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Header("Data Names")]
    public const string playerFallCount = "PlayerFallCount";
    public const string PlayerX = "PlayerX";
    public const string PlayerY = "PlayerY";
    public const string PlayerZ = "PlayerZ";

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
