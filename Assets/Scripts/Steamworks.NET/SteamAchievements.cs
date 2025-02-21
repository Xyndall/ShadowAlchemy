using Steamworks;
using UnityEngine;

public class SteamAchievements : MonoBehaviour
{
    public static void UnlockAchievement(string achievementID)
    {
        if (!SteamAPI.IsSteamRunning()) return;

        bool isUnlocked;
        SteamUserStats.GetAchievement(achievementID, out isUnlocked);

        if (isUnlocked)
        {
            Debug.Log("Achievement already unlocked: " + achievementID);
            return;
        }

        SteamUserStats.SetAchievement(achievementID);
        SteamUserStats.StoreStats();

        Debug.Log("Achievement Unlocked: " + achievementID);
    }

    public static void ResetAchievements()
    {
        if (!SteamAPI.IsSteamRunning()) return;

        SteamUserStats.ResetAllStats(true); // Reset achievements AND stats
        SteamUserStats.StoreStats();

        Debug.Log("All Steam achievements have been reset!");
    }

}
