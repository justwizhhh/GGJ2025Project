using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    PlayerInput playerInput;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(AudioManager.instance.PlaySound("spongebobBoowomp", 1));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            AudioManager.instance.PlayMusic("spongebobGrassSkirt", 1);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AudioManager.instance.StopMusic();
        }

        
    }

    void OnButton1(InputValue value)
    {
        
    }
}
