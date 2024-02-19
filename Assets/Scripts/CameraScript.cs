using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineCam;

    void OnEnable()
    {
        EventManager.getCinemachine += GetCinemachine;
    }

    void OnDisable()
    {
        EventManager.getCinemachine -= GetCinemachine;
    }

    private CinemachineVirtualCamera GetCinemachine()
    {
        return cinemachineCam;
    }
}
