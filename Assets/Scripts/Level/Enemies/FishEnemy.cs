using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemy : Enemy
{
    // A basic fish enemy that minds its own business
    public float MaxXRot;
    public float MinXRot;

    public override void Start()
    {
        base.Start();
        rb.rotation = Quaternion.Euler(Random.Range(MinXRot, MaxXRot), Random.Range(-180, 180), 0);
    }

    public override void movement()
    {
        rb.AddForce(transform.forward * MoveSpeed, ForceMode.Force);
        // TODO - add swaying motion, coroutine moving the fish up and down
        //rb.AddForce(transform.up * Random.Range(-1, 1), ForceMode.Force);
    }

    public override void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player.isSpinning)
            {
                Destroy(gameObject);
            }
            else
            {
                player.OnHurt(transform.position);
            }
        }
    }
}
