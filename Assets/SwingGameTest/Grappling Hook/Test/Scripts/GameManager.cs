using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [Header("Game Settings")]
    public Vector3 StartingPos;
    public PlayableDirector playableDirector;
    public GameObject player;
    public GameObject vcam;
    public GameObject Mcamera;
    public GameObject EndCanvas;
    public GameObject CutscenePlayer;
    public bool CutscenePlaying;

    [Header("UI Elements")]
    public TextMeshProUGUI FallText;
    public Image EndGameImage; // Reference to the UI Image
    public Sprite GoldCrownSprite; // Sprite for the best time
    public Sprite SilverCrownSprite; // Sprite for a good time

    void Start()
    {
        EndCanvas.SetActive(false);

        // Check if the data exists
        if (PlayerPrefs.HasKey(SaveManager.PlayerX))
        {
            player.SetActive(true);
            vcam.SetActive(true);
            Mcamera.SetActive(true);
            CutscenePlayer.SetActive(false);
        }
        else
        {
            StartCutscene();
        }
    }

    public void StartCutscene()
    {
        playableDirector.Play();
        CutscenePlaying = true;
    }

    public void CutsceneFinished()
    {
        //set game stuff
        CutscenePlaying = false;
        playableDirector.Stop();
        player.SetActive(true);
        vcam.SetActive(true);
        Mcamera.SetActive(true);
        CutscenePlayer.SetActive(false);
        player.transform.position = StartingPos;
        GameTimer.Instance.ResetTimer();
        

    }

    public void EndGame()
    {

        if (NewGrappleTest.instance != null)
        {
            NewGrappleTest.instance.enabled = false;
        }
        else
        {
            Debug.LogWarning("NewGrappleTest script not found!");
        }


        if (UIManager.instance != null)
        {
            UIManager.instance.DisableInputs();
        }
        else
        {
            Debug.LogWarning("UIManager not found!");
        }


        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllMusic();
        }
        else
        {
            Debug.LogWarning("AudioManager not found!");
        }


        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.StopTimer();
            UpdateEndGameImage(GameTimer.Instance.elapsedTime); // Update the UI image based on the time
        }
    
        else
        {
            Debug.LogWarning("GameTimer not found!");
        }

        EndCanvas.SetActive(true);
        FallText.text = PlayerPrefs.GetInt(SaveManager.FallCount, 0).ToString();
    }


    private void UpdateEndGameImage(float elapsedTime)
    {
        // Check the time thresholds and update the sprite
        if (elapsedTime <= 600) // Gold crown for times less than or equal to 600 seconds
        {
            EndGameImage.sprite = GoldCrownSprite;
        }
        else // Silver crown for times greater than 600 seconds
        {
            EndGameImage.sprite = SilverCrownSprite;
        }
    }

}
