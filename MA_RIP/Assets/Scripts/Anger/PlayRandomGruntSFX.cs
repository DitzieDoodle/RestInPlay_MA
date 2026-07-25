using UnityEngine;

/// <summary>
/// Einfacher, wiederverwendbarer Random-SFX-Player.
/// PlayRandomSound() aus dem Dialogsystem, UnityEvents oder Code aufrufen.
/// </summary>
public class PlayRandomGruntSFX : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Falls leer, wird automatisch eine AudioSource an diesem GameObject gesucht/hinzugef�gt")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] clips;

    [Header("Variation (optional)")]
    [Tooltip("Leichte Tonh�hen-Variation pro Abspielung, damit es nicht repetitiv klingt")]
    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.95f, 1.05f);

    [Tooltip("Verhindert, dass derselbe Clip zweimal hintereinander kommt (nur ab 2+ Clips)")]
    [SerializeField] private bool avoidRepeats = true;

    private int lastIndex = -1;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    /// <summary>
    /// Spielt einen zuf�lligen Clip aus dem Array ab.
    /// Aus dem Dialogsystem, UnityEvents oder Code aufrufbar.
    /// </summary>
    public void PlayRandomSound()
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"{name}: RandomSoundPlayer hat keine Clips zugewiesen!");
            return;
        }

        int index = GetRandomIndex();
        AudioClip clip = clips[index];

        if (randomizePitch)
        {
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        }

        audioSource.PlayOneShot(clip);
        lastIndex = index;
    }

    /// <summary>
    /// Wie PlayRandomSound(), aber mit fester Lautst�rke (0-1) statt der
    /// AudioSource-Standardlautst�rke - praktisch f�r Feintuning im Dialog.
    /// </summary>
    public void PlayRandomSound(float volume)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"{name}: RandomSoundPlayer hat keine Clips zugewiesen!");
            return;
        }

        int index = GetRandomIndex();
        AudioClip clip = clips[index];

        if (randomizePitch)
        {
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        }

        audioSource.PlayOneShot(clip, volume);
        lastIndex = index;
    }

    private int GetRandomIndex()
    {
        if (clips.Length == 1 || !avoidRepeats)
        {
            return Random.Range(0, clips.Length);
        }

        int index;
        do
        {
            index = Random.Range(0, clips.Length);
        }
        while (index == lastIndex);

        return index;
    }
}