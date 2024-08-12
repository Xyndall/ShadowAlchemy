using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public bool gameIsPaused;
    public GameObject PauseCanvas;
    public bool isMainMenu = false;
    [SerializeField] private PlayerInput playerInput;
    public PlayerInputActions playerInputActions;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        

        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ControlSchemeIsChanged();
    }

    void ControlSchemeIsChanged()
    {
        //switches the controls shown on the ui, dependant on what inputs the player is using
        if (playerInput.currentControlScheme == "Gamepad")
        {
            
        }
        else if (playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            
        }
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
            PauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }

    }

    public void PauseGame()
    {
        if (!isMainMenu)
        {
            Debug.Log("Pause game");
            gameIsPaused = true;
            PauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }





}
