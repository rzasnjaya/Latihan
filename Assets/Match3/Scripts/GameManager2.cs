using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager2 : Singleton<GameManager2>
{
    private MatchablePool pool;
    private MatchableGrid grid;
    private Cursor cursor;
    private AudioMixer audioMixer;

    [SerializeField]
    private Fader loadingScreen;

    [SerializeField] private Vector2Int dimensions = Vector2Int.one;


    [SerializeField] private TMP_Text gridOutput;

    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
        
        cursor = Cursor.Instance;
        audioMixer = AudioMixer.Instance;
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        cursor.enabled = false;

        loadingScreen.Hide(false);

        pool.PoolObjects(dimensions.x * dimensions.y * 2);

        grid.InitializeGrid(dimensions);

        StartCoroutine(loadingScreen.Fade(0));

        audioMixer.PlayMusic();
        
        yield return StartCoroutine(grid.PopulateGrid(false, true));

        grid.CheckPossibleMoves();

        cursor.enabled = true;
    }

    public void NoMoreMoves()
    {
        grid.MatchEverything();
    }
}