using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShadowInteract : MonoBehaviour
{

    public PlayerInputActions playerInputActions;
    public GameObject radialMenu;
    public Button Top, Left, Right;
    bool isRadialMenuOpen;

    public Animator animator;
    public GameObject TransformEffect;

    public GameObject Player;
    public GameObject ShadowCube;
    public GameObject ShadowLadder;
    public GameObject ShadowDistractor;
   [HideInInspector] public GameObject shadowObject; // reference to the object to transmute

    Camera mainCamera; // Reference to the main camera
    public Canvas canvas; // Reference to the UI canvas

    public EventSystem evenSystem;
    public GameObject firstSelectedButton;

    private void Awake()
    {
        radialMenu.SetActive(false);
        isRadialMenuOpen = false;
        TurnOffButtons();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact;


    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(shadowObject == null && isRadialMenuOpen)
        {
            CloseRadialMenu();
        }
    }

    #region Transmutations

    public void ChangeToSquare()
    {
        Debug.Log("ChangingToSquare");
        Instantiate(TransformEffect, shadowObject.transform.position, Quaternion.identity);
        Instantiate(ShadowCube, shadowObject.transform.position, Quaternion.identity);
        Destroy(shadowObject);
    }
    public void ChangeToDistractor()
    {
        Debug.Log("ChangingToDistractor");
        Instantiate(TransformEffect, shadowObject.transform.position, Quaternion.identity);
        Instantiate(ShadowDistractor, shadowObject.transform.position, Quaternion.identity);
        Destroy(shadowObject);
    }
    public void ChangeToLadder()
    {
        Debug.Log("ChangingToLadder");
        Instantiate(TransformEffect, shadowObject.transform.position, Quaternion.identity);
        Instantiate(ShadowLadder, shadowObject.transform.position, Quaternion.identity);
        Destroy(shadowObject);
    }  
    #endregion



    void Interact(InputAction.CallbackContext context)
    {
        if (GameManager.instance.gameIsPaused == false && !isRadialMenuOpen && shadowObject != null)
        {
            OpenRadialMenu();
            
        }
    }

    void OpenRadialMenu()
    {
        evenSystem.firstSelectedGameObject = firstSelectedButton;

        Debug.Log("Opening Radial Menu");
        // Step 1: Convert world position to screen position
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(Player.transform.position);

        // Step 2: Convert screen position to canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
            );
        // Step 3: Move radial menu to canvas/players position
        radialMenu.GetComponent<RectTransform>().anchoredPosition = canvasPosition;

        radialMenu.SetActive(true);
        animator.SetTrigger("Open");
        isRadialMenuOpen = true;


        // Need to stop players movement here



        //
    }

    public void CloseRadialMenu()
    {
        TurnOffButtons();
        Debug.Log("Closing Radial Menu");
        animator.SetTrigger("Close");
        isRadialMenuOpen = false;
        

        // Need to let players move here



        //
    }



    public void TurnOnButtons()
    {
        Top.enabled = true; Left.enabled = true; Right.enabled = true;

    }
    public void TurnOffButtons()
    {
        Top.enabled = false; Left.enabled = false; Right.enabled = false;

    }


}
