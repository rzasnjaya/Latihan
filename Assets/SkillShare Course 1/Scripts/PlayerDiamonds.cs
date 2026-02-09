using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDiamonds : MonoBehaviour
{
    public int Diamonds;
    public int HighScoreDiamonds = 0;
    public TMP_Text counter;

    void Start()
    {
        if (PlayerPrefs.HasKey("Diamonds"))
        HighScoreDiamonds = PlayerPrefs.GetInt("Diamonds");
    }

    public void UpdateText()
    {
        counter.text = Diamonds.ToString();
    }

    public void SaveDiamonds()
    {
        if (Diamonds > HighScoreDiamonds)
        {
        PlayerPrefs.SetInt("Diamonds", Diamonds);
        PlayerPrefs.Save();
        }
    }
}
