using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class mlem_enemy : MonoBehaviour
{
    [SerializeField] GameObject spawner;
    [SerializeField] public int list_pos;
    [SerializeField] int health = 100;
    [SerializeField] GameObject player;
    [SerializeField] bool can_despawn;
    // Start is called before the first frame update
    [SerializeField] float despawn_timer = 1000.0f;
    void Start()
    {
        despawn_timer = 10.0f;
        despawn_timer = Random.Range(1.0f, 16.0f);
        can_despawn = true;
        player = GameObject.FindWithTag("Player");
        spawner = GameObject.FindWithTag("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 || despawn_timer <= 0.0f) 
        {
            spawner.GetComponent<mlem_spawner>().enemies_obj.RemoveAt(list_pos);
            spawner.GetComponent<mlem_spawner>().enemies--;
            Destroy(gameObject);
        }
        detection();
    }
    void movement()
    {
                     
    }
    void detection()
    {
        if(can_despawn)
        {
            despawn_timer -= (1.0f * Time.fixedDeltaTime);
        }
    }
    void take_damage()
    {
        health -= 50;
    }
}
