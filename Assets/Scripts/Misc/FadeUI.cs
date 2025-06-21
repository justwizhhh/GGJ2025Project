using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    public ParticleSystem FadeBubbles;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
        FadeBubbles.Play();
    }

    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }
}
