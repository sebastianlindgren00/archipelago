using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Dictionary<string, float> fovs = new Dictionary<string, float>
    {
        {"default", 50f},
        {"run", 30f}
    };
    [Header("Sensitivity")]
    [Range(0.1f, 10f)]
    public float mouseSensitivityX = 3f;
    [Range(0.1f, 5f)]
    public float mouseSensitivityY = 1f;
    private Vector2 defaultSensitivity;

    private CinemachineFreeLook _virtualCamera;
    public bool limitFPS = false;
    private const int targetFrameRate = 60;

    // Start is called before the first frame update
    void Start()
    {
        // Set the target frame rate (for when streaming the game)
        if (limitFPS)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;
        }

        // Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _virtualCamera = GetComponentInChildren<CinemachineFreeLook>();
        Debug.Log(_virtualCamera.m_Lens.FieldOfView);

        // Set the default FOV
        setFOV("default");

        // Set the default sensitivity
        defaultSensitivity = new Vector2(_virtualCamera.m_XAxis.m_MaxSpeed, _virtualCamera.m_YAxis.m_MaxSpeed);
    }

    void Update()
    {
        // Set the sensitivity 
        _virtualCamera.m_XAxis.m_MaxSpeed = defaultSensitivity.x * mouseSensitivityX;
        _virtualCamera.m_YAxis.m_MaxSpeed = defaultSensitivity.y * mouseSensitivityY;
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

        StartCoroutine(transitionFOV(fovs[key], fovs[key], duration));
    }

    IEnumerator transitionFOV(float startFOV, float endFOV, float duration)
    {
        if (duration == 0)
        {
            _virtualCamera.m_Lens.FieldOfView = endFOV;
            yield break;
        }

        float time = 0;
        while (time < duration)
        {
            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
    }

    public struct Range
    {
        public float start;
        public float end;

        public Range(float start, float end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
