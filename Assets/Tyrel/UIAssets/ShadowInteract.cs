using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShadowInteract : MonoBehaviour
{

    public PlayerInputActions playerInputActions;
    public GameObject radialMenu;
    public Button Top, Left, Right;
    bool isRadialMenuOpen;

    public Animator animator;
    public Animator effectAnimator;

    public Transform SpawnPoint;
    public GameObject TransformEffect;

    public GameObject ShadowCube;
    public GameObject ShadowLadder;
    public GameObject ShadowDistractor;
    public GameObject shadowObject;

    private void Awake()
    {
        radialMenu.SetActive(false);
        isRadialMenuOpen = false;
        Top.enabled = false; Left.enabled = false; Right.enabled = false;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact;


    }



    public void ChangeToSquare()
    {
        Debug.Log("ChangingToSquare");
        Instantiate(TransformEffect, SpawnPoint.position, Quaternion.identity);
        Destroy(shadowObject);
        Instantiate(ShadowCube, SpawnPoint.position, Quaternion.identity);
    } 
    public void ChangeToDistractor()
    {
        Debug.Log("ChangingToDistractor");
        Instantiate(TransformEffect, SpawnPoint.position, Quaternion.identity);
        Destroy(shadowObject);
        Instantiate(ShadowDistractor, SpawnPoint.position, Quaternion.identity);
    } 
    public void ChangeToLadder()
    {
        Debug.Log("ChangingToLadder");
        Instantiate(TransformEffect, SpawnPoint.position, Quaternion.identity);
        Destroy(shadowObject);
        Instantiate(ShadowLadder, SpawnPoint.position, Quaternion.identity);
    } 



    void Interact(InputAction.CallbackContext context)
    {
        if (GameManager.instance.gameIsPaused == false && !isRadialMenuOpen)// && also check if over the shadow object
        {
            OpenRadialMenu();

            //if (isRadialMenuOpen && context.performed)
            //{
            //    CloseRadialMenu();
            //}
            //else
            //{
            //    OpenRadialMenu();
            //}

        }
    }

    void OpenRadialMenu()
    {
        Debug.Log("Opening Radial Menu");
        radialMenu.SetActive(true);
        animator.SetTrigger("Open");
        isRadialMenuOpen = true;
    }

    public void TurnOnButtons()
    {
        Top.enabled = true;Left.enabled = true; Right.enabled = true;
        
    }

    public void CloseRadialMenu()
    {
        Debug.Log("Closing Radial Menu");
        animator.SetTrigger("Close");
        isRadialMenuOpen = false;
        Top.enabled = false; Left.enabled = false; Right.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShadowObject"))
        {
            Debug.Log("Shadow object is in trigger");
            shadowObject = collision.gameObject;
        }
    }


}
