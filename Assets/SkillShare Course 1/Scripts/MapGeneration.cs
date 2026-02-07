using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject[] maptiles;
    public Transform tilestorage;
    public float offsetx;
    public float miny, maxy;
    public float SpawnPos;
    public float SpawnFreq;
    // Start is called before the first frame update
    void Start()
    {
        SpawnMapPiece();
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.x - SpawnPos) >= SpawnFreq)
        SpawnMapPiece();
    }

    public void SpawnMapPiece()
    {
        SpawnPos = transform.position.x;
        GameObject tile = maptiles[Random.Range(0, maptiles.Length)];
        GameObject spawnedtile = Instantiate(tile) as GameObject;
        spawnedtile.transform.SetParent(tilestorage);
        spawnedtile.transform.position = new Vector3(gameObject.transform.position.x + offsetx, Random.Range(miny, maxy), 0);  ;
    }
}
