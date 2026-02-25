using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    none, 
    horizontal,
    vertical,
    both
}

public enum MatchType
{
    invalid,
    match3,
    match4,
    match5,
    cross
}


public class Match
{
    private int unlisted = 0;

    public Orientation orientation = Orientation.none;

    private List<Matchable> matchables;

    private Matchable toBeUpgraded = null;
    public List<Matchable> Matchables
    {
        get
        {
            return matchables;
        }
    }

    public int Count
    {
        get
        {
            return matchables.Count + unlisted;
        }
    }

    public bool Contains(Matchable toCompare)
    {
        return matchables.Contains(toCompare);
    }

    public Match()
    {
        matchables = new List<Matchable>(5);
    }

    public Match(Matchable original) : this()
    {
        AddMatchable(original);
        toBeUpgraded = original;
    }

    public MatchType Type
    {
        get
        {
            if (orientation == Orientation.both)
                return MatchType.cross;

            else if (matchables.Count > 4)
                return MatchType.match5;

            else if (matchables.Count == 4)
                return MatchType.match4;

            else if (matchables.Count == 3)
                return MatchType.match3;

            else
                return MatchType.invalid;
        }

    }

    public Matchable ToBeUpgraded
    {
        get
        {
            if (toBeUpgraded != null)
                return toBeUpgraded;

            return matchables[Random.Range(0, matchables.Count)];
        }
    }

    public void AddMatchable(Matchable toAdd)
    {
        matchables.Add(toAdd);
    }

    public void AddUnlisted()
    {
        ++unlisted;
    }

    public void RemoveMatchable(Matchable toBeRemoved)
    {
        matchables.Remove(toBeRemoved);
    }

    public void Merge(Match toMerge)
    {
        matchables.AddRange(toMerge.Matchables);

        if
        (
                orientation == Orientation.both
            ||  toMerge.orientation == Orientation.both
            ||  (orientation == Orientation.horizontal && toMerge.orientation == Orientation.vertical)
            ||  (orientation == Orientation.vertical && toMerge.orientation == Orientation.horizontal)
        )
            orientation = Orientation.both;

        else if(toMerge.orientation == Orientation.horizontal)
            orientation = Orientation.horizontal;

        else if(toMerge.orientation == Orientation.vertical)
            orientation = Orientation.vertical;
    }

    public override string ToString()
    {
        string s = "Match of type" + matchables[0].Type + " ; ";

        foreach (Matchable m in Matchables)
            s += "(" + m.position.x + ", " + m.position.y + ") ";

        return s;
    }
}
