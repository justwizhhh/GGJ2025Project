using UnityEngine;

public class PlayerAnimSound : MonoBehaviour
{
    public void PlaySwimSound()
    {
        StartCoroutine(AudioManager.instance.PlaySound("swim", 1));
    }
}
