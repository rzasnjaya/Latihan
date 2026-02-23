using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : Singleton<GameManager2>
{
    private MatchablePool pool;

    private void Start()
    {
        pool = (MatchablePool) MatchablePool.Instance;

        pool.PoolObjects(10);

        StartCoroutine(Demo());
    }

    private IEnumerator Demo()
    {
        Matchable m = pool.GetPooledObject();

        m.gameObject.SetActive(true);

        Vector3 randomPosition;

        for (int i = 0; i != 7; ++i)
        {
            randomPosition = new Vector3(Random.Range(-6f, 6f), Random.Range(-4f, 4f), 0);

            yield return StartCoroutine(m.MoveToPosition(randomPosition));
        }
    }
}
