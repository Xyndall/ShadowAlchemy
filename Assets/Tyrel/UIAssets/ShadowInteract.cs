using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShadowInteract : MonoBehaviour
{

    public PlayerInputActions playerInputActions;
    public GameObject radialMenu;
    bool isRadialMenuOpen;

    public Animator animator;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        radialMenu.SetActive(false);
        isRadialMenuOpen = false;
    }


    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Interact.performed += Interact;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.Interact.performed += Interact;
    }


    void Interact(InputAction.CallbackContext context)
    {
        if (GameManager.instance.gameIsPaused == false )// && also check if over the shadow object
        {
            

            if (isRadialMenuOpen && context.performed)
            {
                CloseRadialMenu();
            }
            else
            {
                OpenRadialMenu();
            }

        }
    }

    void OpenRadialMenu()
    {
        radialMenu.SetActive(true);
        isRadialMenuOpen = true;
    }
    void CloseRadialMenu()
    {
        animator.SetTrigger("CloseRadialMenu");
        isRadialMenuOpen = false;
    }

    


}
