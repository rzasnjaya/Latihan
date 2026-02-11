using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public string state;
    public MapSpawner map;
    public Movement move;
    public Score score;
    public SpriteRenderer sprite;
    public ParticleSystem ps;
    public MusicManager music;
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
        sprite.enabled = false;
        ps.Play();
        music.PlayDie();
        StartCoroutine(Delay());
    }

    public void Respawn()
    {
        transform.position = new Vector3(-8.52f, -1.69f, 0);
        state = "Playing";
        move.ResetGravity(3);
        map.ClearStorage();
        sprite.enabled = true;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Respawn();
    }
}
