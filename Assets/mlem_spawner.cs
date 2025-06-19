using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mlem_spawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemies = new List<GameObject>();
    [SerializeField] public List<GameObject> enemies_obj = new List<GameObject>();
    [SerializeField] GameObject player;
    [SerializeField] Vector3 spawnpos;

    [Space(10)]
    public float MaxSpawnCount;
    public float minDistance;
    public float spawnDistance;
    public float despawnDistance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // "Pre-spawn" all objects in the game before dynamically adding new ones during run-time
        for (int i = 0; i < MaxSpawnCount; i++)
        {
            spawn();
        }
    }

    private void Update()
    {
        if (enemies_obj.Count < MaxSpawnCount)
        {
            spawn();
            Debug.Log("dookie");
        }
    }

    void spawn()
    {
        enemies_obj.RemoveAll(item => item == null);

        //sets the spawner parent to the current spawner
        //randomizes the spawner based on the players location
        spawnpos = new Vector3(
            transform.position.x + Random.Range(-spawnDistance, spawnDistance),
            transform.position.y + Random.Range(-spawnDistance, spawnDistance),
            transform.position.z + Random.Range(-spawnDistance, spawnDistance));
        if (Vector3.Distance(spawnpos, player.transform.position) < minDistance)
        {
            player.transform.position += (spawnpos - player.transform.position);
        }

        //instantiates a new enemy and adds it to a list
        GameObject new_enemy = Instantiate(enemies[Random.Range(0, enemies.Count)], spawnpos, Quaternion.identity);
        enemies_obj.Add(new_enemy);
    }

    private void OnDrawGizmosSelected()
    {
        // debug draw for showing the distance range of enemy spawning
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.DrawSphere(transform.position, spawnDistance);
    }
}
