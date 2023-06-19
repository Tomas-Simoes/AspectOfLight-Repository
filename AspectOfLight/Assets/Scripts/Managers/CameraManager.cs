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

    private void OnCameraShake(Dictionary<string, object> message)
    {
        StartCoroutine(
            ShakeCamera(
                CinemachineCore.Instance
                    .GetActiveBrain(0)
                    .ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineStateDrivenCamera>()
                    .LiveChild.VirtualCameraGameObject.gameObject.GetComponent<CinemachineVirtualCamera>()
                    .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>()
            )
        );
    }

    private IEnumerator ShakeCamera(CinemachineBasicMultiChannelPerlin cameraPerlinNoise)
    {
        cameraPerlinNoise.m_AmplitudeGain = 1;
        cameraPerlinNoise.m_FrequencyGain = 1;

        yield return new WaitForSeconds(0.5f);

        cameraPerlinNoise.m_AmplitudeGain = 0;
        cameraPerlinNoise.m_FrequencyGain = 0;
    }

    public void ChangeCamera(Animator cameraAnimator, string conditionName, bool condition)
    {
        cameraAnimator.SetBool(conditionName, condition);
    }
}
