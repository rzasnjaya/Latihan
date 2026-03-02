using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreManager : Singleton<ScoreManager>
{
    private MatchablePool pool;
    private MatchableGrid grid;
    private AudioMixer audioMixer;

    [SerializeField]
    private Transform collectionPoint;

    [SerializeField]
    private TMP_Text    scoreText,
                        comboText;

    [SerializeField]
    private Slider comboSlider;

    private int score,
                comboMultiplier;

    public int Score
    { 
        get 
        {
            return score; 
        } 
    }

    private float timeSinceLastScore;

    [SerializeField]
    private float   maxComboTIme,
                    currentComboTime;

    private bool timerIsActive;

    private void Start()
    {        
        grid = (MatchableGrid)MatchableGrid.Instance;
        pool = (MatchablePool)MatchablePool.Instance;
        audioMixer = AudioMixer.Instance;

        comboText.enabled = false;
        comboSlider.gameObject.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount * IncreaseCombo();
        scoreText.text = "Score : " + score;

        timeSinceLastScore = 0;

        if(!timerIsActive)
            StartCoroutine(ComboTimer());

        audioMixer.PlaySound(SoundEffects.score);
    }

    private IEnumerator ComboTimer()
    {
        timerIsActive = true;
        comboText.enabled = true;
        comboSlider.gameObject.SetActive(true);

        do
        {
            timeSinceLastScore += Time.deltaTime;
            comboSlider.value = 1 - timeSinceLastScore / currentComboTime;
            yield return null;
        }
        while (timeSinceLastScore < currentComboTime);

        comboMultiplier = 0;
        comboText.enabled = false;
        comboSlider.gameObject.SetActive(false);

        timerIsActive = false;
    }

    private int IncreaseCombo()
    {
        comboText.text = "Combo x" + ++comboMultiplier;

        currentComboTime = maxComboTIme - Mathf.Log(comboMultiplier) / 2;

        return comboMultiplier;
    }

    public IEnumerator ResolveMatch(Match toResolve, MatchType powerupUsed = MatchType.invalid)
    {
        Matchable powerupFormed = null;
        Matchable matchable;

        Transform target = collectionPoint;

        if (powerupUsed == MatchType.invalid && toResolve.Count > 3)
        {
            powerupFormed = pool.UpgradeMatchable(toResolve.ToBeUpgraded, toResolve.Type);
            toResolve.RemoveMatchable(powerupFormed);
            target = powerupFormed.transform;
            powerupFormed.SortingOrder = 3;

            audioMixer.PlaySound(SoundEffects.upgrade);
        }
        else
        {
            audioMixer.PlaySound(SoundEffects.resolve);
        }

            for (int i = 0; i != toResolve.Count; ++i)
            {
                matchable = toResolve.Matchables[i];

                if (powerupUsed != MatchType.match5 && matchable.IsGem)
                    continue;

                grid.RemoveItemAt(matchable.position);

                if (i == toResolve.Count - 1)
                    yield return StartCoroutine(matchable.Resolve(target));
                else
                    StartCoroutine(matchable.Resolve(target));
            }
        AddScore(toResolve.Count * toResolve.Count);

        if(powerupFormed != null)
            powerupFormed.SortingOrder = 1;
    }
}
