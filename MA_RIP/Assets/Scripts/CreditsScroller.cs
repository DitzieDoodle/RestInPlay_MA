using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class CreditsScroller : MonoBehaviour, IScrollHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Scroll-Geschwindigkeit")]
    [SerializeField] private float scrollSpeed = 30f;

    [Header("Manuelles Scrollen")]
    [SerializeField] private float resumeDelayAfterManualScroll = 2f;

    [Header("Ende erkennen")]
    public UnityEvent OnCreditsFinished;
    [SerializeField] private bool loop = false;

    private ScrollRect scrollRect;
    private RectTransform content;
    private RectTransform viewport;

    private bool userInteracting;
    private float resumeTimer;
    private bool finished;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
        viewport = scrollRect.viewport != null ? scrollRect.viewport : (RectTransform)transform;

        if (content == null)
        {
            Debug.LogError("CreditsScroller: Kein Content im ScrollRect zugewiesen! Bitte im Inspector setzen.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        // Erzwingt einen sofortigen Layout-Rebuild, damit content.rect.height
        // schon korrekt ist, bevor wir die Startposition setzen.
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        Vector2 pos = content.anchoredPosition;
        pos.y = -viewport.rect.height; // Content beginnt komplett unterhalb des sichtbaren Bereichs
        content.anchoredPosition = pos;
    }

    private void Update()
    {
        if (finished && !loop) return;

        if (userInteracting)
        {
            resumeTimer -= Time.deltaTime;
            if (resumeTimer <= 0f)
                userInteracting = false;

            return;
        }

        Vector2 pos = content.anchoredPosition;
        pos.y += scrollSpeed * Time.deltaTime;
        content.anchoredPosition = pos;

        CheckIfFinished();
    }

    private void CheckIfFinished()
    {
        float maxScroll = content.rect.height;

        if (content.anchoredPosition.y >= maxScroll)
        {
            if (loop)
            {
                Vector2 pos = content.anchoredPosition;
                pos.y = -viewport.rect.height;
                content.anchoredPosition = pos;
            }
            else if (!finished)
            {
                finished = true;
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, maxScroll);
                OnCreditsFinished?.Invoke();
            }
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        RegisterManualInteraction();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RegisterManualInteraction();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RegisterManualInteraction();
    }

    private void RegisterManualInteraction()
    {
        userInteracting = true;
        resumeTimer = resumeDelayAfterManualScroll;
    }
}