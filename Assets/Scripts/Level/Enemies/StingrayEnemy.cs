using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingrayEnemy : FishEnemy
{
    public override void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.OnHurt(transform.position);
        }
    }
}
