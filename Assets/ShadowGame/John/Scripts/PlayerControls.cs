using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerControls : MonoBehaviour
{
    FrontCheck frontCh;
    GroundCheck groundCh;
    PlayerInputActions playerInputActions;
    Climb ladderCheck;
    Animator PlayerAnimator;
    SpriteRenderer SpriteRenderer;

    [SerializeField] float speed;
    [SerializeField] float speedBase;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] float gravityBase;
    [SerializeField] float gravityIncrease;
    [SerializeField] float velocity;
    [SerializeField] float floorHeight = .9f;

    private string currentState;

    const string player_Idle = "Idle";
    const string player_Run = "Run";
    const string player_Jump = "Jump";
    const string player_Climb = "Climb";

    bool climbing;
    bool running;
    bool jumping;
    Vector3 MoveX;

    public LayerMask ladderPosition;
    private void Awake()
    {
        if(playerInputActions != null) return;
        playerInputActions = new PlayerInputActions();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        frontCh = GetComponent<FrontCheck>();
        groundCh = GetComponent<GroundCheck>();
        ladderCheck = GetComponent<Climb>();
        PlayerAnimator = GetComponent<Animator>();
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
        Climb();
    }
    void Update()
    {
        if (!ladderCheck.canClimb)
        {
            Jump();
        }
        

        if (!groundCh.groundCheck)
        {
            if (jumping)
            {

                ChangeAnimationState(player_Jump);
            }
        }
        else
        {
            if(MoveX.x != 0)
            {
                ChangeAnimationState(player_Run);
            }
            else
            {
                ChangeAnimationState(player_Idle);
            }
        }
        
    }
    void ReadValuePerformed(InputAction.CallbackContext value) 
    {
        MoveX = value.ReadValue<Vector2>();
        if (MoveX.x <= 0) { SpriteRenderer.flipX = true; }
        if (MoveX.x >= 0) { SpriteRenderer.flipX = false; }
    }
    void ReadValueCanceled(InputAction.CallbackContext value)
    {
        MoveX = Vector2.zero;
    }
    void Movement( )
    {
        if(frontCh.touchingWall)
        {
            MoveX = Vector2.zero;
        }
        Vector3 movePlayer = new Vector3(MoveX.x, 0, 0) * speed * Time.deltaTime;
        transform.Translate(movePlayer);
        running = true;
    }
    void Jump()
    {
        jumping = true;
        velocity += gravity * gravityIncrease * Time.deltaTime;
        if (groundCh.groundCheck && velocity < 0)
        {
            gravity = gravityBase;
            velocity = 0;
            speed = speedBase;
            transform.position = new Vector3(transform.position.x, groundCh.surfacePosition.y + floorHeight, transform.position.z);
        }
        if (playerInputActions.Player.Jump.triggered && groundCh.groundCheck || playerInputActions.Player.Jump.triggered && groundCh.jumpGrace > 0)
        {
            velocity = jumpForce;
            speed -= speed * Time.deltaTime;
        }
        transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);
    }
    void Climb()
    {
        if (ladderCheck.canClimb)
        {
            Vector3 movePlayer = new Vector3(0, MoveX.y, 0) * speed * Time.deltaTime;
            transform.Translate(movePlayer);
            if (movePlayer.y != 0)
            {
                climbing = true;
            }
            if (climbing)
            {
                ChangeAnimationState(player_Climb);
            }
        }
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        PlayerAnimator.Play(newState);
        currentState = newState;
    }
}
