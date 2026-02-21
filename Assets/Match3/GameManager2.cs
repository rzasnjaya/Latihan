using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : Singleton<GameManager2>
{
    public Player player;

    public GameObject prefab;

    private void Start()
    {
        Instantiate(prefab);
        Instantiate(prefab);


        player = Player.Instance;

        print(player.health);
    }
}
