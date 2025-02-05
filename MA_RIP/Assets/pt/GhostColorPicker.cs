using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Slot = Spine.Slot;
using UnityColor = UnityEngine.Color;
using System;

public class GhostColorPicker : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Button prevColorButton;
    [SerializeField] private Button nextColorButton;
    [SerializeField] private Image colorPreview;

    [Header("Colors")]
    [SerializeField] private UnityColor[] colorArray;  // Array mit Farben, die im Inspector festgelegt werden können
    private int currentColorIndex = 0;

    private string targetSlotName = "Bodies";  // Der Slot, den wir einfärben

    public Color CurrentColor => colorArray[currentColorIndex];

    void Start()
    {
        if (colorArray.Length > 0)
        {
            UpdateColor();  // Setze die initiale Farbe
        }

        // Füge Listener für Button hinzu
        nextColorButton.onClick.AddListener(ChangeToNextColor);
        prevColorButton.onClick.AddListener(ChangeToPreviousColor);
    }

    public void ChangeToNextColor()
    {
        currentColorIndex = mod(currentColorIndex + 1,  colorArray.Length);  // Nächster Index
        UpdateColor();
    }

    public void ChangeToPreviousColor()
    {
        currentColorIndex = (currentColorIndex - 1 + colorArray.Length) % colorArray.Length;  // Negative Modulo verhindern
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (colorArray.Length > 0)
        {
            UnityColor selectedColor = colorArray[currentColorIndex];
            ChangeSlotColor(targetSlotName, selectedColor);
            colorPreview.color = selectedColor;  // Vorschau aktualisieren
            Debug.Log($"Applied color {selectedColor} to slot '{targetSlotName}'");
        }
    }

    private void ChangeSlotColor(string slotName, UnityColor color)
    {
        Slot slot = skeletonAnimation.Skeleton.FindSlot(slotName);
        if (slot == null)
        {
            Debug.LogError($"Slot '{slotName}' not found!");
            return;
        }

        // Direkt die RGBA-Werte setzen
        //slot.R = color.r;
        //slot.G = color.g;
        //slot.B = color.b;
        //slot.A = color.a;

        slot.SetColor(color);

        // Update das Skelett, damit Änderungen sichtbar werden
        //skeletonAnimation.Skeleton.SetToSetupPose();
        skeletonAnimation.Update(0);  // Dies sollte das Skelett aktualisieren
        skeletonAnimation.LateUpdate();  // Überprüfe, ob das zu einer sichtbaren Änderung führt
    
    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
