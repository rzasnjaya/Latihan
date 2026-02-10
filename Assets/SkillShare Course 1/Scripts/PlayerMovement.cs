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
    public Animator anim;
    public AudioManager am;
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

        anim.SetBool("IsRunning", x !=0);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(x * PlayerSpeed, rb.velocity.y);

        if (rb.velocity.x < 0)
        transform.rotation = Quaternion.Euler(0,180,0);
        else if (rb.velocity.x > 0)
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    public void Jump()
    {
        am.PlaySound(am.sfx[0]);
        rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
            anim.SetBool("IsGrounded", IsGrounded);
        }    
    }

    void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
            anim.SetBool("IsGrounded", IsGrounded);
        }       
    }
}
