using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }
    public TextMeshProUGUI timerText;
    public float elapsedTime { get; private set; }
    private bool isRunning = false;

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
        elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
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
        PlayerPrefs.SetFloat("ElapsedTime", 0f);
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

    private void SaveTime()
    {
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.Save();
    }

    private void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{hours}h:{minutes}m:{seconds}s";
    }

}
