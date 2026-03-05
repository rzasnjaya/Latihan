using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] float distancePosition = 1f;
    public Vector3 InteractPosition()
    {
        return transform.position + transform.forward * distancePosition;
    }

    public void Interact(PlayerScript player)
    {
        Debug.Log(gameObject.name + "Clicked by Player");

        StartCoroutine(WaitForPlayerArriving(player));
    }

    IEnumerator WaitForPlayerArriving(PlayerScript player)
    {
        while (!player.CheckIfArrived())
        {
            yield return null;
        }

        Debug.Log("Player Arrived");
    }
}
