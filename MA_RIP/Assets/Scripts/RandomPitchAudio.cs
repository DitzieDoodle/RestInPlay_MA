using UnityEngine;

public class RandomPitchAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float pitchVariation = 0.05f; // ±5%

    public void PlayWithRandomPitch()
    {
        float originalPitch = audioSource.pitch;
        audioSource.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
        audioSource.Play();
        audioSource.pitch = originalPitch;
    }
}