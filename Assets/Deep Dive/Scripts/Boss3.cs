using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : Enemy
{
    private Animator animator;
    private float basePositionX;
    private float chargeSpeed;

    private ObjectPooler projectilePool;
    private float shootTimer;
    private float shootInterval;

    public override void OnEnable()
    {
        base.OnEnable();
        basePositionX = 7f + Random.Range(-1f, 1f);
        chargeSpeed = Random.Range(1.2f, 2.5f);
        speedY = Random.value < 0.5 ? -1f : 1f;
        shootInterval = Random.Range(2f, 3f);
        shootTimer = 1f;
    }

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        destroyEffectPool = GameObject.Find("Boom3Pool").GetComponent<ObjectPooler>();
        projectilePool = GameObject.Find("SquidPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager2.Instance.squidHit2;
        destroySound = AudioManager2.Instance.boom2;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        //
        float currentX = transform.position.x;
        if (Mathf.Abs(currentX - basePositionX) > 0.1f)
        {
            float posX = Mathf.Lerp(currentX, basePositionX, chargeSpeed * Time.deltaTime);
            transform.position = new Vector3(posX, transform.position.y);
        }

        //
        if (transform.position.y > 5 || transform.position.y < -5f)
        {
            speedY *= -1f;
        }


        //
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer += shootInterval;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectile = projectilePool.GetPooledObject();
        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;
        projectile.SetActive(true);AudioManager2.Instance.PlaySound(AudioManager2.Instance.squidShoot3);
        animator.SetBool("shooting", true);
        StartCoroutine(ResetShoot());
    }

    IEnumerator ResetShoot()
    {
        yield return null;
        animator.SetBool("shooting", false);
    }
}
