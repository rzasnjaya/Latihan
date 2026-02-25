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

    [SerializeField]
    private Transform collectionPoint;

    private TMP_Text scoreText;
    private int score;

    public int Score
    { 
        get 
        {
            return score; 
        } 
    }

    protected override void Init()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        grid = (MatchableGrid)MatchableGrid.Instance;
        pool = (MatchablePool)MatchablePool.Instance;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score;
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
