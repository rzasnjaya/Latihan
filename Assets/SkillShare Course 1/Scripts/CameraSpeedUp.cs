using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpeedUp : MonoBehaviour
{
    public float TimeElapsed = 0;
    public float[] CameraSpeeds;
    public float[] Times;
    public CameraMove camerascript;
    public PlayerHealth player;
    public int index = 0;
    public bool begun;

    public void Begin()
    {
        camerascript.CameraSpeed = CameraSpeeds[index];
        begun = true; 
    }

    void Update()
    {
        if (begun)
            TimeElapsed += Time.deltaTime;

        if ((TimeElapsed > Times[index + 1]) && (player.isDead == false))
        {
            index++;
            camerascript.CameraSpeed = CameraSpeeds[index];
            camerascript.speed = camerascript.CameraSpeed;
        }
        if (index == CameraSpeeds.Length-1)
            gameObject.GetComponent<CameraSpeedUp>().enabled = false;
    }
}
