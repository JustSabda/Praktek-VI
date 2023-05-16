using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimCamera : MonoBehaviour
{
    public static AimCamera Instance { get; private set; }

    //[SerializeField]
    //public PlayerInput playerInput;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;

    public bool isAim;


    private CinemachineFreeLook virtualCamera;
    //private InputAction aimAction;

    private void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineFreeLook>();
        //StartCoroutine(lagindicator());
    }
    /*
    IEnumerator lagindicator()
    {
        yield return new WaitForSeconds(1f);
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }
    */

    private void Update()
    {
        if (isAim)
        {
            StartAim();
        }
        else
        {
            CancelAim();
        }
    }

    private void StartAim()
    {
        virtualCamera.Priority = 20;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }

    private void CancelAim()
    {
        virtualCamera.Priority = 9;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
