using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBundle : MonoBehaviour
{
    [Header("All exploding objects in the bundle become children of this object!")]
    
    public float MaxExplosionForce;
    public float MinExplosionForce;

    private Rigidbody[] objects;

    private void Start()
    {
        objects = transform.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody obj in objects)
        {
            obj.isKinematic = true;
            obj.GetComponent<Collectible>().enabled = false;
        }

        transform.rotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            transform.DetachChildren();
            foreach (Rigidbody obj in objects)
            {
                obj.isKinematic = false;
                obj.GetComponent<Collectible>().enabled = true;
            }

            foreach (Rigidbody obj in objects)
            {
                obj.AddForce(
                    (obj.position - transform.position).normalized * Random.Range(MinExplosionForce, MaxExplosionForce),
                    ForceMode.Impulse);
            }

            Destroy(gameObject);
        }
    }
}
