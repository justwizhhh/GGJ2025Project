using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PufferfishEnemy : FishEnemy
{
    [Space(10)]
    public float PuffUpDistance; 
    public float PuffUpScale;
    public float PuffUpSpeed;
    public float PuffUpDetachForce;

    private bool isPuffedUp;
    private Vector3 originalScale;

    private List<Rigidbody> attachedObjects = new List<Rigidbody>();

    private Transform playerPos;
    
    public override void Start()
    {
        base.Start();
        originalScale = transform.localScale;
        playerPos = FindFirstObjectByType<PlayerController>().transform;

        attachedObjects = GetComponentsInChildren<Rigidbody>().ToList();
        foreach (var obj in attachedObjects)
        {
            if (obj == gameObject || obj == mesh.gameObject)
            {
                attachedObjects.Remove(obj);
                obj.isKinematic = true;
            }
        }
    }

    public override void movement()
    {
        base.movement();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(playerPos.position, rb.position);
        if (distance <= PuffUpDistance)
        {
            isPuffedUp = true;
        }

        if (isPuffedUp)
        {
            if (transform.localScale == originalScale)
            {
                foreach (var obj in attachedObjects)
                {
                    obj.transform.SetParent(null);
                    obj.isKinematic = false;

                    obj.AddForce(
                        (obj.position - transform.position).normalized * PuffUpDetachForce,
                        ForceMode.Impulse);

                    obj.position += (obj.position - transform.position).normalized * PuffUpScale * 2;
                }
            }
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * PuffUpScale, PuffUpSpeed);
        }
    }

    public new void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player.isSpinning)
            {
                if (isPuffedUp)
                {
                    player.OnHurt(transform.position);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                player.OnHurt(transform.position);
            }
        }
    }
}
