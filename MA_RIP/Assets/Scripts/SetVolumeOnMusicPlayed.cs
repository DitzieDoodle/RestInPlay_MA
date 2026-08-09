using UnityEngine;
using DG.Tweening;

public class SetVolumeOnMusicPlayed : MonoBehaviour
{
    public AudioSource musicSource;
    public float volumeOnMusicPlayed = 0.1f;
    public float tweenDuration = 1f;

    float initialVolume;

    void Start()
    {
        if (musicSource == null)
        {
            musicSource = GetComponentInChildren<AudioSource>();
        }

        if (musicSource == null)
        {
            Debug.LogError("No AudioSource found on this GameObject or its children.");
            return;
        }

        initialVolume = musicSource.volume;
        var musicBoxManager = FindAnyObjectByType<MusicBoxManager>();
        if (musicBoxManager != null)
        {
            musicBoxManager.OnMusicPlayed.AddListener(SetVolume);
            musicBoxManager.OnMusicStopped.AddListener(ResetVolume);
        }
    }
    private void SetVolume()
    {
        if (musicSource != null)
        {
            musicSource.DOKill();
            musicSource.DOFade(volumeOnMusicPlayed, tweenDuration);
        }
    }
    private void ResetVolume()
    {
        if (musicSource != null)
        {
            musicSource.DOKill();
            musicSource.DOFade(initialVolume, tweenDuration);
        }
    }
}
