using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Base class for all harmful obstacles in the game
    [SerializeField] public mlem_spawner spawner;
    [SerializeField] GameObject player;
    [SerializeField] bool can_despawn;
    [SerializeField] Collider attack_radius;
    // Start is called before the first frame update
    private float initialDespawnTimer;
    [SerializeField] float despawn_timer = 1000.0f;

    private float playerDistance;

    [Space(10)]
    [Header("Dom's Code")]
    public float MoveSpeed;


    // Object references
    protected Collider col;
    protected Rigidbody rb;
    protected MeshRenderer mesh;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        mesh = GetComponentInChildren<MeshRenderer>();
    }
    public virtual void Start()
    {
        spawner = FindFirstObjectByType<mlem_spawner>();
        initialDespawnTimer = Random.Range(12.0f, 32.0f);
        despawn_timer = initialDespawnTimer;
        player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    void Update()
    {
        //calculates distance between player and enemies
        playerDistance = Vector3.Distance(player.transform.position, rb.position);

        //checks if the enemy has no health left or despawn timer
        if (despawn_timer <= 0.0f)
        {
            Destroy(gameObject);
        }
        detection();
    }

    private void FixedUpdate()
    {
        //checks if out of range and slowly despawns
        if (playerDistance >= spawner.despawnDistance)
        {
            can_despawn = true;
            despawn_timer = initialDespawnTimer;
        }
        else
        {
            movement();
            can_despawn = false;

        }
    }

    //moves towards the player
    public virtual void movement()
    {
        rb.position = Vector3.MoveTowards(rb.position, player.transform.position, MoveSpeed * Time.deltaTime);
    }
    void detection()
    {
        //removes from despawn timer to slowly kill the enemy
        if (can_despawn)
        {
            despawn_timer -= Time.deltaTime;
        }
    }

    public void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.OnHurt(transform.position);
        }
    }
}
