using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed;
    public int jumpforce;
    public bool grounded;
    public StateManager statemanage;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (grounded)
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        if (statemanage.state == "Playing")
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
