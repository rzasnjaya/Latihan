using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserBullet : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(PhaserWeapon.Instance.speed * Time.deltaTime, 0f);
        if (transform.position.x > 9)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Critter"))
        {
            gameObject.SetActive(false);
        }
    }
}
