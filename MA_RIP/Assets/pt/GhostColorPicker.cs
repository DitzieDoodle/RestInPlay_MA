using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Slot = Spine.Slot;
using UnityColor = UnityEngine.Color;

public class GhostColorPicker : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Button nextColorButton;
    [SerializeField] private Image colorPreview;

    [Header("Colors")]
    [SerializeField] private UnityColor[] colorArray;  // Array mit Farben, die im Inspector festgelegt werden k�nnen
    private int currentColorIndex = 0;

    private string targetSlotName = "Bodies";  // Der Slot, den wir einf�rben

    void Start()
    {
        if (colorArray.Length > 0)
        {
            UpdateColor();  // Setze die initiale Farbe
        }

        // F�ge Listener f�r Button hinzu
        nextColorButton.onClick.AddListener(ChangeToNextColor);
    }

    public void ChangeToNextColor()
    {
        currentColorIndex = (currentColorIndex + 1) % colorArray.Length;  // N�chster Index
        UpdateColor();
    }

    public void ChangeToPreviousColor()
    {
        currentColorIndex = (currentColorIndex - 1) % colorArray.Length;  // N�chster Index
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
        slot.R = color.r;
        slot.G = color.g;
        slot.B = color.b;
        slot.A = color.a;

        // Update das Skelett, damit �nderungen sichtbar werden
        skeletonAnimation.Skeleton.SetToSetupPose();
        skeletonAnimation.Update(0);  // Dies sollte das Skelett aktualisieren
        skeletonAnimation.LateUpdate();  // �berpr�fe, ob das zu einer sichtbaren �nderung f�hrt
    
}
}
