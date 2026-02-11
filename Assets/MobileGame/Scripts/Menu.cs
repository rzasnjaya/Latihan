using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI title, score, highscore;
    public Button play, music, sound;
    public bool Music, Audio;
    public MusicManager MusicManage;

    public void ChangeMenu(bool x)
    {
        title.gameObject.SetActive(x);
        score.gameObject.SetActive(!x);
        highscore.gameObject.SetActive(!x);
        play.gameObject.SetActive(x);
        music.gameObject.SetActive(x);
        sound.gameObject.SetActive(x);
    }

    public void ChangeAudio()
    {
        Audio = !Audio;
        MusicManage.AsDie.mute = Audio;
        MusicManage.AsJump.mute = Audio;
    }

    public void ChangeMusic()
    {
        Music = !Music;
        MusicManage.AsMusic.mute = Music;
    }
}
