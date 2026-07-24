using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// Verwaltet das Aufheben und Zurücktragen des Fotos durch den Spieler.
/// Arbeitet zusammen mit PictureThrowController: Nach dem Wurf (OnPictureLanded)
/// kann das Bild aufgehoben werden. Sobald es zum honorSpot getragen und dort
/// abgelegt wird, übernimmt wieder die DOTween-Platzierungs-Animation.
/// </summary>
[RequireComponent(typeof(PictureThrowController))]
public class PicturePickupController : MonoBehaviour
{
    public enum PictureState { OnFloor, Carried, Placed }

    [Header("References")]
    [SerializeField] private Transform pictureTransform;
    [SerializeField] private Transform playerTransform;

    [Tooltip("Leeres Child-Objekt am Spieler, an dem das Bild beim Tragen 'klebt' (z.B. vor der Brust)")]
    [SerializeField] private Transform holdPoint;

    [SerializeField] private Transform honorSpot;

    [Header("Interaktion")]
    [SerializeField] private float pickupRange = 1.2f;
    [SerializeField] private float placeRange = 1.2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Timing")]
    [SerializeField] private float pickupSnapDuration = 0.25f;
    [SerializeField] private float placeBackDuration = 0.8f;

    [Header("Events (z.B. für UI-Prompts 'E zum Aufheben')")]
    public UnityEvent OnShowPickupPrompt;
    public UnityEvent OnHidePickupPrompt;
    public UnityEvent OnShowPlacePrompt;
    public UnityEvent OnHidePlacePrompt;
    public UnityEvent OnPictureCarried;

    private PictureState currentState = PictureState.Placed;
    private PictureThrowController throwController;
    private bool pickupPromptVisible;
    private bool placePromptVisible;

    private void Awake()
    {
        throwController = GetComponent<PictureThrowController>();
    }

    private void OnEnable()
    {
        throwController.OnPictureLanded.AddListener(HandlePictureLanded);
    }

    private void OnDisable()
    {
        throwController.OnPictureLanded.RemoveListener(HandlePictureLanded);
    }

    private void HandlePictureLanded()
    {
        // Sobald der NPC das Bild geworfen hat, liegt es am Boden und kann aufgehoben werden
        currentState = PictureState.OnFloor;
    }

    private void Update()
    {
        switch (currentState)
        {
            case PictureState.OnFloor:
                HandlePickupCheck();
                break;

            case PictureState.Carried:
                HandlePlaceCheck();
                break;
        }
    }

    private void HandlePickupCheck()
    {
        bool inRange = Vector3.Distance(playerTransform.position, pictureTransform.position) <= pickupRange;

        if (inRange && !pickupPromptVisible)
        {
            OnShowPickupPrompt?.Invoke();
            pickupPromptVisible = true;
        }
        else if (!inRange && pickupPromptVisible)
        {
            OnHidePickupPrompt?.Invoke();
            pickupPromptVisible = false;
        }

        if (inRange && Input.GetKeyDown(interactKey))
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        currentState = PictureState.Carried;

        if (pickupPromptVisible)
        {
            OnHidePickupPrompt?.Invoke();
            pickupPromptVisible = false;
        }

        // sanftes "Einfangen" in die Hand statt hartem Snap
        pictureTransform.DOKill();
        pictureTransform.SetParent(holdPoint, true);
        pictureTransform.DOLocalMove(Vector3.zero, pickupSnapDuration).SetEase(Ease.OutQuad);
        pictureTransform.DOLocalRotate(Vector3.zero, pickupSnapDuration);

        OnPictureCarried?.Invoke();
    }

    private void HandlePlaceCheck()
    {
        bool inRange = Vector3.Distance(playerTransform.position, honorSpot.position) <= placeRange;

        if (inRange && !placePromptVisible)
        {
            OnShowPlacePrompt?.Invoke();
            placePromptVisible = true;
        }
        else if (!inRange && placePromptVisible)
        {
            OnHidePlacePrompt?.Invoke();
            placePromptVisible = false;
        }

        if (inRange && Input.GetKeyDown(interactKey))
        {
            PlaceBack();
        }
    }

    private void PlaceBack()
    {
        currentState = PictureState.Placed;

        if (placePromptVisible)
        {
            OnHidePlacePrompt?.Invoke();
            placePromptVisible = false;
        }

        // aus der Hand lösen, Weltposition beibehalten, dann sauber einschweben lassen
        pictureTransform.DOKill();
        pictureTransform.SetParent(null, true);

        Sequence placeSeq = DOTween.Sequence();
        placeSeq.Append(pictureTransform.DOMove(honorSpot.position, placeBackDuration).SetEase(Ease.OutBack));
        placeSeq.Join(pictureTransform.DORotate(Vector3.zero, placeBackDuration));
        placeSeq.OnComplete(() => throwController.OnPicturePlacedBack?.Invoke());
        placeSeq.Play();
    }

    private void OnDrawGizmosSelected()
    {
        if (pictureTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pictureTransform.position, pickupRange);
        }
        if (honorSpot != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(honorSpot.position, placeRange);
        }
    }
}