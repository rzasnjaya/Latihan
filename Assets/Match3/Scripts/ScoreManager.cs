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

    public IEnumerator ResolveMatch(Match toResolve)
    {
        Matchable powerup = null;
        Matchable matchable;

        Transform target = collectionPoint;

        if (toResolve.Count > 3)
        {
            powerup = pool.UpgradeMatchable(toResolve.ToBeUpgraded, toResolve.Type);
            toResolve.RemoveMatchable(powerup);
            target = powerup.transform;
            powerup.SortingOrder = 3;
        }

            for (int i = 0; i != toResolve.Count; ++i)
            {
                matchable = toResolve.Matchables[i];

                grid.RemoveItemAt(matchable.position);

                if (i == toResolve.Count - 1)
                    yield return StartCoroutine(matchable.Resolve(target));
                else
                    StartCoroutine(matchable.Resolve(target));
            }
        AddScore(toResolve.Count * toResolve.Count);

        if(powerup != null) 
            powerup.SortingOrder = 1;
    }
}
