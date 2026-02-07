using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClear : MonoBehaviour
{
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - cam.position.x <= -28)
        Destroy(gameObject);   
    }
}
