using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSpawn : MonoBehaviour
{
    public float miny, maxy;
    public float mintime, maxtime;
    public GameObject eagle;
    public float offset;
    public float minspeed, maxspeed;
    public Transform eaglebound;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(Random.Range(mintime, maxtime));
        SpawnEagle();
    }

    public void SpawnEagle()
    {
        GameObject e = Instantiate(eagle) as GameObject;
        e.transform.position = new Vector3(transform.position.x + offset, Random.Range(miny, maxy), 0);
        e.GetComponent<SkyObject>().vel = Random.Range(minspeed, maxspeed);
        e.GetComponent<SkyObject>().travright = true;
        e.GetComponent<SkyObject>().boundary = eaglebound;
        StartCoroutine(SpawnDelay());
    }
}
