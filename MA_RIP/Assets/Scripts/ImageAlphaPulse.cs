using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class ImageAlphaPulse : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Ease fadeEase = Ease.InOutSine;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private float startDelay = 0f;

    private Image _image;
    private Tween _fadeTween;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        if (playOnStart)
        {
            StartPulse();
        }
    }

    public void StartPulse()
    {
        StopPulse();

        // Sicherstellen, dass wir bei Alpha 1 starten
        Color c = _image.color;
        c.a = 1f;
        _image.color = c;

        _fadeTween = _image
            .DOFade(0f, fadeDuration)
            .SetEase(fadeEase)
            .SetLoops(-1, LoopType.Yoyo) // -1 = endlos, Yoyo = hin und zurück
            .SetDelay(startDelay);
    }

    public void StopPulse()
    {
        _fadeTween?.Kill();
    }

    private void OnDestroy()
    {
        _fadeTween?.Kill();
    }
}