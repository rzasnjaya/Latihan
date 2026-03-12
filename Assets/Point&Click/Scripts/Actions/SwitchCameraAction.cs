using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraAction : Actions
{
    [SerializeField] Camera cameraToSwitch;

    public override void Act()
    {
            CameraManager.Instance.SwitchCamera(cameraToSwitch);
    }
}
