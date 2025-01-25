using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Base class for all collectibles in the game

    // Object references
    private Collider col;
    private Rigidbody rb;
    private Animation anim;

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            GameManager.Instance.UpdateScore();
        }
    }
}
