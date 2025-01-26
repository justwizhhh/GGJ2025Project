using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamEnemy : Enemy
{
    [Space(10)]
    public float ChaseDistance;

    private Transform playerPos;

    public override void Start()
    {
        base.Start();
        playerPos = FindFirstObjectByType<PlayerController>().transform;
    }

    public override void movement()
    {
        transform.LookAt(playerPos.position);

        float distance = Vector3.Distance(playerPos.position, rb.position);
        if (distance <= ChaseDistance)
        {
            rb.velocity = transform.forward * MoveSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public new void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.OnHurt(transform.position);
            Destroy(gameObject);
        }
    }
}
