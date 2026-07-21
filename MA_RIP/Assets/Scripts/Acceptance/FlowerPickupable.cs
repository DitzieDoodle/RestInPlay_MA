using UnityEngine;

public class FlowerPickupable : MonoBehaviour
{
    public bool IsCarried { get; private set; }

    private Rigidbody rb;
    private Collider col;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private VaseSlot currentSlot;

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

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (col != null)
            col.enabled = false;

        transform.SetParent(handPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        pickUp.Play();
    }

    public void DropToWorld()
    {
        IsCarried = false;

        if (currentSlot != null)
        {
            currentSlot.occupied = false;
            currentSlot = null;
        }

        transform.SetParent(null, true);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (col != null)
            col.enabled = true;
    }

    public void SnapToSlot(Transform slot)
    {
        IsCarried = false;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (col != null)
            col.enabled = false;

        currentSlot = slot.GetComponent<VaseSlot>();

        transform.SetParent(slot, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void RemoveFromSlot()
    {
        if (currentSlot != null)
        {
            currentSlot.occupied = false;
            currentSlot = null;
        }

        transform.SetParent(null, true);
        transform.localScale = Vector3.one;
    }
}