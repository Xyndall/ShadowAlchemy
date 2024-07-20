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
            //if (context.performed)
            //{
            //    OpenRadialMenu();
            //}

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
        Debug.Log("Opening Radial Menu");
        radialMenu.SetActive(true);
        animator.SetTrigger("Open");
        isRadialMenuOpen = true;
    }
    void CloseRadialMenu()
    {
        Debug.Log("Closing Radial Menu");
        animator.SetTrigger("Close");
        isRadialMenuOpen = false;
        //StartCoroutine(DeactivateAfterAnimation());
    }

    private IEnumerator DeactivateAfterAnimation()
    {
        // Wait for the length of the closing animation
        yield return new WaitForSeconds(.2f);
        radialMenu.SetActive(false);
    }
    


}
