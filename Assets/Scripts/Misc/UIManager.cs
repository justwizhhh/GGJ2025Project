using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;

public class UIManager : MonoBehaviour
{
    public TMP_Text ScoreText;

    private int maxScore;
    private bool deathAnimPlayed;
    private bool endAnimPlayed;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void InitScore(int value)
    {
        if (ScoreText != null)
        {
            maxScore = value;
            ScoreText.text = 0 + "/" + maxScore.ToString();
        }
    }

    public void UpdateScore(int newValue)
    {
        if (ScoreText != null)
        {
            ScoreText.text = newValue.ToString() + "/" + maxScore.ToString();
        }
    }

    public void PlayDeathAnim()
    {
        if (!deathAnimPlayed)
        {
            anim.SetTrigger("Death");
            deathAnimPlayed = true;
        }
    }

    public void PlayEndAnim()
    {
        if (!endAnimPlayed)
        {
            anim.SetTrigger("End");
            endAnimPlayed = true;
        }
    }
}
