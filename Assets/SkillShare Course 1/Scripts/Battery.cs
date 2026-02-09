using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public BoxCollider2D box1, box2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BatteryOff()
    {
        box1.enabled = false;
        box2.enabled = false;

    }

    public void BatteryOn()
    {
        box1.enabled = true;
        box2.enabled = true;

    }
}
