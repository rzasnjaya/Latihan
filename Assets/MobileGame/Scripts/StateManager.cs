using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public string state;
    public MapSpawner map;
    public Movement move;
    public Score score;
    void Start()
    {
        state = "Playing";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    public void Die()
    {
        state = "Dead";
        move.ResetGravity(0);
        score.End();
        StartCoroutine(Delay());
    }

    public void Respawn()
    {
        transform.position = new Vector3(-8.52f, -1.69f, 0);
        state = "Playing";
        move.ResetGravity(3);
        map.ClearStorage();
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Respawn();
    }
}
