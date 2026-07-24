using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// Steuert das Wegwerfen und Zurückstellen des Spieler-Fotos in der Wut-Phase.
/// ThrowPicture() wird direkt aus dem Dialogsystem aufgerufen, sobald im
/// Dialogtext die entsprechende Stelle erreicht wird (z.B. über ein
/// Dialog-Event oder einen Inline-Trigger).
/// </summary>
public class PictureThrowController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Das Transform des hochgeladenen Fotos (Sprite/Frame in der Welt)")]
    [SerializeField] private Transform pictureTransform;

    [Tooltip("Der 'Ehrenplatz', an den das Bild zurückgestellt wird")]
    [SerializeField] private Transform honorSpot;

    [Header("Wurfziele")]
    [Tooltip("Mögliche Positionen, an die der NPC das Bild wirft. Wird zufällig ausgewählt.")]
    [SerializeField] private Transform[] throwTargets;

    [Header("Wurf-Einstellungen")]
    [SerializeField] private float jumpPower = 2.5f;
    [SerializeField] private float throwDuration = 0.6f;
    [SerializeField] private Vector2 rotationRange = new Vector2(180f, 540f);

    [Header("Zurückstellen-Einstellungen")]
    [SerializeField] private float placeBackDuration = 0.8f;

    [Header("Eskalation (optional)")]
    [Tooltip("Zählt, wie oft der NPC das Bild schon weggeworfen hat - nützlich für unterschiedliche 'Ausreden' im Dialog")]
    [SerializeField] private int throwCount = 0;
    public int ThrowCount => throwCount;

    [Header("Events")]
    [Tooltip("Wird ausgelöst, sobald der Wurf beginnt (z.B. für Sound/Kamera-Shake)")]
    public UnityEvent OnPictureThrown;

    [Tooltip("Wird ausgelöst, sobald das Bild aufkommt (z.B. für Impact-Sound/Partikel)")]
    public UnityEvent OnPictureLanded;

    [Tooltip("Wird ausgelöst, sobald das Bild wieder zurückgestellt wurde")]
    public UnityEvent OnPicturePlacedBack;

    private Sequence currentSequence;
    private Transform lastTarget;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    private Transform initialParent;

    public Vector3 InitialPosition => initialPosition;
    public Quaternion InitialRotation => initialRotation;
    public Vector3 InitialScale => initialScale;
    public Transform InitialParent => initialParent;

    private void Awake()
    {
        initialPosition = pictureTransform.position;
        initialRotation = pictureTransform.rotation;
        initialScale = pictureTransform.localScale;
        initialParent = pictureTransform.parent;
    }

    /// <summary>
    /// Setzt das Bild exakt auf die beim Start gemerkten Ausgangswerte zurück
    /// (Position, Rotation inkl. 80°-Neigung, Scale, Parent) - ohne Tween, für Sofort-Reset.
    /// </summary>
    public void SnapToInitialTransform()
    {
        pictureTransform.SetParent(initialParent, true);
        pictureTransform.position = initialPosition;
        pictureTransform.rotation = initialRotation;
        pictureTransform.localScale = initialScale;
    }

    /// <summary>
    /// Aus dem Dialogsystem aufrufen, sobald der NPC das Bild wegwirft.
    /// Wählt zufällig eines der throwTargets aus (nicht zweimal hintereinander dasselbe).
    /// </summary>
    public void ThrowPicture()
    {
        if (throwTargets == null || throwTargets.Length == 0)
        {
            Debug.LogWarning("PictureThrowController: Keine Wurfziele zugewiesen!");
            return;
        }

        currentSequence?.Kill();

        Transform target = GetRandomTarget();
        float randomRotation = Random.Range(rotationRange.x, rotationRange.y) * (Random.value > 0.5f ? 1f : -1f);

        currentSequence = DOTween.Sequence();
        currentSequence.Append(
            pictureTransform.DOJump(target.position, jumpPower, 1, throwDuration)
                .SetEase(Ease.OutQuad)
        );
        float startZ = pictureTransform.eulerAngles.z;
        float endZ = startZ + randomRotation;
        currentSequence.Join(
            DOTween.To(() => startZ, z =>
            {
                startZ = z;
                Vector3 e = initialRotation.eulerAngles;
                pictureTransform.rotation = Quaternion.Euler(e.x, e.y, z);
            }, endZ, throwDuration)
        );

        OnPictureThrown?.Invoke();

        currentSequence.OnComplete(() =>
        {
            // kleiner "Impact" beim Aufkommen - dezent, damit die Skalierung
            // nie in Richtung 0 oder negativ ausschlägt (sonst wirkt es wie
            // ein kurzes Verschwinden/Invertieren)
            pictureTransform.DOPunchScale(new Vector3(0.08f, 0.08f, 0f), 0.2f, 3, 0.3f);

            // Rotation exakt auf den gemerkten Ausgangswert zurücksetzen
            pictureTransform.DORotateQuaternion(initialRotation, 0.15f);

            throwCount++;
            OnPictureLanded?.Invoke();
        });

        currentSequence.Play();
    }

    /// <summary>
    /// Aus dem Dialog- oder Interaktionssystem aufrufen, wenn der Spieler
    /// das Bild wieder auf den Ehrenplatz stellt.
    /// </summary>
    public void PlacePictureBack()
    {
        currentSequence?.Kill();

        currentSequence = DOTween.Sequence();
        currentSequence.Append(
            pictureTransform.DOMove(initialPosition, placeBackDuration).SetEase(Ease.OutBack)
        );
        currentSequence.Join(
            pictureTransform.DORotateQuaternion(initialRotation, placeBackDuration)
        );
        currentSequence.OnComplete(() =>
        {
            // Exakter Snap am Ende, damit keine Tween-/Float-Ungenauigkeiten übrig bleiben
            SnapToInitialTransform();
            OnPicturePlacedBack?.Invoke();
        });

        currentSequence.Play();
    }

    private Transform GetRandomTarget()
    {
        if (throwTargets.Length == 1)
            return throwTargets[0];

        Transform target;
        do
        {
            target = throwTargets[Random.Range(0, throwTargets.Length)];
        }
        while (target == lastTarget);

        lastTarget = target;
        return target;
    }

    /// <summary>
    /// Optional: Eskalationsstufe zurücksetzen, z.B. beim Betreten der Phase.
    /// </summary>
    public void ResetEscalation()
    {
        throwCount = 0;
        lastTarget = null;
    }
}