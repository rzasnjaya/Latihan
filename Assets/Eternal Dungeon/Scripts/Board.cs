using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class Board : MonoBehaviour
{
    public BallSlot ballSlotPrefab;

    private PathCreator pathCreator;
    void Start()
    {
        pathCreator = FindObjectOfType<PathCreator>();

        InitBallSlots();
    }

    private void InitBallSlots()
    {
        float pathLength = pathCreator.path.length;
        float slotsCount = (int) pathLength;
        float step = pathLength / slotsCount;

        for (int i = 0; i < slotsCount; i++)
        {
            float distanceTraveled = i * step;
            Vector3 slotPos = pathCreator.path.GetPointAtDistance(i);
            BallSlot ballSlot = Instantiate(ballSlotPrefab, slotPos, Quaternion.identity);
            ballSlot.distanceTraveled = distanceTraveled;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
