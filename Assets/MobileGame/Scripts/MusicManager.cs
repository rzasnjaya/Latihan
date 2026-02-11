using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource AsMusic, AsJump, AsDie;

    void Start()
    {
        Play();   
    }

    public void Play()
    {
        AsMusic.Play();
    }

    public void PlayJump()
    {
        AsJump.Play();
    }

    public void PlayDie()
    {
        AsDie.Play();
    }
}
