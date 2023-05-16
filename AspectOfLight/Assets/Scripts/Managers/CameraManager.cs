using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private void OnEnable()
    {
        EventManager.StartListening("onCameraShake", OnCameraShake);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onCameraShake", OnCameraShake);
    }

    private void OnCameraShake(Dictionary<string, object> message) { }
}
