using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera _virtualCamera;
    public Dictionary<string, float> fovs = new Dictionary<string, float>
    {
        {"default", 60f},
        {"run", 40f}
    };

    // Start is called before the first frame update
    void Start()
    {
        // Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _virtualCamera = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        Debug.Log(_virtualCamera.m_Lens.FieldOfView);
    }

    public void setFOV(string key)
    {
        if (!fovs.ContainsKey(key))
        {
            Debug.LogError("FOV key not found in dictionary");
            return;
        }
        // TODO: Fix FOV setting
        Debug.Log("Setting FOV to " + fovs[key]);
        _virtualCamera.m_Lens.FieldOfView = fovs[key];
        Debug.Log(_virtualCamera.m_Lens.FieldOfView);
    }
}
