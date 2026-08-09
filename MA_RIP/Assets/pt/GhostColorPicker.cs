using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Slot = Spine.Slot;
using UnityColor = UnityEngine.Color;
using System;

public class GhostColorPicker : MonoBehaviour
{
    const string PREFS_KEY = "GhostColorIndex";

    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private SkeletonGraphic skeletonGraphic;
    [SerializeField] private Button prevColorButton;
    [SerializeField] private Button nextColorButton;
    [SerializeField] private Image colorPreview;

    [Header("Colors")]
    [SerializeField] private UnityColor[] colorArray;  // Array mit Farben, die im Inspector festgelegt werden k�nnen
    private int currentColorIndex = 0;


    private string targetSlotName = "Bodies";  // Der Slot, den wir einf�rben

    public Color CurrentColor => colorArray[currentColorIndex];

    void Awake()
    {
        currentColorIndex = PlayerPrefs.GetInt(PREFS_KEY, 0);
        if (colorArray.Length > 0)
        {
            UpdateColor();  // Setze die initiale Farbe
        }

        // F�ge Listener f�r Button hinzu
        nextColorButton?.onClick.AddListener(ChangeToNextColor);
        prevColorButton?.onClick.AddListener(ChangeToPreviousColor);

        // Lade den gespeicherten Index, falls vorhanden
        UpdateColor();
    }

    public void ChangeToNextColor()
    {
        currentColorIndex = mod(currentColorIndex + 1, colorArray.Length);  // N�chster Index
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
            ChangeSlotColorGraphic(targetSlotName, selectedColor);
            if (colorPreview) colorPreview.color = selectedColor;  // Vorschau aktualisieren
            Debug.Log($"Applied color {selectedColor} to slot '{targetSlotName}'");
        }
    }

    private void ChangeSlotColorGraphic(string slotName, UnityColor color)
    {
        if (skeletonGraphic == null)
        {
            return;
        }

        Slot slot = skeletonGraphic.Skeleton.FindSlot(slotName);
        if (slot == null)
        {
            Debug.LogError($"Slot '{slotName}' not found!");
            return;
        }

        slot.SetColor(color);

        // Update das Skelett, damit �nderungen sichtbar werden
        skeletonGraphic.Update(0);  // Dies sollte das Skelett aktualisieren
        skeletonGraphic.LateUpdate();  // �berpr�fe, ob das zu einer sichtbaren �nderung f�hrt

        PlayerPrefs.SetInt(PREFS_KEY, currentColorIndex);
        PlayerPrefs.Save();

        Debug.Log($"Color for slot '{slotName}' changed to {color}. Current index saved as {currentColorIndex}.");
    }

    private void ChangeSlotColor(string slotName, UnityColor color)
    {
        if (skeletonAnimation == null)
        {
            return;
        }

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

        // Update das Skelett, damit �nderungen sichtbar werden
        //skeletonAnimation.Skeleton.SetToSetupPose();
        skeletonAnimation.Update(0);  // Dies sollte das Skelett aktualisieren
        skeletonAnimation.LateUpdate();  // �berpr�fe, ob das zu einer sichtbaren �nderung f�hrt

        PlayerPrefs.SetInt(PREFS_KEY, currentColorIndex);
        PlayerPrefs.Save();

        Debug.Log($"Color for slot '{slotName}' changed to {color}. Current index saved as {currentColorIndex}.");
    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
