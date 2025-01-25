using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Global Variables")]
    public int Score;
    
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore()
    {
        Score++;
        // Update HUD, etc etc
    }

    public void EndGame()
    {

    }
}
