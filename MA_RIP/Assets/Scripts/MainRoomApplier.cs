using UnityEngine;
using System.Collections.Generic;

public class MainRoomApplier : MonoBehaviour
{
    [System.Serializable]
    public class PhaseVisuals
    {
        public string phaseName;
        public Color mainColor = Color.red;
        public Color companionColor = new Color(1f, 0.7f, 0.7f);
        public List<SpriteRenderer> mainSprites = new List<SpriteRenderer>();
        public List<SpriteRenderer> companionSprites = new List<SpriteRenderer>();
        public List<GameObject> bonusObjects = new List<GameObject>();
    }

    [SerializeField] private List<PhaseVisuals> phases = new List<PhaseVisuals>();

    private void OnEnable()
    {
        // Ab hier auf Änderungen hören
        GriefProgressManager.OnProgressChanged += ApplyAll;
    }

    private void OnDisable()
    {
        // Sauber abmelden wenn Objekt deaktiviert wird
        GriefProgressManager.OnProgressChanged -= ApplyAll;
    }

    private void Start()
    {
        // Initialer Aufruf beim Laden der Scene
        ApplyAll();
    }

    public void ApplyAll()
    {
        if (GriefProgressManager.Instance == null) return;

        foreach (var phase in phases)
        {
            int flowers = GriefProgressManager.Instance.GetFlowers(phase.phaseName);

            // 1 Blume  → Hauptfarbe aktiv
            ApplyColorToSprites(phase.mainSprites, phase.mainColor, flowers >= 1);

            // 2 Blumen → Begleitfarbe aktiv
            ApplyColorToSprites(phase.companionSprites, phase.companionColor, flowers >= 2);

            // 3 Blumen → Bonus-Objekte aktiv (z.B. Blumenstrauß, Foto, Kerze)
            bool bonusActive = flowers >= 3;
            foreach (var obj in phase.bonusObjects)
            {
                if (obj != null)
                    obj.SetActive(bonusActive);
            }
        }
    }

    private void ApplyColorToSprites(List<SpriteRenderer> sprites, Color color, bool active)
    {
        foreach (var sr in sprites)
        {
            if (sr == null) continue;
            sr.gameObject.SetActive(true);
            sr.color = active ? color : Color.white;
        }
    }
}