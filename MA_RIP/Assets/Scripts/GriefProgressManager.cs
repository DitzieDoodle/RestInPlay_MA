using UnityEngine;
using System.Collections.Generic;

public class GriefProgressManager : MonoBehaviour
{
    public static GriefProgressManager Instance { get; private set; }

    [System.Serializable]
    public class PhaseProgress
    {
        public string phaseName;
        [Range(0, 3)] public int flowers;
    }

    public List<PhaseProgress> phases = new List<PhaseProgress>()
    {
        new PhaseProgress { phaseName = "Anger", flowers = 0 },
        new PhaseProgress { phaseName = "Denial", flowers = 0 },
        new PhaseProgress { phaseName = "Depression", flowers = 0 },
        new PhaseProgress { phaseName = "Bargain", flowers = 0 },
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

    public void SetFlowers(string phaseName, int flowerCount)
    {
        flowerCount = Mathf.Clamp(flowerCount, 0, 3);

        for (int i = 0; i < phases.Count; i++)
        {
            if (phases[i].phaseName == phaseName)
            {
                phases[i].flowers = flowerCount;
                return;
            }
        }
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