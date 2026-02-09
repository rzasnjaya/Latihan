using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSpawn : MonoBehaviour
{
    public int chance;
    public GameObject diamond;
    public Rigidbody2D rb;
    public float offset;
    public float launch;
    // Start is called before the first frame update
    void Start()
    {
        int x = Random.Range(1, chance + 1);
        if (x != chance)
        {
            Destroy(gameObject.GetComponent<DiamondSpawn>());
        }
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject d = Instantiate(diamond) as GameObject;
            d.transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
            rb = d.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, launch);
            Destroy(gameObject.GetComponent<DiamondSpawn>());
        }
    }
}
