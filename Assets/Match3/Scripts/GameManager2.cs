using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2 : Singleton<GameManager2>
{
    private MatchablePool pool;
    private MatchableGrid grid;
    private Cursor cursor;
    private AudioMixer audioMixer;
    private ScoreManager score;

    [SerializeField]
    private Fader   loadingScreen,
                    darkener;

    [SerializeField]
    private TMP_Text finalScoreText;

    [SerializeField]
    private Movable resultsPage;

    [SerializeField]
    private bool levelIsTimed;

    [SerializeField]
    private LevelTimer timer;

    [SerializeField]
    private float timeLimit;

    [SerializeField] private Vector2Int dimensions = Vector2Int.one;


    [SerializeField] private TMP_Text gridOutput;
    [SerializeField] private bool debugMode;

    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
        
        cursor = Cursor.Instance;
        audioMixer = AudioMixer.Instance;
        score = ScoreManager.Instance;

        StartCoroutine(Setup());
    }

    private void Update()
    {
        if (debugMode && Input.GetButtonDown("Jump"))
            NoMoreMoves();
    }

    private IEnumerator Setup()
    {
        yield return new WaitUntil(() => 
            MatchablePool.Instance != null && 
            MatchableGrid.Instance != null && 
            AudioMixer.Instance != null
        );

        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
        cursor = Cursor.Instance;
        audioMixer = AudioMixer.Instance;

        cursor.enabled = false;
        loadingScreen.Hide(false);

        if (levelIsTimed)
            timer.SetTimer(timeLimit);

        pool.PoolObjects(dimensions.x * dimensions.y * 2);
        grid.InitializeGrid(dimensions);
        StartCoroutine(loadingScreen.Fade(0));
        audioMixer.PlayMusic();

        yield return StartCoroutine(grid.PopulateGrid(false, true));
        grid.CheckPossibleMoves();
        cursor.enabled = true;

        if (levelIsTimed)
            StartCoroutine(timer.Countdown());
    }

    public void NoMoreMoves()
    {
        if(levelIsTimed)
            grid.MatchEverything();


        else
            GameOver();

        //grid.MatchEverything();
    }

    public void GameOver()
    {
        finalScoreText.text = score.Score.ToString();
        cursor.enabled = false;
        darkener.Hide(false);
        StartCoroutine(darkener.Fade(0.75f));
        StartCoroutine(resultsPage.MoveToPosition(Vector2.zero));
    }

    private IEnumerator Quit()
    {
        yield return StartCoroutine(loadingScreen.Fade(1));
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitButtonPressed()
    {
        StartCoroutine(Quit());
    }

    private IEnumerator Retry()
    {
        StartCoroutine(resultsPage.MoveToPosition(Vector2.down * 1000));
        yield return StartCoroutine(darkener.Fade(0));
        darkener.Hide(true);

        if (levelIsTimed)
            timer.SetTimer(timeLimit);

        cursor.Reset();
        score.Reset();
        yield return StartCoroutine(grid.Reset());

        cursor.enabled = true;

        if (levelIsTimed)
            StartCoroutine(timer.Countdown());
    }

    public void RetryButtonPressed()
    {
        StartCoroutine(Retry());
    }
}