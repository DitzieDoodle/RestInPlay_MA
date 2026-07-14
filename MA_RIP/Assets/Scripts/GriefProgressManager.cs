using UnityEngine;
using System.Collections.Generic;

public class GriefProgressManager : MonoBehaviour
{
    public static GriefProgressManager Instance { get; private set; }

    // Feuert automatisch wenn SetFlowers() aufgerufen wird
    public static event System.Action OnProgressChanged;

    [Header("Blumenvorrat")]
    [SerializeField] private int totalFlowers = 9;
    public int RemainingFlowers => totalFlowers;

    [System.Serializable]
    public class PhaseProgress
    {
        public string phaseName;
        [Range(0, 3)] public int flowers;
    }

    public List<PhaseProgress> phases = new List<PhaseProgress>()
    {
        new PhaseProgress { phaseName = "Anger",      flowers = 0 },
        new PhaseProgress { phaseName = "Denial",     flowers = 0 },
        new PhaseProgress { phaseName = "Depression", flowers = 0 },
        new PhaseProgress { phaseName = "Bargain",    flowers = 0 },
        new PhaseProgress { phaseName = "Acceptance", flowers = 0 },
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Vergibt Blumen an eine Phase. Zieht automatisch vom Gesamtvorrat ab.
    /// Gibt false zurück wenn nicht genug Blumen vorhanden.
    /// </summary>
    public bool SetFlowers(string phaseName, int flowerCount)
    {
        flowerCount = Mathf.Clamp(flowerCount, 0, 3);

        if (flowerCount > totalFlowers)
        {
            Debug.LogWarning($"Nicht genug Blumen übrig. Verbleibend: {totalFlowers}");
            return false;
        }

        for (int i = 0; i < phases.Count; i++)
        {
            if (phases[i].phaseName == phaseName)
            {
                // Alten Wert zurückgeben bevor neu gesetzt wird
                totalFlowers += phases[i].flowers;
                totalFlowers -= flowerCount;

                phases[i].flowers = flowerCount;

                OnProgressChanged?.Invoke(); // Hauptraum updated sich automatisch
                return true;
            }
        }

        Debug.LogWarning($"Phase '{phaseName}' nicht gefunden.");
        return false;
    }

    public int GetFlowers(string phaseName)
    {
        for (int i = 0; i < phases.Count; i++)
        {
            if (phases[i].phaseName == phaseName)
                return phases[i].flowers;
        }
        return 0;
    }
}