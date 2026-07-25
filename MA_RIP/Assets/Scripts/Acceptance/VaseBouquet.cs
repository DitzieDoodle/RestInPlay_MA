using System.Collections.Generic;
using UnityEngine;

public class VaseBouquet : MonoBehaviour
{
    public VaseSlot[] slots;
    public GameObject[] flowerPrefabs;
    private bool bouquetCompleted;
    public GameObject acceptanceEnding;

    public bool IsLocked => bouquetCompleted;

    public Dictionary<string, GameObject> identifierToFlowerPrefab = new Dictionary<string, GameObject>();
    public Dictionary<VaseSlot, string> slotToFlowerIdentifier = new Dictionary<VaseSlot, string>();

    private void Awake()
    {
        foreach (var s in slots)
        {
            s.parentVase = this;
            Debug.Log($"[Vase Awake] slot={s.name}, id={s.GetInstanceID()}, occupied={s.occupied}");
        }

        foreach (var prefab in flowerPrefabs)
        {
            FlowerPickupable flowerPickupable = prefab.GetComponent<FlowerPickupable>();
            if (flowerPickupable != null)
            {
                identifierToFlowerPrefab[flowerPickupable.Identifier] = prefab;
            }
        }
    }

    void Start()
    {
        LoadSavedState();
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

    private void LoadSavedState()
    {
        int counter = 0;
        foreach (var slot in slots)
        {
            string identifier = PlayerPrefs.GetString($"Vase_Slot_{counter}", null);
            if (!string.IsNullOrEmpty(identifier))
            {
                slotToFlowerIdentifier[slot] = identifier;
                slot.occupied = true;
                var flower = Instantiate(identifierToFlowerPrefab[identifier]).GetComponent<FlowerPickupable>();
                flower.SnapToSlot(slot);
            }
            counter++;
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
        slotToFlowerIdentifier[chosen] = flower.Identifier;

        int counter = 0;
        foreach (var slot in slots)
        {
            if (slot == chosen)
                break;
            counter++;
        }
        PlayerPrefs.SetString($"Vase_Slot_{counter}", flower.Identifier);
        PlayerPrefs.Save();

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