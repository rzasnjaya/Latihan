using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClear : MonoBehaviour
{
    public Transform cam;
    public bool robotonboard;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - cam.position.x <= -28)
        {
            Destroy(gameObject);
            if (robotonboard)
                GameObject.Find("AudioManager").GetComponent<AudioManager>().sfx[1].volume = 0f;
        }
    }
}
