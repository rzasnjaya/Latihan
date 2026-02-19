using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFactory : MonoBehaviour
{
    public Ball ballPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Ball CreateBallAt(Vector3 point)
    {
        Ball ball = Instantiate(ballPrefab, point, Quaternion.identity);
        ball.state = BallState.Spawning;
        ball.transform.localScale = Vector3.zero;
        return ball;
    }
}
