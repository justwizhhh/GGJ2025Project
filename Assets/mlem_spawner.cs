using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mlem_spawner : MonoBehaviour
{
    [SerializeField] public int enemies = 0;
    [SerializeField] public List<GameObject> enemies_obj;
    [SerializeField] public GameObject enemy;
    [SerializeField] bool has_run = false;
    [SerializeField] GameObject player;
    [SerializeField] Vector3 spawnpos;

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
        if (enemies <= 5 && !has_run)
        {
            //instantiates a new enemy and adds it to a list
            GameObject new_enemy = Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity);
            enemies_obj.Add(new_enemy);
            //sets the spawner parent to the current spawner
            new_enemy.GetComponent<mlem_enemy>().spawner = gameObject;
            //randomizes the spawner based on the players location
            spawnpos = new Vector3(player.transform.position.x + Random.Range(-10.0f, 10.0f), player.transform.position.y + Random.Range(-10.0f, 10.0f), player.transform.position.z + Random.Range(-10.0f, 10.0f));
            new_enemy.transform.position = spawnpos;
            enemies++;
            Debug.Log("woo");
        }
        //only allows the spawner to run once(if removed it will spam spawn infinitely, add a timer cooldown for spawning????
        if (enemies >= 5)
        {
            has_run = true;
        }
    }
}
