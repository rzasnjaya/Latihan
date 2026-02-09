using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{
    public float speed;
    public float x;
    public float minx, maxx;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        x = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x < minx)
        {
        transform.rotation = Quaternion.Euler(0,180,0);
        x = speed;
        }
        else if (transform.localPosition.x > maxx)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            x = -speed;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(x,0);
    }
}
