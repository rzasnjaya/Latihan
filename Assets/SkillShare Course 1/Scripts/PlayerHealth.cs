using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float MaxInvTime;
    public float InvTime;
    public SpriteRenderer sprite;
    public float ColorChangeTime, InvDel;
    public float Transparancy;
    public int MaxHealth;
    public int CurHealth;
    public Sprite dead;
    public float yoffset;
    public bool isDead;
    public GameObject curtain;

    public Sprite[] hearts;
    public Image heartsicon;
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        CurHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (InvTime > 0)
        {
            InvTime -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        CurHealth -= damage;
        CheckDead();
    }

    public void CheckDead()
    {
        if (CurHealth <= 0 && !isDead)
            Death();

        else if (!isDead)
        {
            InvTime = MaxInvTime;
            StartCoroutine(TakeDamageEffect());
            StartCoroutine(InvicibilityTime());
            heartsicon.sprite = hearts[CurHealth];
        }
    }

    public void Death()
    {
        isDead = true;
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + yoffset, transform.position.z);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = dead;
        GameObject.Find("Main Camera").GetComponent<CameraMove>().speed = 0;  
        heartsicon.sprite = hearts[0];  
        StartCoroutine(ReloadDelay());
        gameObject.GetComponent<PlayerDiamonds>().SaveDiamonds();
        ps.Play();
    }

    void OnTriggerStay2D (Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy")) && (InvTime <= 0))
        {
            TakeDamage(collision.gameObject.GetComponent<EnemyInfo>().PlayerDamage);
        }
        else if ((collision.gameObject.CompareTag("MainCamera")) && !isDead)
        Death();
        
    }

    IEnumerator TakeDamageEffect()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(ColorChangeTime);
        sprite.color = Color.white;
    }

    IEnumerator InvicibilityTime()
    {
        while (InvTime > 0)
        {
            sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b, Transparancy);
            yield return new WaitForSeconds(InvDel);
            sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,1);
            yield return new WaitForSeconds(InvDel);
        }
    }

    IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(2);
        curtain.SetActive(true);
        curtain.GetComponent<CurtainScript>().FadeTo();
    }
}
