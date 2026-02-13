using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter1 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    private float moveSpeed;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private float moveTimer;
    private float moveInterval;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        moveSpeed = Random.Range(0.5f, 3f);
        GenerateRandomPosition();
        moveInterval = Random.Range(0.1f, 2f);
        moveTimer = moveInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        }
        else
        {
            GenerateRandomPosition();
            moveInterval = Random.Range(0.1f, 2f);
            moveTimer = moveInterval;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); 

        Vector3 relativePos = targetPosition - targetPosition;
        if (relativePos != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(Vector3.forward,relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1080 & Time.deltaTime);
        }

        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
    }

    private void GenerateRandomPosition()
    {
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(-5f, 5f);
        targetPosition = new Vector2(randomX, randomY);
    }
}
