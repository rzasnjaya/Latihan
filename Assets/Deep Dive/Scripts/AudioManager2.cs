using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    public static AudioManager2 Instance;

    public AudioSource ice;
    public AudioSource fire;
    public AudioSource hit;
    public AudioSource pause;
    public AudioSource unpause;

    
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            {
                Instance = this;
            }
        }
    }

    public  void PlaySound(AudioSource sound)
    {
        sound.Stop();
        sound.Play();
    }
}
