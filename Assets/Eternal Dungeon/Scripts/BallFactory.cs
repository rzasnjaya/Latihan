using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallFactory : MonoBehaviour
{
    private static readonly BallType[] Colors =
    {
        BallType.Red,
        BallType.Green,
        BallType.Blue
    };

    private static readonly BallType[] SpecialTypes =
    {
        BallType.Bomb,
        BallType.Reverse,
        BallType.TimeSlow
    };

    public Ball ballPrefab;

    public Sprite redSprite;
    public Sprite greenSprite;
    public Sprite blueSprite;
    public Sprite bombSprite;
    public Sprite reverseSprite;
    public Sprite timeSlowSprite;

    public Sprite activeRedSprite;
    public Sprite activeGreenSprite;
    public Sprite activeBlueSprite;
    public Sprite activeBombSprite;
    public Sprite activeReverseSprite;
    public Sprite activeTimeSlowSprite;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Ball CreateBallAt(Vector3 point, BallType ballType)
    {
        Ball ball = Instantiate(ballPrefab, point, Quaternion.identity);
        ball.type = ballType;
        ball.state = BallState.SpawningOnTrack;
        SpriteRenderer spriteRenderer = ball.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetSpriteByType(ballType);

        return ball;
    }

    public Ball CreateRandomBallAt(Vector3 point)
    {
        return CreateBallAt(point, GetRandomBallColor());
    }

    private BallType GetRandomBallColor ()
    {
        return Colors[Random.Range(0, Colors.Length)];
    }

    private BallType GetRandomBallSpecialType()
    {
        return SpecialTypes[Random.Range(0, SpecialTypes.Length)];
    }

    public BallType GetRandomBallType()
    {
        return Random.Range(0f, 1f) > 0.2f ? GetRandomBallColor() : GetRandomBallSpecialType();
    }

    private Sprite GetSpriteByType(BallType type)
    {
        switch (type)
        {
            case BallType.Red:
                return redSprite;
                break;
            case BallType.Green:
                return greenSprite;
                break;
            case BallType.Blue:
                return blueSprite;
                break;
            case BallType.Bomb:
                return bombSprite;
                break;
            case BallType.Reverse:
                return reverseSprite;
                break;
            case BallType.TimeSlow:
                return timeSlowSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public Sprite GetActiveSpriteByType(BallType type)
    {
        switch (type)
        {
            case BallType.Red:
                return activeRedSprite;
                break;
            case BallType.Green:
                return activeGreenSprite;
                break;
            case BallType.Blue:
                return activeBlueSprite;
                break;
            case BallType.Bomb:
                return activeBombSprite;
                break;
            case BallType.Reverse:
                return activeReverseSprite;
                break;
            case BallType.TimeSlow:
                return activeTimeSlowSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
