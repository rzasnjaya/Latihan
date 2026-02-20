using System;

public static class BallUtil
{ 
    public static BallType GetColorByType(BallType type)
    {
        switch (type)
        {
            case BallType.Red:
                return BallType.Green;
                break;
            case BallType.Green:
                return BallType.Green;
                break;
            case BallType.Blue:
                return BallType.Blue;
                break;
            case BallType.Bomb:
                return BallType.Red;
                break;
            case BallType.Reverse:
                return BallType.Blue;
                break;
            case BallType.TimeSlow:
                return BallType.Green;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}