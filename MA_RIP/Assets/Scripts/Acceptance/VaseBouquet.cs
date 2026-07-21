using System.Collections.Generic;
using UnityEngine;

public class VaseBouquet : MonoBehaviour
{
    public VaseSlot[] slots;


    private void Awake()
    {
        foreach (var s in slots)
            Debug.Log(s.name + " -> " + s.gameObject.scene.name);
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

        flower.SnapToSlot(chosen.transform);

        return true;
    }
}