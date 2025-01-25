using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Base class for all harmful obstacles in the game
    [SerializeField] public GameObject spawner;
    [SerializeField] int health = 100;
    [SerializeField] GameObject player;
    [SerializeField] bool can_despawn;
    [SerializeField] Collider despawn_radius;
    [SerializeField] Collider attack_radius;
    // Start is called before the first frame update
    [SerializeField] float despawn_timer = 1000.0f;

    [Space(10)]
    [Header("Dom's Code")]
    public float MoveSpeed;


    // Object references
    private Collider col;
    private Rigidbody rb;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
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

    private void FixedUpdate()
    {
        //calculates distance between player and enemies
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        //Debug.Log(distance);
        //checks if out of range and slowly despawns
        if (distance >= 12.1f)
        {
            can_despawn = true;
        }
        //checks if in range to home towards the player
        if (distance <= 12.0f)
        {
            movement();
        }
    }

    //moves towards the player
    void movement()
    {
        rb.position = Vector3.MoveTowards(rb.position, player.transform.position, MoveSpeed * Time.deltaTime);
    }
    void detection()
    {
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

    private void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.OnHurt(transform.position);
        }
    }
}
