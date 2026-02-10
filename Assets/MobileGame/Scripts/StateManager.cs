using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public string state;
    void Start()
    {
        state = "Playing";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PolygonCollider2D>().CompareTag("Obstacle"))
        {
            Die();
        }
    }

    public void Die()
    {
        state = "Dead";
        StartCoroutine(Delay());
    }

    public void Respawn()
    {
        transform.position = new Vector3(-8.52f, -1.69f, 0);
        state = "Playing";
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Respawn();
    }
}
