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

    private void Start()
    {
        ApplyAll();
    }

    public void ApplyAll()
    {
        if (GriefProgressManager.Instance == null) return;

        foreach (var phase in phases)
        {
            int flowers = GriefProgressManager.Instance.GetFlowers(phase.phaseName);

            ApplyColorToSprites(phase.mainSprites, phase.mainColor, flowers >= 1);
            ApplyColorToSprites(phase.companionSprites, phase.companionColor, flowers >= 2);

            bool bonusActive = flowers >= 3;
            foreach (var obj in phase.bonusObjects)
                obj.SetActive(bonusActive);
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