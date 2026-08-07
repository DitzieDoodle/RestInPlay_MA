using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class WordUi : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Movement")]
    [SerializeField] float snapDistance = 60f;
    [SerializeField] bool isMoveable = true;
    public bool IsRightWord = false;

    [Header("Audio Feedback")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip correctSfx;
    [SerializeField] AudioClip wrongSfx;

    [Header("Color Feedback")]
    [SerializeField] Image targetImage;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color hoverColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] Color dragColor = new Color(1f, 1f, 1f, 1f);

    public string GetWordText()
    {
        return wordText != null ? wordText.text : gameObject.name;
    }

    TMP_Text wordText;
    WordSlotUi parentSlot;
    RectTransform rectTransform;
    RectTransform parentSlotRectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;
    IdleFloatAnimation idleAnimation;
    bool isDragging;
    bool isHovering;
    bool isSnappedToParentSlot;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        wordText = GetComponentInChildren<TMP_Text>();
        idleAnimation = GetComponent<IdleFloatAnimation>();

        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        UpdateVisualState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isMoveable) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;

        // WICHTIG: Animation zuerst stoppen, bevor irgendwas anderes passiert
        if (idleAnimation != null)
        {
            idleAnimation.StopAnimation(false); // false = NICHT auf Startposition zurücksetzen
        }

        isDragging = true;
        isSnappedToParentSlot = false;
        UpdateVisualState();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isMoveable) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;

        isDragging = false;
        UpdateVisualState();

        if (isSnappedToParentSlot && parentSlot != null)
        {
            parentSlot.OnWordSelected(this);
        }
        else if (idleAnimation != null)
        {
            idleAnimation.StartAnimation();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UpdateVisualState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UpdateVisualState();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || !isMoveable) return;

        Vector3 targetWorldPosition;
        if (!TryGetPointerWorldPosition(eventData, out targetWorldPosition)) return;

        rectTransform.position = targetWorldPosition;

        if (parentSlotRectTransform == null)
        {
            isSnappedToParentSlot = false;
            return;
        }

        float distanceToParentSlot = Vector2.Distance(rectTransform.position, parentSlotRectTransform.position);
        isSnappedToParentSlot = distanceToParentSlot <= snapDistance;
        if (isSnappedToParentSlot)
        {
            rectTransform.position = parentSlotRectTransform.position;
        }
    }

    public void SetParentSlot(WordSlotUi slot)
    {
        parentSlot = slot;
        parentSlotRectTransform = slot != null ? slot.GetComponent<RectTransform>() : null;
    }

    public void PlayCorrectFeedback()
    {
        if (audioSource != null && correctSfx != null)
        {
            audioSource.PlayOneShot(correctSfx);
        }
    }

    public void PlayWrongFeedback()
    {
        if (audioSource != null && wrongSfx != null)
        {
            audioSource.PlayOneShot(wrongSfx);
        }
    }

    public void Deny()
    {
        PlayWrongFeedback();

        rectTransform.DOShakePosition(0.5f, 10f, 20, 90f, false, true).OnComplete(() =>
        {
            canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }

    void UpdateVisualState()
    {
        if (targetImage == null) return;

        if (isDragging)
        {
            targetImage.color = dragColor;
        }
        else if (isHovering)
        {
            targetImage.color = hoverColor;
        }
        else
        {
            targetImage.color = normalColor;
        }
    }

    bool TryGetPointerWorldPosition(PointerEventData eventData, out Vector3 worldPosition)
    {
        RectTransform canvasRect = canvas != null ? canvas.rootCanvas.transform as RectTransform : null;
        RectTransform referenceRect = canvasRect != null ? canvasRect : rectTransform;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(referenceRect, eventData.position, eventData.pressEventCamera, out worldPosition))
        {
            return true;
        }
        worldPosition = rectTransform.position;
        return false;
    }
}