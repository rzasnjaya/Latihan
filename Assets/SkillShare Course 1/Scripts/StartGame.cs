using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public CameraMove cameramove;
    public MapGeneration map;
    public Animator UIanim;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            map.BeginGame();
            cameramove.speed = cameramove.CameraSpeed;
            UIanim.SetTrigger("Start");
        }
    }
}
