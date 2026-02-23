using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchableGrid : GridSystem<Matchable>
{
    private MatchablePool pool;

    [SerializeField] private Vector3 offscreenOffset;

    private void Start()
    {
        pool = (MatchablePool) MatchablePool.Instance;    
    }

    public IEnumerator PopulateGrid(bool allowMatches = false)
    {
        Matchable newMatchable;
        Vector3 onscreenPosition;

        for (int y = 0; y != Dimensions.y; ++y)
            for(int x = 0; x != Dimensions.x; ++x)
            {
                newMatchable = pool.GetRandomMatchable();

                //newMatchable.transform.position = transform.position + new Vector3(x,y);
                onscreenPosition = transform.position + new Vector3(x, y);
                newMatchable.transform.position = onscreenPosition + offscreenOffset;

                newMatchable.gameObject.SetActive(true);

                newMatchable.position = new Vector2Int(x, y);

                PutItemAt(newMatchable, x, y);

                int type = newMatchable.Type;

                while(!allowMatches && IsPartOfAMatch(newMatchable))
                {
                    if (pool.NextType(newMatchable) == type)
                    {
                        Debug.LogWarning("failed to find a matchable type that didnt match at (" + x +", " + y + ")"); 
                        Debug.Break();
                        break;
                    }
                }

                StartCoroutine(newMatchable.MoveToPosition(onscreenPosition));

                yield return new WaitForSeconds(0.1f);
            }
        //yield return null;
    }

    private bool IsPartOfAMatch(Matchable matchable)
    {
        return false;
    }
}
