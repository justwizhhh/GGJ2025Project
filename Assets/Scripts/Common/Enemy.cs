using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Base class for all harmful obstacles in the game

    // Object references
    private Collider col;
    private Rigidbody rb;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.OnHurt(true, transform.position);
        }
    }
}
