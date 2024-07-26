using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushObject : MonoBehaviour
{
    [SerializeField] LayerMask Player; 
    [SerializeField] LayerMask Wall;
    [SerializeField] float speed;

    float Reduce = 0.5f;
    float leftOffset = 1f;
    float rightOffset = 1f;
    Vector2 left;
    Vector2 right;
    Vector3 areaBox = new Vector3(0.1f,1.8f,0);
    
    public PlayerInputActions playerInputActions;
    Vector3 movedirection;
    Vector3 moveObject;

    public bool on;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }
    private void Start()
    {
        on = true;
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
    void ReadValuePerformed(InputAction.CallbackContext value)
    {
        movedirection = value.ReadValue<Vector2>();
    }
    void ReadValueCanceled(InputAction.CallbackContext value)
    {
        movedirection = Vector2.zero;
    }
    private void Update()
    {
        left = transform.position + Vector3.left * leftOffset;
        right = transform.position + Vector3.right * rightOffset;

        if (on)
        {
            ObjectBeingPushed();
        }
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0, Wall))
        {
            touchingWall();
        }

    }
    void ObjectBeingPushed()
    {
        Vector2 leftDetect = transform.position + Vector3.left * leftOffset;
        Vector2 rightDetect = transform.position + Vector3.right * rightOffset;
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0, Player))
        {
            if (Physics2D.OverlapBox(leftDetect, new Vector3(0.1f, transform.localScale.y - Reduce, 0), 0, Player))
            {
                gameObject.layer = LayerMask.NameToLayer("MovableObject");
                moveObject = Vector3.right * speed * Time.deltaTime;
            }
            if (Physics2D.OverlapBox(rightDetect, new Vector3(0.1f, transform.localScale.y - Reduce, 0), 0, Player))
            {
                gameObject.layer = LayerMask.NameToLayer("MovableObject");
                moveObject = Vector3.left * speed * Time.deltaTime;
            }
        }
        else { moveObject =  Vector3.zero; }
        
        transform.Translate(moveObject);
    }
    void touchingWall()
    {
        gameObject.layer = LayerMask.NameToLayer("Wall");
        movedirection = Vector2.zero;
        on = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        if (on)
        {
            Gizmos.DrawCube(transform.position + Vector3.left * leftOffset, new Vector3(0.1f, transform.localScale.y - Reduce,0));
            Gizmos.DrawCube(transform.position + Vector3.right * rightOffset, new Vector3(0.1f, transform.localScale.y - Reduce, 0));
        }
    }
}
