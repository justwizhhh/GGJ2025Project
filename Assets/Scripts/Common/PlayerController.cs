using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    // Main gameplay logic for player

    // Public toggles
    [Header("Movement Toggles")]
    public float MaxAccel;
    public float MaxSpeed;
    public float MaxSpinAccel;
    public float MaxSpinSpeed;
    public float MaxSpinTime;
    public float SpinDelayTime;
    public float MaxRotationDamping;

    [Space(10)]
    [Header("Camera Toggles")]
    public CinemachineVirtualCamera VirtualCam;
    public float CameraFOV;
    public float CameraSpinFOV;
    public float MaxCameraSpeed;
    public float MaxCameraDamping;

    // Private variables
    private bool isSpinning;
    private bool isSpinCooldown;

    private bool forwardInput;
    private bool backwardInput;
    private bool spinInput;

    // Object references
    private Rigidbody rb;

    private Vector2 lastMousePos;
    private Vector2 currentMousePos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = false;
        VirtualCam.enabled = true;
    }

    private void OnApplicationPause(bool pause)
    {
        Cursor.visible = true;
        VirtualCam.enabled = false;
    }

    private void Start()
    {
        VirtualCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = MaxCameraSpeed;
        VirtualCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = MaxCameraSpeed;

        VirtualCam.m_Lens.FieldOfView = CameraFOV;
    }

    private IEnumerator Spin()
    {
        isSpinning = true;
        isSpinCooldown = true;
        yield return new WaitForSeconds(MaxSpinTime);
        isSpinning = false;
        StartCoroutine(SpinDelay());
    }

    private IEnumerator SpinDelay()
    {
        yield return new WaitForSeconds(SpinDelayTime);
        isSpinCooldown = false;
    }

    private void Update()
    {
        // Basic input
        forwardInput = Input.GetKey(KeyCode.W) ? true : false;
        backwardInput = Input.GetKey(KeyCode.S) ? true : false;
        spinInput = Input.GetKeyDown(KeyCode.Space) ? true : false;

        if (spinInput && !backwardInput)
        {
            if (!isSpinning && !isSpinCooldown)
            {
                StartCoroutine(Spin());
            }
        }

        // Camera stuff
        VirtualCam.enabled = Application.isFocused;

        if (isSpinning)
        {
            VirtualCam.m_Lens.FieldOfView = Mathf.Lerp(VirtualCam.m_Lens.FieldOfView, CameraSpinFOV, MaxCameraDamping);
        }
        else
        {
            VirtualCam.m_Lens.FieldOfView = Mathf.Lerp(VirtualCam.m_Lens.FieldOfView, CameraFOV, MaxCameraDamping);
        }
    }

    void FixedUpdate()
    {
        // Basic forwards and backwards movement
        if (forwardInput || backwardInput)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, VirtualCam.transform.rotation, Time.deltaTime / MaxRotationDamping);

            if (isSpinning)
            {
                if (rb.velocity.sqrMagnitude <= MaxSpinSpeed)
                {
                    rb.AddForce(transform.forward * MaxSpinAccel, ForceMode.Acceleration);
                }
            }
            else
            {
                if (rb.velocity.sqrMagnitude <= MaxSpeed)
                {
                    rb.AddForce(transform.forward * (forwardInput ? 1 : backwardInput ? -1 : 0) * MaxAccel, ForceMode.Acceleration);
                }
            }
        }
    }
}
