using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraChanger : MonoBehaviour
{
    public GameObject cameraObj;

    private bool mainCameraActive;

    [SerializeField]
    PlayerInputControler playerInputs;

    private void Update()
    {
        if (playerInputs.changeCamera.WasPerformedThisFrame())
        {
            SwapCameras();
        }
    }

    public void SwapCameras()
    {
        if(mainCameraActive)
        {
            mainCameraActive = false;
            cameraObj.SetActive(true);
        }
        else
        {
            mainCameraActive = true;
            cameraObj.SetActive(false);
        }
       
    }
}
