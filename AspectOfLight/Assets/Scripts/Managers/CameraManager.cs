using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineVirtualCamera defaultCamera;

    private void OnEnable()
    {
        EventManager.StartListening("onCameraShake", OnCameraShake);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onCameraShake", OnCameraShake);
    }

    public void SetUpDefaultCamera()
    {
        defaultCamera = GameObject
            .FindGameObjectWithTag("DefaultCamera")
            .GetComponent<CinemachineVirtualCamera>();

        defaultCamera.Follow = GameManager.Instance.playerReference.transform;
    }

    private void OnCameraShake(Dictionary<string, object> message) { }
}
