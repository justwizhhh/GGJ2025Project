using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class mlem_enemy : MonoBehaviour
{
    [SerializeField] public GameObject spawner;
    [SerializeField] int health = 100;
    [SerializeField] GameObject player;
    [SerializeField] bool can_despawn;
    [SerializeField] Collider despawn_radius;
    [SerializeField] Collider attack_radius;
    // Start is called before the first frame update
    [SerializeField] float despawn_timer = 1000.0f;
    void Start()
    {
        despawn_timer = 10.0f;
        despawn_timer = Random.Range(12.0f, 32.0f);
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        //checks if the enemy has no health left or despawn timer
        if (health <= 0 || despawn_timer <= 0.0f) 
        {
            spawner.GetComponent<mlem_spawner>().enemies_obj.RemoveAll(item => item == null); ;
            spawner.GetComponent<mlem_spawner>().enemies--;
            Destroy(gameObject);
        }
        detection();
    }
    //moves towards the player
    void movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.01f);
    }
    void detection()
    {
        //calculates distance between player and enemies
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        Debug.Log(distance);
        //checks if out of range and slowly despawns
        if (distance >= 12.1f)
        {
            can_despawn = true;
        }
        //checks if in range to home towards the player
        if(distance <= 12.0f)
        {
            movement();
        }
        //removes from despawn timer to slowly kill the enemy
        if (can_despawn)
        {
            despawn_timer -= (1.0f * Time.fixedDeltaTime);
        }
    }
    //damage variable for the player to hit
    void take_damage()
    {
        health -= 50;
    }
}
