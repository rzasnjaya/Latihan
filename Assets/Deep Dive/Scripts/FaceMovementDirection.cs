using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMovementDirection : MonoBehaviour
{
    private Vector3 previousPosiition;
    private Vector3 moveDirection;
    private Quaternion targetRotation;
    private float rotationSpeed = 200;

    void Start()
    {
        previousPosiition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        previousPosiition -= new Vector3(GameManager.Instance.adjustedWorldSpeed, 0);
        moveDirection = transform.position - previousPosiition;

        targetRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        previousPosiition = transform.position;
    }
}
