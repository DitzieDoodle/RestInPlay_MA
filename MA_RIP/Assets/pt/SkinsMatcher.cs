using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.UI;  // Hier f�gen wir den Namespace f�r UI hinzu, damit wir Buttons nutzen k�nnen

public class SkinsMatcher : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation; // Referenz zum Spine-Charakter
    private Skin combinedSkin;  // Der kombinierte Skin

    // Arrays der Skin-Namen
    private string[] Eyes = { "Eyes_A", "Eyes_B", "Eyes_C" }; // Augen-Skins
    private string[] Mouths = { "Mouth_A", "Mouth_B", "Mouth_C" }; // Mund-Skins
    private string[] Body = { "Body_A", "Body_B", "Body_C" }; // K�rper-Skins

    private int currentEyeIndex = 0;
    private int currentMouthIndex = 0;
    private int currentBodyIndex = 0;

    // UI Button-Referenzen, die im Inspector zugewiesen werden k�nnen
    [Header("UI Buttons")]
    public Button nextEyeButton;
    public Button previousEyeButton;
    public Button nextMouthButton;
    public Button previousMouthButton;
    public Button nextBodyButton;
    public Button previousBodyButton;

    void Start()
    {
        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation reference is missing!");
            return;
        }

        // Skin nur einmal beim Start erstellen
        combinedSkin = new Skin("combined-skin");
        UpdateSkin();

        // Buttons mit den Methoden verkn�pfen
        if (nextEyeButton != null) nextEyeButton.onClick.AddListener(NextEyeSkin);
        if (previousEyeButton != null) previousEyeButton.onClick.AddListener(PreviousEyeSkin);
        if (nextMouthButton != null) nextMouthButton.onClick.AddListener(NextMouthSkin);
        if (previousMouthButton != null) previousMouthButton.onClick.AddListener(PreviousMouthSkin);
        if (nextBodyButton != null) nextBodyButton.onClick.AddListener(NextBodySkin);
        if (previousBodyButton != null) previousBodyButton.onClick.AddListener(PreviousBodySkin);
    }

    public void NextEyeSkin()
    {
        currentEyeIndex = (currentEyeIndex + 1) % Eyes.Length;
        UpdateSkin();
    }
    public void PreviousEyeSkin()
    {
        currentEyeIndex = (currentEyeIndex - 1 + Eyes.Length) % Eyes.Length;  // Negative Modulo verhindern
        UpdateSkin();
    }

    public void NextMouthSkin()
    {
        currentMouthIndex = (currentMouthIndex + 1) % Mouths.Length;
        UpdateSkin();
    }

    public void PreviousMouthSkin()
    {
        currentMouthIndex = (currentMouthIndex - 1 + Mouths.Length) % Mouths.Length;  // Negative Modulo verhindern
        UpdateSkin();
    }

    public void NextBodySkin()
    {
        currentBodyIndex = (currentBodyIndex + 1) % Body.Length;
        UpdateSkin();
    }

    public void PreviousBodySkin()
    {
        currentBodyIndex = (currentBodyIndex - 1 + Body.Length) % Body.Length;  // Negative Modulo verhindern
        UpdateSkin();
    }

    private void UpdateSkin()
    {
        // L�sche alle vorherigen Skins, bevor du neue hinzuf�gst
        combinedSkin.Clear();

        // Skins f�r Augen, Mund und K�rper hinzuf�gen
        AddSkinToCombined(Eyes[currentEyeIndex], "Eye");
        AddSkinToCombined(Mouths[currentMouthIndex], "Mouth");
        AddSkinToCombined(Body[currentBodyIndex], "Body");

        // Setze den Skin und wende ihn auf das Skelett an
        skeletonAnimation.Skeleton.SetSkin(combinedSkin);
        skeletonAnimation.Skeleton.SetToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
    }

    private void AddSkinToCombined(string skinName, string category)
    {
        var skin = skeletonAnimation.Skeleton.Data.FindSkin(skinName);
        if (skin == null)
        {
            Debug.LogWarning($"Skin '{skinName}' for category '{category}' not found.");
            return;
        }

        combinedSkin.AddSkin(skin);  // F�ge den Skin dem kombinierten Skin hinzu
    }
}
