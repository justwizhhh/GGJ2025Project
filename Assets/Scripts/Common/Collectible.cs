using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Base class for all collectibles in the game
    public float HomingRadius;
    public float HomingDamping;

    private bool isHomingIn;

    // Object references
    private Collider col;
    private Rigidbody rb;
    private Animation anim;

    private Transform playerPos;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        try
        {
            anim = GetComponent<Animation>();
        }
        catch 
        { 
            // No anim, oh well...
        }
    }

    private void Start()
    {
        playerPos = FindFirstObjectByType<PlayerController>().transform;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.UpdateScore();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(playerPos.position, rb.position);
        if (distance <= 3.0f)
        {
            isHomingIn = true;
        }

        if (isHomingIn)
        {
            rb.position = Vector3.MoveTowards(rb.position, playerPos.position, HomingDamping);
        }
    }
}
