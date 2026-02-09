using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainScript : MonoBehaviour
{
    public Animator anim;


    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void FadeTo()
    {
        transform.position = GameObject.Find("Main Camera").transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        anim.SetBool("FadeOut", true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}