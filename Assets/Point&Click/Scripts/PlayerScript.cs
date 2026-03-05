using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClick();
    }

    void OnClick()
    {
        Debug.Log("Left Clicked");

        RaycastHit hit;
        Ray camToScreen = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camToScreen, out hit,Mathf.Infinity))
        {
            if (hit.collider != null)
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    MovePlayer(interactable.InteractPosition());
                    interactable.Interact(this);
                }
                else
                {
                    MovePlayer(hit.point);
                }                    
            }
        }
    }

    public bool CheckIfArrived()
    {
        return (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }

    void MovePlayer(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }
}
