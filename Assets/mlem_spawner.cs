using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mlem_spawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemies = new List<GameObject>();
    [SerializeField] public List<GameObject> enemies_obj = new List<GameObject>();
    [SerializeField] bool has_run = false;
    [SerializeField] GameObject player;
    [SerializeField] Vector3 spawnpos;

    [Space(10)]
    public float MaxSpawnCount;
    public float minDistance;
    public float spawnDistance;
    public float despawnDistance;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        spawn();
    }
    void spawn()
    {
        enemies_obj.RemoveAll(item => item == null);
        if (enemies_obj.Count <= MaxSpawnCount && !has_run)
        {
            //instantiates a new enemy and adds it to a list
            GameObject new_enemy = Instantiate(enemies[Random.Range(0, enemies.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            enemies_obj.Add(new_enemy);
            //sets the spawner parent to the current spawner
            //randomizes the spawner based on the players location
            spawnpos = new Vector3(player.transform.position.x + Random.Range(-spawnDistance, spawnDistance), player.transform.position.y + Random.Range(-spawnDistance, spawnDistance), player.transform.position.z + Random.Range(-spawnDistance, spawnDistance));
            if (Vector3.Distance(spawnpos, player.transform.position) < minDistance)
            {
                player.transform.position += (spawnpos - player.transform.position);
            }
            new_enemy.transform.position = spawnpos;
            Debug.Log("woo");
        }
        //only allows the spawner to run once(if removed it will spam spawn infinitely, add a timer cooldown for spawning????
        if (enemies_obj.Count > MaxSpawnCount)
        {
            //has_run = true;
        }
    }
}
