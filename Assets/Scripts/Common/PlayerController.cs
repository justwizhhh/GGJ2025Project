using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Main gameplay logic for player

    // Public toggles
    [Space(10)]
    [Header("Movement Toggles")]
    public float MaxAccel;
    public float MaxSpeed;
    public float MaxSpinAccel;
    public float MaxSpinSpeed;
    public float MaxSpinTime;
    public float SpinDelayTime;
    public float MaxRotationDamping;
    public float MaxStrafeRotation;

    [Space(10)]
    [Header("Hurt/death Settings")]
    public int MaxHealth;
    public float HurtKnockbackForce;
    public float HurtCooldownTime;

    [Space(10)]
    [Header("Camera Toggles")]
    public CinemachineVirtualCamera VirtualCam;
    public float CameraFOV;
    public float CameraSpinFOV;
    public float CameraDeathDistance;
    public float MaxCameraSpeed;
    public float MaxCameraDamping;

    [Space(10)]
    [Header("Visual Settings")]
    public List<Material> HurtMaterials = new List<Material>();

    // Private variables
    private int health;

    private Quaternion strafeRotation;

    [HideInInspector] public bool isSpinning;
    private bool isSpinCooldown;

    private bool isHurtCooldown;
    public bool isDead;
    public bool isInvincible;

    private bool forwardInput;
    private bool backwardInput;
    private int strafeInput;
    private bool spinInput;
    private bool restartInput;

    // Object references
    private Collider2D col;
    private Rigidbody rb;
    private MeshRenderer mesh;
    private Animator anim;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody>();
        mesh = GetComponentInChildren<MeshRenderer>();
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
        health = MaxHealth;
        
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

    private IEnumerator HurtCooldown()
    {
        isHurtCooldown = true;
        yield return new WaitForSeconds(HurtCooldownTime);
        isHurtCooldown = false;
    }

    [ContextMenu("Hurt the squid")]
    public void OnHurt(Vector3 knockbackSource)
    {
        if (!isInvincible)
        {
            if (!isHurtCooldown)
            {
                rb.AddForce((rb.position - knockbackSource).normalized * HurtKnockbackForce, ForceMode.Impulse);

                health--;
                if (health <= 0)
                {
                    OnDeath();
                }
                else
                {
                    // Player gets brief invincibility time
                    StartCoroutine(HurtCooldown());
                }

                if (HurtMaterials.Count != 0)
                {
                    mesh.material = HurtMaterials[MaxHealth - health];
                }
            }
        }
    }

    [ContextMenu("The squid is no more...")]
    public void OnDeath()
    {
        isDead = true;
    }

    private void Update()
    {
        // Basic input
        if (!isDead)
        {
            forwardInput = Input.GetKey(KeyCode.W) ? true : false;
            backwardInput = Input.GetKey(KeyCode.S) ? true : false;
            strafeInput = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            spinInput = Input.GetKeyDown(KeyCode.Space) ? true : false;
        }
        else
        {
            restartInput = Input.GetKeyDown(KeyCode.Return) ? true : false;
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

        // Respawning when the player dies
        if (restartInput)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        // Basic forwards and backwards movement
        if (!isDead)
        {
            if (forwardInput || backwardInput)
            {
                strafeRotation.eulerAngles = VirtualCam.transform.forward + new Vector3(0, MaxStrafeRotation * strafeInput, 0);
                rb.rotation = Quaternion.Slerp(
                        rb.rotation,
                        VirtualCam.transform.rotation * strafeRotation,
                        Time.deltaTime / MaxRotationDamping);

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
}
