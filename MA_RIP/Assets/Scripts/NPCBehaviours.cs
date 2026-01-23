using UnityEngine;

public class DialogueSFX : MonoBehaviour
{
    public AudioSource sfxSource;

    public void PlaySFX()
    {
        if (sfxSource != null)
            sfxSource.Play();
    }
}