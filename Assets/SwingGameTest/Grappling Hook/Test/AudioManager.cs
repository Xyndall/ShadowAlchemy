using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("UI Sounds")]
    public AudioClip clickSound;
    public AudioSource uiAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional, keeps AudioManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayUIClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }
}
