using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] sfx;
    public AudioSource music;
    public bool mutedmusic, mutedsfx;
    public Button Sfx, Music;
    public Color greyed;



    public void MuteMusic()
    {
        music.mute = !mutedmusic;
        mutedmusic = !mutedmusic;
        if (mutedmusic)
            Music.image.color = greyed;
        else
            Music.image.color = Color.white;
    }

    public void UnmuteMuteSFX()
    {
        foreach (AudioSource As in sfx)
            As.mute = !mutedsfx;
            mutedsfx = !mutedsfx;
        if (mutedsfx)
            Sfx.image.color = greyed;
        else
            Sfx.image.color = Color.white;
    }

    public void PlaySound(AudioSource As)
    {
        As.Play();
    }
}
