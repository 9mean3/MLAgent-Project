using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private CinemachineVirtualCamera _modelCam;

    public void SetModelCam()
    {
        _modelCam.Priority = 11;
    }

    public void SetGameCam()
    {
        _modelCam.Priority = 9;
    }
}
