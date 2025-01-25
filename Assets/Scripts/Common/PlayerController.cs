using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // Main gameplay logic for player

    // Public toggles
    [Header("Movement Toggles")]
    public float MaxMoveForce;
    public float MaxSpeed;
    public float MaxRotationDamping;

    [Space(10)]
    [Header("Camera Toggles")]
    public float MaxCameraSpeed;

    // Private variables
    private bool forwardInput;
    private bool backwardInput;

    // Object references
    private Rigidbody rb;
    private CinemachineVirtualCamera virtualCam;

    private Vector2 lastMousePos;
    private Vector2 currentMousePos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        virtualCam = FindFirstObjectByType<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        virtualCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = MaxCameraSpeed;
        virtualCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = MaxCameraSpeed;
    }

    void Update()
    {
        // Basic forwards and backwards movement
        forwardInput = Input.GetKey(KeyCode.W) ? true : false;
        backwardInput = Input.GetKey(KeyCode.S) ? true : false;

        if (forwardInput || backwardInput)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, virtualCam.transform.rotation, Time.deltaTime / MaxRotationDamping);
            if (rb.velocity.sqrMagnitude <= MaxSpeed)
            {
                rb.AddForce(transform.forward * (forwardInput ? 1 : backwardInput ? -1 : 0) * MaxMoveForce, ForceMode.Acceleration);
            }
        }
    }
}
