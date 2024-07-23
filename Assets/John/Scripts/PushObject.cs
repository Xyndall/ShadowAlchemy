using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushObject : MonoBehaviour
{
    [SerializeField] LayerMask Player; 
    [SerializeField] LayerMask Wall;
    [SerializeField] float speed;
    public PlayerInputActions playerInputActions;
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
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0, Player))
        {
            ObjectBeingPushed();
        }
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0, Wall))
        {
            Debug.Log("Wall");
            touchingWall();
        }
    }
    void ObjectBeingPushed()
    {
       
        gameObject.layer = LayerMask.NameToLayer("MovableObject");
        Vector3 moveObject = new Vector3(movedirection.x, 0, 0) * speed * Time.deltaTime;
        transform.Translate(moveObject);
    }
    void touchingWall()
    {
        gameObject.layer = LayerMask.NameToLayer("Wall");
        movedirection = Vector2.zero;
    }
}
