using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAction : Actions
{
    [SerializeField] string sceneTarget;

    public override void Act()
    {
        DataManager.instance.SetPrevScene(SceneManager.GetActiveScene().name);

        DataManager.instance.LevelManager.SceneLoad(sceneTarget);
    }
}
