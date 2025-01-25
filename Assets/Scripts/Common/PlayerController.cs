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
    public float CameraDeathDistance;
    public float MaxCameraSpeed;
    public float MaxCameraDamping;

    // Private variables
    private bool isSpinning;
    private bool isSpinCooldown;
    private bool isDead;

    private bool forwardInput;
    private bool backwardInput;
    private bool spinInput;

    // Object references
    private Collider2D col;
    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
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
        if (!isDead)
        {
            forwardInput = Input.GetKey(KeyCode.W) ? true : false;
            backwardInput = Input.GetKey(KeyCode.S) ? true : false;
            spinInput = Input.GetKeyDown(KeyCode.Space) ? true : false;
        }

        if (spinInput && !backwardInput)
        {
            if (!isSpinning && !isSpinCooldown)
            {
                StartCoroutine(Spin());
            }
        }

        // Camera stuff
        if (isSpinning)
        {
            VirtualCam.m_Lens.FieldOfView = Mathf.Lerp(VirtualCam.m_Lens.FieldOfView, CameraSpinFOV, MaxCameraDamping);
        }
        else
        {
            VirtualCam.m_Lens.FieldOfView = Mathf.Lerp(VirtualCam.m_Lens.FieldOfView, CameraFOV, MaxCameraDamping);
        }

        if (isDead)
        {
            float distance = VirtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
            VirtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 
                Mathf.Lerp(distance, CameraDeathDistance, MaxCameraDamping);
        }

        // Animation updating
        anim.SetBool("isMoving", forwardInput || backwardInput);
        anim.SetBool("isSpinning", isSpinning);
    }

    void FixedUpdate()
    {
        // Basic forwards and backwards movement
        if (forwardInput || backwardInput)
        {
            rb.rotation = Quaternion.Slerp(rb.rotation, VirtualCam.transform.rotation, Time.deltaTime / MaxRotationDamping);

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

    [ContextMenu("The squid is no more...")]
    public void OnDeath()
    {
        isDead = true;
    }    
}
