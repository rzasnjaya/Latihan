using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MatchableGrid : GridSystem<Matchable>
{
    private MatchablePool pool;
    private ScoreManager score;

    [SerializeField] private Vector3 offscreenOffset;

    private void Start()
    {
        pool = (MatchablePool) MatchablePool.Instance;   
        score = ScoreManager.Instance;
    }

    public IEnumerator PopulateGrid(bool allowMatches = false, bool initialPopulation = false)
    {
        List<Matchable> newMatchables = new List<Matchable>();

        Matchable newMatchable;
        Vector3 onscreenPosition;

        for (int y = 0; y != Dimensions.y; ++y)
            for (int x = 0; x != Dimensions.x; ++x)
                if (IsEmpty(x, y))
                {
                    newMatchable = pool.GetRandomMatchable();

                    newMatchable.transform.position = transform.position + new Vector3(x, y) + offscreenOffset;

                    newMatchable.gameObject.SetActive(true);

                    newMatchable.position = new Vector2Int(x, y);

                    PutItemAt(newMatchable, x, y);

                    newMatchables.Add(newMatchable);

                    int type = newMatchable.Type;

                    while (!allowMatches && IsPartOfAMatch(newMatchable))
                    {
                        if (pool.NextType(newMatchable) == type)
                        {
                            Debug.LogWarning("failed to find a matchable type that didnt match at (" + x + ", " + y + ")");
                            Debug.Break();
                            yield return null;
                            break;
                        }
                    }
                }
        for (int i = 0; i != newMatchables.Count; ++i)
        {
            onscreenPosition = transform.position + new Vector3(newMatchables[i].position.x, newMatchables[i].position.y);

            if (i == newMatchables.Count - 1)
                yield return StartCoroutine(newMatchables[i].MoveToPosition(onscreenPosition));
            else
                StartCoroutine(newMatchables[i].MoveToPosition(onscreenPosition));

            if (initialPopulation)
                yield return new WaitForSeconds(0.1f);
        }
    }

    private bool IsPartOfAMatch(Matchable toMatch)
    {
        int horizontalMatches = 0,
            verticalMatches = 0;

        horizontalMatches += CountMatchesInDirection(toMatch, Vector2Int.left);
        horizontalMatches += CountMatchesInDirection(toMatch, Vector2Int.right);

        if (horizontalMatches > 1)
            return true;

        verticalMatches += CountMatchesInDirection(toMatch, Vector2Int.up);
        verticalMatches += CountMatchesInDirection(toMatch, Vector2Int.down);

        if (verticalMatches > 1)
            return true;

        return false;
    }

    private int CountMatchesInDirection(Matchable toMatch, Vector2Int direction)
    {
        int matches = 0;
        Vector2Int position = toMatch.position + direction;

        if (BoundsCheck(position) && !IsEmpty(position) && GetItemAt(position).Type == toMatch.Type)
        {
            ++matches;
            position += direction;
        }
        return matches;
    }

    public IEnumerator TrySwap(Matchable[] toBeSwapped)
    {
        Matchable[] copies = new Matchable[2];
        copies[0] = toBeSwapped[0];
        copies[1] = toBeSwapped[1];

        yield return StartCoroutine(Swap(copies));

        Match[] matches = new Match[2];

        matches[0] = GetMatch(copies[0]);
        matches[1] = GetMatch(copies[1]);

        if (matches[0] != null)
            StartCoroutine(score.ResolveMatch(matches[0]));
        if (matches[1] != null)
            StartCoroutine(score.ResolveMatch(matches[1]));

        if (matches[0] == null && matches[1] == null)
        {
            yield return StartCoroutine(Swap(copies));

            if(ScanForMatches())
                StartCoroutine(FillAndScanGrid());
        }
        else
            StartCoroutine(FillAndScanGrid());
    }

    private IEnumerator FillAndScanGrid()
    {
        CollapseGrid();
        yield return StartCoroutine(PopulateGrid(true));

        if (ScanForMatches())
            StartCoroutine(FillAndScanGrid());
    }

    private Match GetMatch(Matchable toMatch)
    {
        Match match = new Match(toMatch);

        Match horizontalMatch,
              verticalMatch;

        horizontalMatch = GetMatchesInDirection(toMatch, Vector2Int.left);
        horizontalMatch.Merge(GetMatchesInDirection(toMatch, Vector2Int.right));

        if (horizontalMatch.Count > 1)
            match.Merge(horizontalMatch);

        verticalMatch = GetMatchesInDirection(toMatch, Vector2Int.up);
        verticalMatch.Merge(GetMatchesInDirection(toMatch, Vector2Int.down));

        if (verticalMatch.Count > 1)
            match.Merge(verticalMatch);

        if(match.Count == 1)
            return null;

        return match;
    }

    private Match GetMatchesInDirection(Matchable toMatch, Vector2Int direction)
    {
        Match match = new Match();
        Vector2Int position = toMatch.position + direction;
        Matchable next;

        while (BoundsCheck(position) && !IsEmpty(position))
        {
            next = GetItemAt(position);

            if (next.Type == toMatch.Type && next.Idle)
            {
                match.AddMatchable(next);
                position += direction;
            }
            else
                break;
        }
        return match;
    }

    private IEnumerator Swap(Matchable[] toBeSwapped)
    {
        SwapItemsAt(toBeSwapped[0].position, toBeSwapped[1].position);

        Vector2Int temp = toBeSwapped[0].position;
        toBeSwapped[0].position = toBeSwapped[1].position;
        toBeSwapped[1].position = temp;

        Vector3[] worldPosition = new Vector3[2];
        worldPosition[0] = toBeSwapped[0].transform.position;
        worldPosition[1] = toBeSwapped[1].transform.position;

                        StartCoroutine(toBeSwapped[0].MoveToPosition(worldPosition[1]));
        yield return    StartCoroutine(toBeSwapped[1].MoveToPosition(worldPosition[0]));
    }

    private void CollapseGrid()
    {
        for (int x = 0; x != Dimensions.x; ++x)
            for (int yEmpty = 0; yEmpty != Dimensions.y - 1; ++yEmpty)
                if (IsEmpty(x, yEmpty))
                    for (int yNotEmpty = yEmpty + 1; yNotEmpty != Dimensions.y; ++yNotEmpty)
                        if (!IsEmpty(x, yNotEmpty) && GetItemAt(x, yNotEmpty).Idle)
                        {
                            MoveMatchableToPosition(GetItemAt(x, yNotEmpty), x, yEmpty);
                            break;
                        }
    }

    private void MoveMatchableToPosition(Matchable toMove, int x, int y)
    {
        MoveItemTo(toMove.position, new Vector2Int(x, y));

        toMove.position = new Vector2Int(x, y);

        StartCoroutine(toMove.MoveToPosition(transform.position + new Vector3(x, y)));
    }

    private bool ScanForMatches()
    {
        bool madeAMatch = false;
        Matchable toMatch;
        Match match;

        for(int y = 0; y != Dimensions.y; ++y)
            for(int x = 0; x != Dimensions.x; ++x)
                if(!IsEmpty(x, y))
                {
                    toMatch = GetItemAt(x, y);

                    if (!toMatch.Idle)
                        continue;

                    match = GetMatch(toMatch);

                    if (match != null)
                    {
                        madeAMatch = true;
                        StartCoroutine(score.ResolveMatch(match));
                    }
                }
        return madeAMatch;
    }
}
