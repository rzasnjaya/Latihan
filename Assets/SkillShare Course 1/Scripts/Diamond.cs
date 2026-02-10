using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public ParticleSystem ps;
    public SpriteRenderer sprite;
    public bool collected;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collected)
        {
            collision.GetComponent<PlayerDiamonds>().Diamonds += 1;
            collision.GetComponent<PlayerDiamonds>().UpdateText();
            ps.Play();
            collected = true;
            sprite.enabled = false;
        }
        else if (collision.gameObject.CompareTag("MainCamera"))
        {
            Destroy(gameObject);
        }
    }
}
