using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Score : MonoBehaviour
{
    public Transform player;
    public int score;
    public TextMeshProUGUI scoretext, highscoretext;
    public int highscore;

    void Start()
    {
        highscore = PlayerPrefs.GetInt("Score");
        highscoretext.text = "Highscore: " + highscore.ToString();
    }

    void Update()
    {
        score = Mathf.RoundToInt(player.transform.position.x - transform.position.x);
        scoretext.text = "Score: " + score.ToString();
    }

    public void End()
    {
        if (score > highscore)
        {
            highscore = score;
            highscoretext.text = "Highscore: " + highscore.ToString();
            PlayerPrefs.SetInt("Score", highscore);
            PlayerPrefs.Save();
        }
    }
}
