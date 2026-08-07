using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class IdleFloatAnimation : MonoBehaviour
{
    [Header("Float (Position)")]
    [SerializeField] bool enableFloat = true;
    [SerializeField] float floatStrength = 10f;
    [SerializeField] float floatDuration = 2f;

    [Header("Rotation Wobble")]
    [SerializeField] bool enableRotation = true;
    [SerializeField] float rotationStrength = 5f;
    [SerializeField] float rotationDuration = 2.5f;

    [Header("Scale Pulse")]
    [SerializeField] bool enableScale = false;
    [SerializeField] float scaleStrength = 0.05f;
    [SerializeField] float scaleDuration = 1.8f;

    [Header("Randomization")]
    [SerializeField] bool randomizeStartOffset = true;
    [SerializeField] Ease ease = Ease.InOutSine;

    RectTransform rectTransform;
    Vector2 startAnchoredPos;
    Quaternion startRotation;
    Vector3 startScale;

    Tween floatTween;
    Tween rotationTween;
    Tween scaleTween;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startAnchoredPos = rectTransform.anchoredPosition;
        startRotation = rectTransform.localRotation;
        startScale = rectTransform.localScale;
    }

    void OnEnable()
    {
        StartAnimation();
    }

    void OnDisable()
    {
        KillTweens();
    }

    public void StartAnimation()
    {
        KillTweens();

        float startDelay = randomizeStartOffset ? Random.Range(0f, 1f) : 0f;

        if (enableFloat)
        {
            float targetY = startAnchoredPos.y + floatStrength;
            floatTween = rectTransform.DOAnchorPosY(targetY, floatDuration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(startDelay);
        }

        if (enableRotation)
        {
            rotationTween = rectTransform.DOLocalRotate(new Vector3(0f, 0f, rotationStrength), rotationDuration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(startDelay);
        }

        if (enableScale)
        {
            Vector3 targetScale = startScale * (1f + scaleStrength);
            scaleTween = rectTransform.DOScale(targetScale, scaleDuration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(startDelay);
        }
    }

    public void StopAnimation(bool resetToStart = true)
    {
        KillTweens();

        if (resetToStart)
        {
            rectTransform.anchoredPosition = startAnchoredPos;
            rectTransform.localRotation = startRotation;
            rectTransform.localScale = startScale;
        }
    }

    void KillTweens()
    {
        floatTween?.Kill();
        rotationTween?.Kill();
        scaleTween?.Kill();
    }
}