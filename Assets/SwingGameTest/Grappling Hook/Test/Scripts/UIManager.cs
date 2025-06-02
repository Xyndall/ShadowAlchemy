using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{


    [Header("Canvases")]
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject ControlsPanel;
    [SerializeField] private GameObject HudCanvas;
    [SerializeField] private GameObject QuitCanvas;

    [Header("PopUps / animations")]
    [SerializeField] private GameObject OverwriteSavePopUp;

    [Header("First Selected Buttons")]
    public Button MainPrimaryButton;
    public Button OSPopUpPrimaryButton;

    [Header("Other stuff")]
    [SerializeField] private GameObject ContinueButton;
    public bool gameIsPaused;
    PlayerInputActions playerInputActions;
    public bool isMainMenu = false;
    public GameObject Player;
    public GameObject Credits;
    public bool EasyMode = false;
    public Toggle _toggle;
    public Button modifierButton;
    

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
        if(PlayerPrefs.GetInt(SaveManager.TotalGameCompletions, 0) > 0)
        {
            modifierButton.interactable = true;
            
        }
        else
        {
            modifierButton.interactable = false;
        }

            gameIsPaused = true;
        QuitCanvas.SetActive(false);
        PauseGame();
        int EasyModeOptionInt = (PlayerPrefs.GetInt(SaveManager.EasyModeOption, 0));
        if (EasyModeOptionInt == 0)
            EasyModeOn(false);
        else if (EasyModeOptionInt == 1)
            EasyModeOn(true);
        if (PlayerPrefs.HasKey(SaveManager.PlayerX))
        {
            ContinueButton.GetComponent<Button>().interactable = true;
            
        }
        else
        {
            ContinueButton.GetComponent<Button>().interactable = false;
        }
        //if(first time playing continue button is disabled)

        

    }

    public void CheckPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(SaveManager.PlayerX))
        {
            ContinueButton.GetComponent<Button>().interactable = true;
            
        }
        
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
        GameTimer.Instance.ResetTimer();
        SaveManager.instance.DeleteSaveKeys();
       
    }

    public void LoadGame()
    {
        ResumeGame();
    }

    public void NewGame()
    {
        DeleteOldSaveData();
        Player.transform.position = GameManager.instance.StartingPos;
        GameManager.instance.StartCutscene();
        Debug.Log("New Game");
    }

    public void SelectButton(Button button)
    {
        button.Select();
    }
    public void SelectToggle(Toggle toggle)
    {
        toggle.Select();
    }
    public void SwitchToSettings()
    {
        OptionsPanel.SetActive(true);
        MainPanel.SetActive(false);
        ControlsPanel.SetActive(false);

    }

    public void SwitchToMenu()
    {
        MainPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        OverwriteSavePopUp.SetActive(false);
        Credits.SetActive(false);

    }

    public void OpenCredits()
    {
        Credits.SetActive(!Credits.activeSelf);
    }
    
    public void DisableInputs()
    {
        playerInputActions.Player.Disable();
    }
    public void EnableInputs()
    {
        playerInputActions.Player.Enable();
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

    public void EasyModeOn(bool on)
    {
        _toggle.isOn = on;
        EasyMode = on;
        NewGrappleTest.instance.EasyModeGrapple = on;
        if(on) SaveManager.instance.SaveIntData(SaveManager.EasyModeOption, 1);
        else if (!on)SaveManager.instance.SaveIntData(SaveManager.EasyModeOption, 0);
    }

    public void CutsceneFinished()
    {
        //set game stuff
        GameManager.instance.CutsceneFinished();

        //now load ui stuff
        int EasyModeOptionInt = (PlayerPrefs.GetInt(SaveManager.EasyModeOption, 0));
        if (EasyModeOptionInt == 0)
            EasyModeOn(false);
        else if (EasyModeOptionInt == 1)
            EasyModeOn(true);


        ResumeGame();
    }

    public void ResumeGame()
    {
        if (!isMainMenu)
        {
            CheckPlayerPrefs();
            gameIsPaused = false;
            GameTimer.Instance.StartTimer();
            AudioManager.Instance.StartGameMusic();
            OptionsPanel.SetActive(false);
            MainMenuPanel.SetActive(false);
            MainPanel.SetActive(false);
            ControlsPanel.SetActive(false);
            Credits.SetActive(false);
            OverwriteSavePopUp.SetActive(false);
            if (!EasyMode)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
           Time.timeScale = 1;
        }

    }

    public void PauseGame()
    {
        if (!isMainMenu)
        {
            if (PlayerPrefs.GetInt(SaveManager.TotalGameCompletions, 0) > 0)
            {
                modifierButton.interactable = true;

            }

            
            gameIsPaused = true;
            GameTimer.Instance.StopTimer();
            AudioManager.Instance.StartTitleMusic();
            MainMenuPanel.SetActive(true);
            MainPanel.SetActive(true);
            ControlsPanel.SetActive(false);
            OptionsPanel.SetActive(false);
            OverwriteSavePopUp.SetActive(false);
            Credits.SetActive(false);
            MainPrimaryButton.Select();
            SaveManager.instance.SaveData();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;


           // Time.timeScale = 0;
        }
    }

    public void QuitGame()
    {
        QuitCanvas.SetActive(true);
        SaveManager.instance.SaveData();
        StartCoroutine(WaitForQuit());
    }

    IEnumerator WaitForQuit()
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }

}
