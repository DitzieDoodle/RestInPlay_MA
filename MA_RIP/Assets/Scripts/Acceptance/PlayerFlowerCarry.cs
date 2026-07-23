using UnityEngine;
public class PlayerFlowerCarry : MonoBehaviour
{
    public Transform handPoint;
    public FlowerPickupable carriedFlower;
    private FlowerPickupable nearbyFlower;
    public VaseDepositZone currentDepositZone;
    public AudioSource ding;
    public void SetNearbyFlower(FlowerPickupable flower)
    {
        nearbyFlower = flower;
    }
    public void ClearNearbyFlower(FlowerPickupable flower)
    {
        if (nearbyFlower == flower)
            nearbyFlower = null;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (carriedFlower == null)
            {
                if (nearbyFlower != null)
                    TryPickup(nearbyFlower);
            }
            else if (currentDepositZone != null)
            {
                DropCurrentFlowerToVase(currentDepositZone.vase);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropCurrentFlowerToWorld();
        }
    }
    public void TryPickup(FlowerPickupable flower)
    {
        Debug.Log($"[TryPickup] flower={flower?.name}, CurrentSlot={flower?.CurrentSlot?.name} (id={flower?.CurrentSlot?.GetInstanceID()})");
        if (carriedFlower != null) return;
        if (flower == null) return;
        if (flower.CurrentSlot != null)
        {
            VaseBouquet vase = flower.CurrentSlot.parentVase;
            Debug.Log($"[TryPickup] vase={vase?.name}, IsLocked={vase?.IsLocked}, slot.occupied before={flower.CurrentSlot.occupied}");
            if (vase != null && vase.IsLocked)
            {
                Debug.Log("[TryPickup] BLOCKED wegen IsLocked");
                return;
            }
            flower.RemoveFromSlot();
            Debug.Log("[TryPickup] RemoveFromSlot ausgeführt");
        }
        carriedFlower = flower;
        flower.PickUp(handPoint);
        nearbyFlower = null;
    }
    public void DropCurrentFlowerToVase(VaseBouquet vase)
    {
        if (carriedFlower == null) return;
        if (vase == null) return;

        bool accepted = vase.TryAddFlower(carriedFlower);

        if (accepted)
        {
            carriedFlower = null;
            if (ding != null)
                ding.Play();
        }
    }
    public void DropCurrentFlowerToWorld()
    {
        if (carriedFlower == null) return;
        carriedFlower.DropToWorld();
        carriedFlower = null;
    }
}