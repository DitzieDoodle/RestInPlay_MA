using UnityEngine;
using DG.Tweening;

public class NpcSquash : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Transform, das skaliert werden soll (z.B. Sprite-Child). Wenn leer, wird this.transform genutzt.")]
    public Transform targetTransform;

    [Header("Squash & Stretch Settings")]
    public float squashScaleY = 0.9f;   // „zusammengedrückt“
    public float stretchScaleY = 1.1f;  // „gestreckt“
    public float animationDuration = 0.2f;
    public Ease animationEase = Ease.InOutSine;

    private Vector3 originalScale;
    private Tween squashTween;

    private void Awake()
    {
        if (targetTransform == null)
            targetTransform = transform;

        originalScale = targetTransform.localScale;
    }

    // 👉 Aufrufen, wenn der NPC anfängt zu reden
    public void StartTalkingSquash()
    {
        // laufende Animation stoppen, falls vorhanden
        if (squashTween != null && squashTween.IsActive())
            squashTween.Kill();

        targetTransform.localScale = originalScale;

        // einfache Squash-Stretch-Loop:
        // 1) in Y squashen, in X leicht strecken
        // 2) wieder auf Original zurück
        // 3) Loop als Yoyo
        Vector3 squashScale = new Vector3(
            originalScale.x * (1f / squashScaleY), // breiter, wenn niedriger in Y
            originalScale.y * squashScaleY,
            originalScale.z
        );

        Vector3 stretchScale = new Vector3(
            originalScale.x * (1f / stretchScaleY),
            originalScale.y * stretchScaleY,
            originalScale.z
        );

        // Sequence: squash -> stretch -> zurück
        var seq = DOTween.Sequence();

        seq.Append(targetTransform.DOScale(squashScale, animationDuration).SetEase(animationEase));
        seq.Append(targetTransform.DOScale(stretchScale, animationDuration).SetEase(animationEase));
        seq.Append(targetTransform.DOScale(originalScale, animationDuration).SetEase(animationEase));

        seq.SetLoops(-1, LoopType.Yoyo); // endlos, bis du stoppst

        squashTween = seq;
    }

    // 👉 Aufrufen, wenn der NPC aufhört zu reden
    public void StopTalkingSquash()
    {
        if (squashTween != null && squashTween.IsActive())
            squashTween.Kill();

        targetTransform.localScale = originalScale;
    }
}