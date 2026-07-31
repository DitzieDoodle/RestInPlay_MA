using UnityEngine;
using DG.Tweening;

/// <summary>
/// Lässt Wolken-Sprites (Children) gleichmäßig nach rechts scrollen und
/// endlos loopen, indem DOTween bei jedem Loop-Neustart auf die
/// Ausgangsposition zurückspringt (SetRelative + LoopType.Restart).
/// </summary>
public class CloudScroller : MonoBehaviour
{
    [Header("Wolken (Children-Sprites)")]
    [SerializeField] private Transform[] clouds;

    [Header("Bewegung")]
    [Tooltip("Wie weit jede Wolke wandert, bevor sie zurückspringt (Welteinheiten). Sollte der Abstand/Periode deines Wolken-Layouts entsprechen, siehe Erklärung unten.")]
    [SerializeField] private float travelDistance = 20f;
    [Tooltip("Geschwindigkeit in Units pro Sekunde.")]
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private bool scrollRight = true;

    private void Start()
    {
        float duration = travelDistance / scrollSpeed;
        float direction = scrollRight ? 1f : -1f;

        foreach (var cloud in clouds)
        {
            cloud.DOMoveX(travelDistance * direction, duration)
                .SetEase(Ease.Linear)
                .SetRelative()
                .SetLoops(-1, LoopType.Restart);
        }
    }

    private void OnDestroy()
    {
        foreach (var cloud in clouds)
        {
            if (cloud != null)
                DOTween.Kill(cloud);
        }
    }
}