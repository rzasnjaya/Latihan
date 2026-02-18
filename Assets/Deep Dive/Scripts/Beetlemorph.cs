using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetlemorph : Enemy
{
    [SerializeField] private Sprite[] sprites;
    private float timer;
    private float frequency;
    private float amplitude;
    private float centerY;

    public override void OnEnable()
    {
        base.OnEnable();
        timer = transform.position.y;
        frequency = Random.Range(0.3f, 1f);
        amplitude = Random.Range(0.8f, 1.5f);
        centerY = transform.position.y;
    }

    public override void Start()
    {
        base.Start();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        destroyEffectPool = GameObject.Find("BeetlePopPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager2.Instance.beetleHit;
        destroySound = AudioManager2.Instance.beetleDestroy;
        speedX = Random.Range(-0.8f, -1.5f);
    }

    public override void Update()
    {
        base.Update();

        timer -= Time.deltaTime;
        float sine = Mathf.Sin(timer * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, centerY + sine);
    }
}
