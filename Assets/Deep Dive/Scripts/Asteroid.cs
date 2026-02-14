using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private FlashWhite flashWhite;

    private ObjectPooler destroyEffectPool;
    private int lives;
    private int maxLives;
    private int damage;

    [SerializeField] private Sprite[] sprites;

    void onEnbale()
    {
        lives = maxLives;
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        flashWhite = GetComponent<FlashWhite>();
        destroyEffectPool = GameObject.Find("Boom2Pool").GetComponent<ObjectPooler>();
        
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        float pushX = Random.Range(-1f, 0);
        float pushY = Random.Range(-1f, 1f);

        rb.velocity = new Vector2(pushX,pushY);
        float randomScale = Random.Range(0.6f, 1f);
        transform.localScale = new Vector2(randomScale, randomScale);

        maxLives = 5;
        lives = maxLives;
        damage = 1;

    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player) player.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        AudioManager2.Instance.PlayModifiedSound(AudioManager2.Instance.hitrock);
        lives -= damage;
        flashWhite.Flash();
        if (lives <=0)
        {
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);

            AudioManager2.Instance.PlayModifiedSound(AudioManager2.Instance.boom2);
            flashWhite.Reset();
            gameObject.SetActive(false);
        }
    }
}
