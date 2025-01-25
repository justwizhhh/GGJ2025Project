using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Main Variables")]
    public int MaxScore;

    [Space(10)]
    [Header("Global Variables")]
    public int Score;

    // todo, find all tapiocas
    
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore()
    {
        Score++;
        // Update HUD, etc etc

        if (Score > MaxScore)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene(3);
    }
}
