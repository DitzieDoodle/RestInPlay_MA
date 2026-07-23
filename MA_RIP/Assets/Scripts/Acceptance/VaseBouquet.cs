using System.Collections.Generic;
using UnityEngine;

public class VaseBouquet : MonoBehaviour
{
    public VaseSlot[] slots;
    private bool bouquetCompleted;
    public GameObject acceptanceEnding;

    public bool IsLocked => bouquetCompleted;

    private void Awake()
    {
        foreach (var s in slots)
        {
            s.parentVase = this;
            Debug.Log($"[Vase Awake] slot={s.name}, id={s.GetInstanceID()}, occupied={s.occupied}");
        }
    }

    public bool IsFull
    {
        get
        {
            foreach (var slot in slots)
            {
                if (!slot.occupied)
                    return false;
            }
            return true;
        }
    }

    public bool TryAddFlower(FlowerPickupable flower)
    {
        if (flower == null) return false;
        if (IsFull) return false;

        List<VaseSlot> freeSlots = new List<VaseSlot>();

        foreach (var slot in slots)
        {
            if (!slot.occupied)
                freeSlots.Add(slot);
        }

        if (freeSlots.Count == 0)
            return false;

        VaseSlot chosen = freeSlots[Random.Range(0, freeSlots.Count)];
        chosen.occupied = true;

        flower.SnapToSlot(chosen);

        CheckBouquetCompleted();

        return true;
    }

    private void CheckBouquetCompleted()
    {
        if (bouquetCompleted) return;
        if (!IsFull) return;

        bouquetCompleted = true;
        OnBouquetCompleted();
    }

    private void OnBouquetCompleted()
    {
        acceptanceEnding.SetActive(true);
        Debug.Log("Bouquet complete!");
    }
}