using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Main Variables")]
    public int MaxScore;
    public float EndTransitionTime;

    [Space(10)]
    [Header("Global Variables")]
    public int Score;

    private bool isGameEnding;

    // Object references
    private PlayerController player;
    private UIManager uiManager;

    // todo, find all tapiocas
    
    public static GameManager Instance;
    [Space(10)]
    [Header("Sounds")]
    [SerializeField] AudioSource pickup;

    private void Awake()
    {
        Instance = this;

        player = FindFirstObjectByType<PlayerController>();
        uiManager = FindFirstObjectByType<UIManager>();
    }

    private void Start()
    {
        uiManager.InitScore(MaxScore);
        AudioManager.instance.PlayMusic("GGJ2025ProjectMusic", 1);
    }

    private void Update()
    {
        if (player.isDead)
        {
            uiManager.PlayDeathAnim();
        }
    }

    public void UpdateScore()
    {
        pickup.Play();
        Score++;
        uiManager.UpdateScore(Score);

        if (Score >= MaxScore)
        {
            EndGame();
        }
    }

    private IEnumerator EndGameTransition()
    {
        player.enabled = false;
        player.isInvincible = true;
        uiManager.PlayEndAnim();
        yield return new WaitForSeconds(EndTransitionTime);
        SceneManager.LoadScene(3);
    }

    public void EndGame()
    {
        if (!isGameEnding)
        {
            StartCoroutine(EndGameTransition());
        }
    }
}
