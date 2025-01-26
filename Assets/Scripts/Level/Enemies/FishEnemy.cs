using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemy : Enemy
{
    // A basic fish enemy that minds its own business
    public float MaxXRot;
    public float MinXRot;

    [Space(10)]
    public GameObject Tapioca;

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

    public new void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player.isSpinning)
            {
                Rigidbody tapioca = Instantiate(Tapioca, rb.position, Quaternion.identity).GetComponent<Rigidbody>();

                Vector3 randomDir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
                tapioca.transform.position = transform.position + (randomDir * 2);
                tapioca.AddForce(randomDir * 5, ForceMode.Impulse);
                Destroy(gameObject);
            }
            else
            {
                player.OnHurt(transform.position);
            }
        }
    }
}
