using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public FrontCheck frontCh;
    public GroundCheck groundCh;
    public PlayerInputActions playerInputActions;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] float gravityBase;
    [SerializeField] float gravityIncrease;
    [SerializeField] float velocity;

    
    Vector3 movedirection;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }


    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Move.performed += ReadValuePerformed;
        playerInputActions.Player.Move.canceled += ReadValueCanceled;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.Move.performed -= ReadValuePerformed;
        playerInputActions.Player.Move.canceled -= ReadValueCanceled;
    }
    private void FixedUpdate()
    {

        Movement();

    }
    void Update()
    {
        velocity += gravity * Time.deltaTime;
        if (groundCh.groundCheck && velocity < 0)
        {
            velocity = 0;
            gravity = gravityBase;
        }
        if (Input.GetKey(KeyCode.Space) && groundCh.groundCheck)
        {
            velocity = jumpForce;

        }
        if (!groundCh.groundCheck)
        {
            gravity -= gravityIncrease * Time.deltaTime;
        }
        transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);


    }
    void ReadValuePerformed(InputAction.CallbackContext value) 
    {
        movedirection = value.ReadValue<Vector2>();
    }
    void ReadValueCanceled(InputAction.CallbackContext value)
    {
        movedirection = Vector2.zero;
    }
    void Movement( )
    {
        if(frontCh.touchingWall)
        {
            movedirection = Vector2.zero;
        }

        Vector3 movePlayer = new Vector3(movedirection.x,0,0) *speed*Time.deltaTime;
        transform.Translate(movePlayer);
    }
}
