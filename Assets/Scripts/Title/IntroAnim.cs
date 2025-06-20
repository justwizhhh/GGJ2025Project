using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroAnim : MonoBehaviour
{
    // Intro sequence script, for getting the player's input focus, and to play a titlecard animation before going into the title-screen

    public Animation TitleAnimation;
    Button startButton;

    private void Awake()
    {
        startButton = FindFirstObjectByType<Button>();
    }

    public void IntroStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        if (TitleAnimation != null)
        {
            TitleAnimation.Play();
            //StartCoroutine(AudioManager.instance.PlaySound());
            yield return new WaitForSeconds(TitleAnimation.clip.length);
        }
        SceneManager.LoadScene(1);
    }
}
