using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamEnemy : Enemy
{
    [Space(10)]
    public float MaxMoveSpeed;
    public float MinChaseDistance;
    public float MaxChaseDistance;

    private float currentMoveSpeed;
    private float currentChaseDistance;
    private Transform playerPos;

    public override void Start()
    {
        base.Start();

        currentMoveSpeed = Random.Range(MoveSpeed, MaxMoveSpeed);
        currentChaseDistance = Random.Range(MinChaseDistance, MaxChaseDistance);
        playerPos = FindFirstObjectByType<PlayerController>().transform;
    }

    public override void movement()
    {
        transform.LookAt(playerPos.position);
        
        float distance = Vector3.Distance(rb.position, playerPos.position);
        if (distance <= currentChaseDistance)
        {
            rb.velocity = transform.forward * currentMoveSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.OnHurt(transform.position);
            Destroy(gameObject);
        }
    }
}
