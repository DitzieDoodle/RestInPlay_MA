using UnityEngine;
using DG.Tweening;

/// <summary>
/// Lässt das GameObject sanft auf und ab schweben (DOTween Loop).
/// Einfach auf beliebige GameObjects legen.
/// </summary>
public class Levitate : MonoBehaviour
{
    [Header("Bewegung")]
    [Tooltip("Wie weit nach oben/unten geschwebt wird (Gesamtausschlag von der Startposition).")]
    [SerializeField] private float amplitude = 0.2f;
    [Tooltip("Dauer für eine Richtung (rauf ODER runter), nicht der volle Zyklus.")]
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private Ease ease = Ease.InOutSine;

    [Header("Variation")]
    [Tooltip("Zufälliger Start-Delay, damit mehrere Objekte nicht synchron schweben.")]
    [SerializeField] private bool randomizeStartOffset = true;

    [Header("Optional: leichtes Wackeln")]
    [SerializeField] private bool addRotationSway = false;
    [SerializeField] private float rotationSwayAngle = 3f;
    [SerializeField] private float rotationSwayDuration = 2f;

    private Vector3 startPos;
    private Tweener moveTween;
    private Tweener rotateTween;

    private void Start()
    {
        startPos = transform.localPosition;

        // Startet unterhalb der Ausgangsposition, damit der Yoyo-Loop
        // symmetrisch um startPos herum schwebt, statt nur nach oben.
        transform.localPosition = startPos - new Vector3(0f, amplitude, 0f);

        moveTween = transform
            .DOLocalMoveY(startPos.y + amplitude, duration)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);

        if (randomizeStartOffset)
        {
            moveTween.Goto(Random.Range(0f, duration), true);
        }

        if (addRotationSway)
        {
            rotateTween = transform
                .DORotate(new Vector3(0f, 0f, rotationSwayAngle), rotationSwayDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            if (randomizeStartOffset)
            {
                rotateTween.Goto(Random.Range(0f, rotationSwayDuration), true);
            }
        }
    }

    private void OnDestroy()
    {
        moveTween?.Kill();
        rotateTween?.Kill();
    }
}