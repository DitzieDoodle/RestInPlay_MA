using UnityEngine;
using DG.Tweening;

/// <summary>
/// Färbt das SpriteRenderer-Sprite eines NPCs von Schwarz zu einer im
/// Inspector gesetzten Farbe ein, sobald der Player in den Box Collider
/// tritt. Beim Verlassen fadet es zurück zu Schwarz.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class NpcColorReveal : MonoBehaviour
{
    [Header("Sprite")]
    [Tooltip("Der SpriteRenderer, dessen Farbe geändert wird. Leer lassen, um automatisch auf diesem GameObject zu suchen.")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Farben")]
    [SerializeField] private Color hiddenColor = Color.black;
    [Tooltip("Die Farbe, die sich zeigt, sobald der Player in der Nähe steht.")]
    [SerializeField] private Color revealColor = Color.white;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.6f;
    [SerializeField] private Ease fadeEase = Ease.InOutSine;

    [Header("Player Erkennung")]
    [SerializeField] private string playerTag = "Player";

    private BoxCollider zoneCollider;
    private Tweener colorTween;

    private void Awake()
    {
        zoneCollider = GetComponent<BoxCollider>();
        zoneCollider.isTrigger = true;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.color = hiddenColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        FadeTo(revealColor);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        FadeTo(hiddenColor);
    }

    private void FadeTo(Color target)
    {
        if (spriteRenderer == null) return;

        colorTween?.Kill();
        colorTween = spriteRenderer.DOColor(target, fadeDuration).SetEase(fadeEase);
    }

    private void OnDestroy()
    {
        colorTween?.Kill();
    }
}