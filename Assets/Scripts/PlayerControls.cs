using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    public FrontCheck frontCh;
    public GroundCheck groundCh;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] float gravityBase;
    [SerializeField] float gravityIncrease;
    [SerializeField] float velocity;

    
    Vector3 movedirection;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (frontCh.touchingWall)
        {
            horizontalInput = 0;
        }
        movedirection.x = horizontalInput * speed * Time.deltaTime;

        transform.Translate(movedirection);

        velocity += gravity * Time.deltaTime;
        if(groundCh.groundCheck && velocity < 0)
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
            gravity -= gravityIncrease *Time.deltaTime;
        }
        transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);

        
    }
}
