using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyObject : MonoBehaviour
{
    public Transform boundary;
    public bool travright;
    public Rigidbody2D rb;
    public float vel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(vel, 0 );

        if(travright)
        {
            if (transform.position.x > boundary.position.x)
            Destroy(gameObject);
        }
        else
        {
            if (transform.position.x < boundary.position.x)
            Destroy(gameObject);
        }
    }
}
