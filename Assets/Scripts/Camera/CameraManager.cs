using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook _virtualCamera;
    public Dictionary<string, float> fovs = new Dictionary<string, float>
    {
        {"default", 50f},
        {"run", 30f}
    };

    // Start is called before the first frame update
    void Start()
    {
        // Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _virtualCamera = GetComponentInChildren<CinemachineFreeLook>();

        // Set the default FOV
        setFOV("default");
    }

    public void setNoise(float amplitude, float frequency)
    {
        for (int i = 0; i < 3; i++)
        {
            _virtualCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
            _virtualCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        }
    }

    public void setFOV(string key, float duration = 0.2f)
    {
        if (!fovs.ContainsKey(key))
        {
            Debug.LogError("FOV key not found in dictionary");
            return;
        }

        StartCoroutine(transitionFOV(fovs[key], duration));
    }

    IEnumerator transitionFOV(float endFOV, float duration)
    {
        if (duration == 0)
        {
            _virtualCamera.m_Lens.FieldOfView = endFOV;
            yield break;
        }

        float startFOV = _virtualCamera.m_Lens.FieldOfView;
        float time = 0;
        while (time < duration)
        {
            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
    }
}
