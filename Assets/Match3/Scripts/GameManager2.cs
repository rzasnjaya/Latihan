using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager2 : Singleton<GameManager2>
{
    private MatchablePool pool;
    private MatchableGrid grid;

    [SerializeField] private Vector2Int dimensions = Vector2Int.one;


    [SerializeField] private TMP_Text gridOutput;

    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
        
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {


        pool.PoolObjects(dimensions.x * dimensions.y * 2);

        grid.InitializeGrid(dimensions);

        yield return null;

        StartCoroutine(grid.PopulateGrid());


    }
}