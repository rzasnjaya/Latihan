using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxInvTime;
    public float InvTime;
    public SpriteRenderer sprite;
    public float ColorChangeTime, InvDel;
    public float Transparancy;
    // Start is called before the first frame update
    void Start()
    {
        
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
        print("Player took" + damage.ToString() + " damage.");
        InvTime = MaxInvTime;
        StartCoroutine(TakeDamageEffect());
        StartCoroutine(InvicibilityTime());
    }

    void OnTriggerStay2D (Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy")) && (InvTime <= 0))
        {
            TakeDamage(collision.gameObject.GetComponent<EnemyInfo>().PlayerDamage);
        }
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
}
