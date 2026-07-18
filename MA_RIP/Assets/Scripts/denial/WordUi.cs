using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WordUi : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] float snapDistance = 60f;
    [SerializeField] bool isMoveable = true;

    public bool IsRightWord = false;

    WordSlotUi parentSlot;
    RectTransform rectTransform;
    RectTransform parentSlotRectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;

    bool isDragging;
    bool isSnappedToParentSlot;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isMoveable)
        {
            return;
        }
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        isDragging = true;
        isSnappedToParentSlot = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isMoveable)
        {
            return;
        }

        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        isDragging = false;

        if (isSnappedToParentSlot && parentSlot != null)
        {
            parentSlot.OnWordSelected(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || !isMoveable)
        {
            return;
        }

        Vector3 targetWorldPosition;
        if (!TryGetPointerWorldPosition(eventData, out targetWorldPosition))
        {
            return;
        }

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

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Deny()
    {
        rectTransform.DOShakePosition(0.5f, 10f, 20, 90f, false, true).OnComplete(() =>
        {
            canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
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


    // void OnDrawGizmos()
    // {
    //     if (IsRightWord)
    //     {
    //         Gizmos.color = Color.green;
    //     }
    //     else
    //     {
    //         Gizmos.color = Color.red;
    //     }

    //     Gizmos.DrawWireCube(transform.position, transform.localScale);
    // }
}
