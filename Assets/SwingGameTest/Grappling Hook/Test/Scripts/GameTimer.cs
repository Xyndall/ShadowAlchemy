using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerTextEnd;
    public float elapsedTime { get; private set; }
    private bool isRunning = false;
    public bool OverTenMins = false;

    [Header("Sprite Change Settings")]
    [SerializeField] private GameObject[] Crowns; // The UI Image or SpriteRenderer to update
    [SerializeField] private float[] timeThresholds; // Time thresholds for sprite changes

    private int currentGameObjectIndex = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: keep it across scenes
    }

    private void Start()
    {
        // Load saved time
        elapsedTime = PlayerPrefs.GetFloat(SaveManager.GameTimer, 0f);
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
            
        }

    }
    private void FixedUpdate()
    {
        CheckAndUpdateGameObject();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
        SaveTime();
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        PlayerPrefs.SetFloat(SaveManager.GameTimer, 0f);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveTime();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveTime();
    }

    public void SaveTime()
    {
        PlayerPrefs.SetFloat(SaveManager.GameTimer, elapsedTime);
        PlayerPrefs.Save();
    }

    private void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // Format the time as 00:00:00
        string formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";

        // Update the timer text
        timerText.text = formattedTime;
        timerTextEnd.text = formattedTime;
    }
    private void CheckAndUpdateGameObject()
    {
        // Ensure we have valid thresholds and GameObjects
        if (timeThresholds.Length == 0 || Crowns.Length == 0)
            return;

        // Check if the elapsed time has reached the next threshold
        if (currentGameObjectIndex < timeThresholds.Length && elapsedTime >= timeThresholds[currentGameObjectIndex])
        {
            currentGameObjectIndex++;
            UpdateGameObject();
            OverTenMins = true;
        }
    }

    private void UpdateGameObject()
    {
        // Deactivate all GameObjects
        foreach (GameObject obj in Crowns)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Activate the current GameObject if the index is valid
        if (currentGameObjectIndex - 1 >= 0 && currentGameObjectIndex - 1 < Crowns.Length)
        {
            GameObject currentObject = Crowns[currentGameObjectIndex - 1];
            if (currentObject != null)
                currentObject.SetActive(true);
        }
    }
}
