using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{


    [Header("Canvases")]
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject MainMenuPanel;

    [Header("PopUps / animations")]
    [SerializeField] private GameObject OverwriteSavePopUp;

    [Header("First Selected Buttons")]
    public Button OptionsPrimaryButton;
    public Button MainPrimaryButton;
    public Button OSPopUpPrimaryButton;

    [Header("Other stuff")]
    [SerializeField] private GameObject ContinueButton;
    public bool gameIsPaused;
    PlayerInputActions playerInputActions;
    public bool isMainMenu = false;
    public GameObject Player;

    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameIsPaused = true;

        if (PlayerPrefs.HasKey(SaveManager.PlayerX))
        {
            ContinueButton.GetComponent<Button>().interactable = true;

        }
        else
        {
            ContinueButton.GetComponent<Button>().interactable = false;
        }

        MainPrimaryButton.Select();
        OverwriteSavePopUp.SetActive(false);
        OptionsPanel.SetActive(false);
        //if(first time playing continue button is disabled)
    }


    public void StartNewGame()
    {
        if(PlayerPrefs.HasKey(SaveManager.PlayerX))
        {
            OverwriteSavePopUp.SetActive(true);
            OSPopUpPrimaryButton.Select();
        }
        else
        {
            NewGame();
        }
    }

    public void DeleteOldSaveData()
    {
       SaveManager.instance.DeleteSaveKeys();
    }

    public void LoadGame()
    {
        ResumeGame();
        gameIsPaused = false;
    }
    public void NewGame()
    {
        ResumeGame();
        Player.transform.position = GameManager.instance.StartingPos;
    }

    public void SwitchToSettings()
    {
        MainPanel.SetActive(false);
        OptionsPanel.SetActive(true);
        OptionsPrimaryButton.Select();
    }

    public void SwitchToMenu()
    {
        MainPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        OverwriteSavePopUp.SetActive(false);
        MainPrimaryButton.Select();
    }


    private void Pause_performed(InputAction.CallbackContext context)
    {
        if (gameIsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        if (!isMainMenu)
        {
            Debug.Log("Resume Game");
            gameIsPaused = false;
            OptionsPanel.SetActive(false);
            MainMenuPanel.SetActive(false);
            MainPanel.SetActive(false);
            OverwriteSavePopUp.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

           Time.timeScale = 1;
        }

    }

    public void PauseGame()
    {
        if (!isMainMenu)
        {
            Debug.Log("show Pause game");
            gameIsPaused = true;
            MainMenuPanel.SetActive(true);
            MainPanel.SetActive(true);
            OptionsPanel.SetActive(false);
            OverwriteSavePopUp.SetActive(false);
            MainPrimaryButton.Select();
            SaveManager.instance.SaveData();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;


           // Time.timeScale = 0;
        }
    }

    public void QuitGame()
    {
        SaveManager.instance.SaveData();
        StartCoroutine(WaitForQuit());
    }

    IEnumerator WaitForQuit()
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }

}
