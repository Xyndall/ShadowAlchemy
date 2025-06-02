using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using System.Collections;


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
    public GameObject FinalCreditsCanvas;
    public GameObject CutscenePlayer;
    public GameObject ControlsWorldCanvas;
    public bool CutscenePlaying;

    [Header("UI Elements")]
    public Button newGameButton;
    public TextMeshProUGUI FallText;
    public Image EndGameImage; // Reference to the UI Image
    public Sprite GoldCrownSprite; // Sprite for the best time
    public Sprite SilverCrownSprite; // Sprite for a good time

    [Header("Other Stuff")]
    public GameObject Gate;

    void Start()
    {
        EndCanvas.SetActive(false);
        FinalCreditsCanvas.SetActive(false);

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
        Gate.SetActive(true);
        ControlsWorldCanvas.SetActive(false);
        UIManager.instance.DeleteOldSaveData();
        EndCanvas.SetActive(false);
        playableDirector.Play();
        CutscenePlaying = true;
    }

    public void CutsceneFinished()
    {
        //set game stuff
        ControlsWorldCanvas.SetActive(true);
        CutscenePlaying = false;
        playableDirector.Stop();
        player.SetActive(true);
        vcam.SetActive(true);
        Mcamera.SetActive(true);
        CutscenePlayer.SetActive(false);
        player.transform.position = StartingPos;
        GameTimer.Instance.ResetTimer();
        PlayerPositionManager.Instance.SavePlayerPosition();
        NewGrappleTest.instance.enabled = true;
        UIManager.instance.EnableInputs();
        AudioManager.Instance.StartGameMusic();

    }

    public void EndGame()
    {
        NewGrappleTest.instance.enabled = false;
        UIManager.instance.DisableInputs();
        AudioManager.Instance.StopAllMusic();
        GameTimer.Instance.StopTimer();
        UpdateEndGameImage(GameTimer.Instance.elapsedTime); // Update the UI image based on the time

        if (PlayerPrefs.GetInt(SaveManager.TotalGameCompletions, 0) == 0)
        {
            FinalCreditsCanvas.SetActive(true);
            StartCoroutine(WaitForCredits());
        }
        

        EndCanvas.SetActive(true);
        FallText.text = PlayerPrefs.GetInt(SaveManager.FallCount, 0).ToString();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UIManager.instance.SelectButton(newGameButton);
        PlayerPrefs.SetInt(SaveManager.TotalGameCompletions, PlayerPrefs.GetInt(SaveManager.TotalGameCompletions, 0) + 1);
    }

    IEnumerator WaitForCredits()
    {
        yield return new WaitForSeconds(5f);
        FinalCreditsCanvas.SetActive(false);
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
