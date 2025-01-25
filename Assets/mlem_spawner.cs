using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mlem_spawner : MonoBehaviour
{
    [SerializeField] public int enemies = 0;
    [SerializeField] public List<GameObject> enemies_obj;
    [SerializeField] public GameObject enemy;
    [SerializeField] bool has_run = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawn();
    }
    void spawn()
    {
        if (enemies <= 5 && !has_run)
        {
            GameObject new_enemy = Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity);
            enemies_obj.Add(new_enemy);
            new_enemy.GetComponent<mlem_enemy>().list_pos = enemies;
            enemies++;
            Debug.Log("woo");
            //enemies_obj[i] = Instantiate(enemy, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        }
        if (enemies >= 5)
        {
            has_run = true;
        }
            
    }
}
