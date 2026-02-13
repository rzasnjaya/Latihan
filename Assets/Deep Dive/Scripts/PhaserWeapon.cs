using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserWeapon : MonoBehaviour
{
    public static PhaserWeapon Instance;

    [SerializeField] private GameObject prefab;

    public float speed;
    public int damage;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; 
        }   
    }

    public void Shoot()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
