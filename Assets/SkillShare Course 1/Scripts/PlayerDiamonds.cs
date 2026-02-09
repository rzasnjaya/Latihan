using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDiamonds : MonoBehaviour
{
    public int Diamonds;
    public int HighScoreDiamonds;
    public TMP_Text counter;

    public void UpdateText()
    {
        counter.text = Diamonds.ToString();
    }
}
