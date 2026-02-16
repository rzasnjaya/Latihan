using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetlemorph : Enemy
{
    [SerializeField] private Sprite[] sprites;

    public override void Start()
    {
        base.Start();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        destroyEffectPool = GameObject.Find("BeetlePopPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager2.Instance.beetleHit;
        destroySound = AudioManager2.Instance.beetleDestroy;
        speedX = Random.Range(-0.8f, -1.5f);
    }
}
