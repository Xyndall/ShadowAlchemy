using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPositionManager : MonoBehaviour
{
    public static PlayerPositionManager Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        // Ensure only one instance of the PlayerPositionManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

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
        PlayerPrefs.SetFloat(SaveManager.PlayerX, playerPosition.x);
        PlayerPrefs.SetFloat(SaveManager.PlayerY, playerPosition.y);
        PlayerPrefs.SetFloat(SaveManager.PlayerZ, playerPosition.z); // If you're in 2D, you might not need Z

        PlayerPrefs.Save(); // Ensure data is written to disk
    }



    public void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey(SaveManager.PlayerX))
        {
            float x = PlayerPrefs.GetFloat(SaveManager.PlayerX);
            float y = PlayerPrefs.GetFloat(SaveManager.PlayerY);
            float z = PlayerPrefs.GetFloat(SaveManager.PlayerZ);

            Vector3 targetPosition = new Vector3(x, y, z);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();

            if (rb != null)
                rb.position = targetPosition;
            else
                transform.position = targetPosition;

            if (playerCollider != null)
            {
                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(LayerMask.GetMask("Ground", "UnGrappleable", "Grappleable"));
                filter.useTriggers = false;

                Collider2D[] results = new Collider2D[5];
                int maxAttempts = 10;
                bool stuck = false;

                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    int count = playerCollider.OverlapCollider(filter, results);
                    bool moved = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (results[i] == null) continue;
                        ColliderDistance2D dist = playerCollider.Distance(results[i]);
                        if (dist.isOverlapped)
                        {
                            Vector3 move = (Vector3)(dist.pointA - dist.pointB);
                            if (rb != null)
                                rb.position += (Vector2)move;
                            else
                                transform.position += move;
                            moved = true;
                        }
                    }
                    if (!moved)
                    {
                        stuck = (playerCollider.OverlapCollider(filter, results) > 0);
                        break;
                    }
                }

                // Fallback: move up until not stuck
                if (stuck)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Vector3 upMove = new Vector3(0, 0.1f, 0);
                        if (rb != null)
                            rb.position += (Vector2)upMove;
                        else
                            transform.position += upMove;

                        if (playerCollider.OverlapCollider(filter, results) == 0)
                            break;
                    }
                }
            }
        }
    }


}
