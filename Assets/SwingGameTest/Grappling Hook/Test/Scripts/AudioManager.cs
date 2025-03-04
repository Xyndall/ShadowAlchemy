using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("UI Sounds")]
    public AudioClip clickSound;
    public AudioSource uiAudioSource;

    [Header("Music")]
    [SerializeField] private AudioSource titleMusic;
    [SerializeField] private AudioSource gameMusic;
    [SerializeField] private AudioClip[] gameMusicClips;
    private bool titleMusicPlaying = false;
    private bool gameMusicPlaying = false;
    public bool caveLevel;
    public bool ForestLevel;
    public bool CastleLevel;

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
    private void Start()
    {
        StartTitleMusic();
    }

    public void PlayUIClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }

    public void StartTitleMusic()
    {
        if (!titleMusicPlaying)
        {
            titleMusic.Play();
            if (gameMusicPlaying)
            {
                gameMusic.Pause();
            }
            else
            {
                gameMusic.Stop();
            }
            gameMusicPlaying = false;
            titleMusicPlaying = true;
        }
    }


    public void StartGameMusic()
    {
            string currentPlayerLevel = PlayerPrefs.GetString(SaveManager.PlayersLevel, "CaveLevel");
            switch (currentPlayerLevel)
            {
                case "CaveLevel":
                    gameMusic.clip = gameMusicClips[0];
                    break;

                case "ForestLevel":
                    gameMusic.clip = gameMusicClips[1];
                    break;

                case "CastleLevel":
                    gameMusic.clip = gameMusicClips[2];
                    break;
            }
            gameMusic.Play();
            titleMusic.Stop();
            titleMusicPlaying = false;
            gameMusicPlaying = true;
    }
}
