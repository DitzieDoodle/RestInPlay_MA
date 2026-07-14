using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicBoxManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip melancholic;
    public AudioClip acoustic;
    public AudioClip dreamy;
    public AudioClip hopeful;
    public AudioClip upbeat;

    [Header("AudioSources")]
    [SerializeField] private AudioSource source1;  // erste AudioSource (z.B. bestehende)
    [SerializeField] private AudioSource source2;  // zweite AudioSource (neu anlegen)


    [Header("Clips für Zufallsauswahl")]
    public List<AudioClip> randomClips = new List<AudioClip>();

    [Header("Settings")]
    public float fadeDuration = 2f;
    public float targetVolume = 0.8f;

    private AudioSource currentSource;
    private AudioSource nextSource;
    private Coroutine fadeCoroutine;

    private void Awake()
    {

        foreach (var src in new[] { source1, source2 })
        {
            src.playOnAwake = false;
            src.loop = true;
            src.volume = 0f;
        }

        currentSource = source1;
        nextSource = source2;

        DontDestroyOnLoad(gameObject); // optional: Musik über Szenen behalten
    }

    // Öffentliche Methoden für deine Dialog-Nodes
    public void PlayMelancholic()
    {
        PlayMusic(melancholic);
        Debug.Log("Play melancholic");
    }

    public void PlayAcoustic()
    {
        PlayMusic(acoustic);
        Debug.Log("Play acoustic");
    }

    public void PlayDreamy()
    {
        PlayMusic(dreamy);
        Debug.Log("Play dreamy");
    }

    public void PlayHopeful()
    {
        PlayMusic(hopeful);
        Debug.Log("Play hopeful");
    }

    public void PlayUpbeat()
    {
        PlayMusic(upbeat);
        Debug.Log("Play upbeat");
    }

    public void PlayRandomMusic()
    {
        if (randomClips == null || randomClips.Count == 0)
        {
            Debug.LogWarning("MusicManager: randomClips ist leer – kein Zufallsclip verfügbar.");
            return;
        }

        // Optional: vermeiden, dass direkt wieder der gleiche Clip gespielt wird
        AudioClip newClip = GetRandomClipAvoidingCurrent();

        PlayMusic(newClip);
        Debug.Log("Play random");
    }

    private AudioClip GetRandomClipAvoidingCurrent()
    {
        if (randomClips.Count == 1)
            return randomClips[0];

        AudioClip currentClip = currentSource.clip;

        // ziehe zufällig, bis ein anderer Clip gefunden ist (max. ein paar Versuche)
        for (int i = 0; i < 10; i++)
        {
            int index = Random.Range(0, randomClips.Count);
            AudioClip candidate = randomClips[index];

            if (candidate != null && candidate != currentClip)
                return candidate;
        }

        // fallback: nimm irgendeinen (auch wenn er gleich sein sollte)
        int fallbackIndex = Random.Range(0, randomClips.Count);
        return randomClips[fallbackIndex];
    }


    // Zentrale Logik
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("MusicManager: Clip ist null.");
            return;
        }

        // Wenn dieser Clip bereits läuft, nichts tun
        if (currentSource.clip == clip && currentSource.isPlaying)
            return;

        // evtl. laufende Fade-Coroutine stoppen
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CrossfadeTo(clip));
    }

    private IEnumerator CrossfadeTo(AudioClip newClip)
    {
        // nextSource vorbereiten
        nextSource.clip = newClip;
        nextSource.volume = 0f;
        nextSource.Play();

        float time = 0f;
        float startVolumeCurrent = currentSource.volume;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);

            // crossfade
            currentSource.volume = Mathf.Lerp(startVolumeCurrent, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, targetVolume, t);

            yield return null;
        }

        // Sicherstellen, dass wir sauber enden
        currentSource.Stop();
        currentSource.volume = 0f;

        // Rollen tauschen: next wird current
        var temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;

        fadeCoroutine = null;
    }
}