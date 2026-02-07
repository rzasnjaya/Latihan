using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed;
    public float JumpHeight;
    public Rigidbody2D rb;
    public bool IsGrounded;
    public float x;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.W) && IsGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(x * PlayerSpeed, rb.velocity.y);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }    
    }

    void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }       
    }
}
