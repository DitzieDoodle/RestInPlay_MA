using UnityEngine;

public class FlowerPickupable : MonoBehaviour
{
    public bool IsCarried { get; private set; }
    public VaseSlot CurrentSlot { get; private set; }

    private Rigidbody rb;
    private Collider col;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public AudioSource pickUp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        originalParent = transform.parent;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerFlowerCarry player = other.GetComponent<PlayerFlowerCarry>();
        if (player != null)
        {
            player.SetNearbyFlower(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerFlowerCarry player = other.GetComponent<PlayerFlowerCarry>();
        if (player != null)
        {
            player.ClearNearbyFlower(this);
        }
    }

    public void PickUp(Transform handPoint)
    {
        IsCarried = true;
        CurrentSlot = null;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Collider bleibt aktiv (nur Trigger), damit Enter/Exit weiter zuverlässig feuern.

        transform.SetParent(handPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (pickUp != null)
            pickUp.Play();
    }

    public void DropToWorld()
    {
        IsCarried = false;
        CurrentSlot = null;

        transform.SetParent(originalParent);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (col != null)
            col.enabled = true;
    }

    public void SnapToSlot(VaseSlot slot)
    {
        IsCarried = false;
        CurrentSlot = slot;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.SetParent(slot.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void RemoveFromSlot()
    {
        if (CurrentSlot != null)
        {
            CurrentSlot.occupied = false;
            CurrentSlot = null;
        }
    }
}