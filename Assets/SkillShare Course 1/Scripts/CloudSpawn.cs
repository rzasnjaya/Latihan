using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawn : MonoBehaviour
{
    public float miny, maxy;
    public float mintime, maxtime;
    public GameObject[] cloud;
    public float offset;
    public float minspeed, maxspeed;
    public Transform cloudbound;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(Random.Range(mintime, maxtime));
        SpawnCloud();
    }

    public void SpawnCloud()
    {
        GameObject e = Instantiate(cloud[Random.Range(0, cloud.Length)]) as GameObject;
        e.transform.position = new Vector3(transform.position.x + offset, Random.Range(miny, maxy), 0);
        e.GetComponent<SkyObject>().vel = Random.Range(minspeed, maxspeed);
        e.GetComponent<SkyObject>().travright = false;
        e.GetComponent<SkyObject>().boundary = cloudbound;
        StartCoroutine(SpawnDelay());
    }
}
