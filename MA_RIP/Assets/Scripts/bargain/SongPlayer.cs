using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Source & Clips")]
    public AudioSource audioSource; // Die AudioSource, die die Musik abspielt
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song3;

    [Header("Fading Settings")]
    public float fadeDuration = 1f; // Dauer des Überblendens

    private Coroutine fadeCoroutine;

    // Methode für Song 1
    public void PlaySong1()
    {
        ChangeSong(song1);
    }

    // Methode für Song 2
    public void PlaySong2()
    {
        ChangeSong(song2);
    }

    // Methode für Song 3
    public void PlaySong3()
    {
        ChangeSong(song3);
    }

    // Interne Methode zum Wechseln mit Fade
    private void ChangeSong(AudioClip newClip)
    {
        if (audioSource.clip == newClip)
            return; // Schon der gleiche Song, nichts tun

        // Stoppe alten Fade, falls noch aktiv
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutIn(newClip));
    }

    // Coroutine: Fade Out, Clip wechseln, Fade In
    private System.Collections.IEnumerator FadeOutIn(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        // Clip wechseln und abspielen
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, startVolume, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
