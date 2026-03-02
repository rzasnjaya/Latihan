using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MatchableGrid : GridSystem<Matchable>
{
    private MatchablePool pool;
    private ScoreManager score;
    private HintIndicator hint;
    private AudioMixer audioMixer;

    [SerializeField] private Vector3 offscreenOffset;

    private List<Matchable> possibleMoves;

    private void Start()
    {
        pool = (MatchablePool) MatchablePool.Instance;   
        score = ScoreManager.Instance;
        hint = HintIndicator.Instance;
        audioMixer = AudioMixer.Instance;
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

            audioMixer.PlayDelayedSound(SoundEffects.land, 1f / newMatchables[i].Speed);

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

        hint.CancelHint();

        yield return StartCoroutine(Swap(copies));

        if (copies[0].IsGem && copies[1].IsGem)
        {
            MatchEverything();
            yield break;
        }

        else if (copies[0].IsGem)
        {
            MatchEverythingByType(copies[0], copies[1].Type);
            yield break;
        }
        else if (copies[1].IsGem)
        {
            MatchEverythingByType(copies[1], copies[0].Type);
            yield break;
        }

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
        else
            CheckPossibleMoves();
    }

    public void CheckPossibleMoves()
    {
        if(ScanForMoves() == 0)
        {
            GameManager2.Instance.NoMoreMoves();
        }
        else
        {
            //hint.EnableHintButton();

            hint.StartAutoHint(possibleMoves[Random.Range(0, possibleMoves.Count)].transform);
        }
    }

    private Match GetMatch(Matchable toMatch)
    {
        Match match = new Match(toMatch);

        Match horizontalMatch,
              verticalMatch;

        horizontalMatch = GetMatchesInDirection(match, toMatch, Vector2Int.left);
        horizontalMatch.Merge(GetMatchesInDirection(match, toMatch, Vector2Int.right));

        horizontalMatch.orientation = Orientation.horizontal;

        if (horizontalMatch.Count > 1)
        {
            match.Merge(horizontalMatch);

            GetBranches(match, horizontalMatch, Orientation.vertical);
        }

        verticalMatch = GetMatchesInDirection(match, toMatch, Vector2Int.up);
        verticalMatch.Merge(GetMatchesInDirection(match, toMatch, Vector2Int.down));

        verticalMatch.orientation = Orientation.vertical;

        if (verticalMatch.Count > 1)
        {
            match.Merge(verticalMatch);

            GetBranches(match, verticalMatch, Orientation.horizontal);
        }

        if(match.Count == 1)
            return null;

        return match;
    }

    private void GetBranches(Match tree, Match branchToSearch, Orientation perpendicular)
    {
        Match branch;

        foreach(Matchable matchable in branchToSearch.Matchables)
        {
            branch = GetMatchesInDirection(tree, matchable, perpendicular == Orientation.horizontal ? Vector2Int.left : Vector2Int.down);
            branch.Merge(GetMatchesInDirection(tree, matchable, perpendicular == Orientation.horizontal ? Vector2Int.right : Vector2Int.up));

            branch.orientation = perpendicular;

            if(branch.Count > 1)
            {
                tree.Merge(branch);
                GetBranches(tree, branch, perpendicular == Orientation.horizontal ? Orientation.vertical : Orientation.horizontal);
            }
        }
    }

    private Match GetMatchesInDirection(Match tree, Matchable toMatch, Vector2Int direction)
    {
        Match match = new Match();
        Vector2Int position = toMatch.position + direction;
        Matchable next;

        while (BoundsCheck(position) && !IsEmpty(position))
        {
            next = GetItemAt(position);

            if (next.Type == toMatch.Type && next.Idle)
            {
                if(!tree.Contains(next))
                    match.AddMatchable(next);
                else
                    match.AddUnlisted();

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

        audioMixer.PlaySound(SoundEffects.swap);

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

        audioMixer.PlayDelayedSound(SoundEffects.land, 1f / toMove.Speed);
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

    public void MatchAllAdjacent(Matchable powerup)
    {
        Match allAdjacent = new Match();

        for(int y = powerup.position.y - 1; y != powerup.position.y + 2; ++y)
            for(int x = powerup.position.x - 1; x != powerup.position.x +2; ++x)
                if(BoundsCheck(x, y) && !IsEmpty(x,y) && GetItemAt(x, y).Idle)
                {
                    allAdjacent.AddMatchable(GetItemAt(x,y));
                }

        StartCoroutine(score.ResolveMatch(allAdjacent, MatchType.match4));

        audioMixer.PlaySound(SoundEffects.powerup);
    }

    public void MatchRowAndColumn(Matchable powerup)
    {
        Match rowAndColumn = new Match();

        for (int y = 0; y !=  + Dimensions.y; ++y)
            if (BoundsCheck(powerup.position.x, y) && !IsEmpty(powerup.position.x, y) && GetItemAt(powerup.position.x, y).Idle)
                rowAndColumn.AddMatchable(GetItemAt(powerup.position.x, y));

        for (int x = 0; x != Dimensions.x; ++x)
            if (BoundsCheck(x, powerup.position.y) && !IsEmpty(x, powerup.position.y) && GetItemAt(x, powerup.position.y).Idle)
                rowAndColumn.AddMatchable(GetItemAt(x, powerup.position.y));

        StartCoroutine(score.ResolveMatch(rowAndColumn, MatchType.cross));

        audioMixer.PlaySound(SoundEffects.powerup);
    }

    public void MatchEverythingByType(Matchable gem, int type)
    {
        Match everythingByType = new Match(gem);

        for (int y = 0; y != Dimensions.y; ++y)
            for (int x = 0; x != Dimensions.x; ++x)
                if (BoundsCheck(x, y) && !IsEmpty(x, y) && GetItemAt(x, y).Idle && GetItemAt(x, y).Type == type)
                    everythingByType.AddMatchable(GetItemAt(x, y));

        StartCoroutine(score.ResolveMatch(everythingByType, MatchType.match5));
        StartCoroutine(FillAndScanGrid());

        audioMixer.PlaySound(SoundEffects.powerup);
    }

    public void MatchEverything()
    {
        Match everything = new Match();

        for (int y = 0; y != Dimensions.y; ++y)
            for (int x = 0; x != Dimensions.x; ++x)
                if (BoundsCheck(x, y) && !IsEmpty(x, y) && GetItemAt(x, y).Idle)
                    everything.AddMatchable(GetItemAt(x,y));

        StartCoroutine(score.ResolveMatch(everything, MatchType.match5));
        StartCoroutine(FillAndScanGrid());

        audioMixer.PlaySound(SoundEffects.powerup);
    }

    private int ScanForMoves()
    {
        possibleMoves = new List<Matchable>();

        for (int y = 0; y != Dimensions.y; ++y)
            for (int x = 0; x != Dimensions.x; ++x)
                if (BoundsCheck(x, y) && !IsEmpty(x, y) && CanMove(GetItemAt(x, y)))
                    possibleMoves.Add(GetItemAt(x, y));

                    return possibleMoves.Count;
    }

    private bool CanMove(Matchable toCheck)
    {
        if
        (
                CanMove(toCheck, Vector2Int.up) 
            ||  CanMove(toCheck, Vector2Int.right) 
            ||  CanMove(toCheck, Vector2Int.down) 
            ||  CanMove(toCheck, Vector2Int.left)
        )
            return true;

        if (toCheck.IsGem)
            return true;

        return false;
    }

    private bool CanMove(Matchable toCheck, Vector2Int direction)
    {
        Vector2Int cw = new Vector2Int(direction.y, -direction.x),
                   ccw = new Vector2Int(-direction.y, direction.x);

        // X X o  — dua di depan
        Vector2Int position1 = toCheck.position + direction + cw,
                   position2 = toCheck.position + direction + ccw;

        if (IsAPotentialMatch(toCheck, position1, position2))
            return true;

        // X o X  — satu di depan, dua ke kanan (cw)
        position1 = toCheck.position + direction + cw;
        position2 = toCheck.position + direction + cw * 2;

        if (IsAPotentialMatch(toCheck, position1, position2))
            return true;

        // o X X  — satu di depan, dua ke kiri (ccw)
        position1 = toCheck.position + direction + ccw;
        position2 = toCheck.position + direction + ccw * 2;

        if (IsAPotentialMatch(toCheck, position1, position2))
            return true;

        // lurus: o o X X
        position1 = toCheck.position + direction * 2;
        position2 = toCheck.position + direction * 3;

        if (IsAPotentialMatch(toCheck, position1, position2))
            return true;

        return false;
    }

    private bool IsAPotentialMatch(Matchable toCompare, Vector2Int position1, Vector2Int position2)
    {
        if
        (
            BoundsCheck(position1) && BoundsCheck(position2)
            && !IsEmpty(position1) && !IsEmpty(position2)
            && GetItemAt(position1).Idle && GetItemAt(position2).Idle
            && GetItemAt(position1).Type == toCompare.Type
            && GetItemAt(position2).Type == toCompare.Type
        )        
            return true;

        return false;
    }

    public void ShowHint()
    {
        hint.IndicateHint(possibleMoves[Random.Range(0, possibleMoves.Count)].transform);
    }
}
