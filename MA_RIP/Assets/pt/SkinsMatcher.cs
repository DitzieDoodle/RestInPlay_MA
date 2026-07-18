using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.UI;
using System;
using TMPro;

public class SkinsMatcher : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private GhostColorPicker colorPicker;
    private Skin combinedSkin;

    // Arrays der Skin-Namen
    private string[] Eyes = { "Eyes_A", "Eyes_B", "Eyes_C" };
    private string[] Mouths = { "Mouth_A", "Mouth_B", "Mouth_C" };
    private string[] Body = { "Body_A", "Body_B", "Body_C" };

    private int currentEyeIndex = 0;
    private int currentMouthIndex = 0;
    private int currentBodyIndex = 0;

    // PlayerPrefs Keys
    private const string EYES_KEY = "Char_Eyes";
    private const string MOUTH_KEY = "Char_Mouth";
    private const string BODY_KEY = "Char_Body";
    public const string NAME_KEY = "Char_Name";
    public const string BASE_NAME = "Player";

    [Header("UI Buttons")]
    public Button nextEyeButton;
    public Button previousEyeButton;
    public Button nextMouthButton;
    public Button previousMouthButton;
    public Button nextBodyButton;
    public Button previousBodyButton;
    public TMP_InputField playerNameInputField;

    private string colorSlotName = "Bodies";

    void Start()
    {
        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation reference is missing!");
            return;
        }

        // 🎯 NEU: Gespeicherte Auswahl laden
        LoadSelection();

        combinedSkin = new Skin("combined-skin");
        UpdateSkin();

        // Buttons verknüpfen
        if (nextEyeButton != null) nextEyeButton.onClick.AddListener(NextEyeSkin);
        if (previousEyeButton != null) previousEyeButton.onClick.AddListener(PreviousEyeSkin);
        if (nextMouthButton != null) nextMouthButton.onClick.AddListener(NextMouthSkin);
        if (previousMouthButton != null) previousMouthButton.onClick.AddListener(PreviousMouthSkin);
        if (nextBodyButton != null) nextBodyButton.onClick.AddListener(NextBodySkin);
        if (previousBodyButton != null) previousBodyButton.onClick.AddListener(PreviousBodySkin);
        if (playerNameInputField != null)
        {
            playerNameInputField.text = PlayerPrefs.GetString(NAME_KEY, "Player");
            playerNameInputField.onEndEdit.AddListener(SetPlayerName);
        }
    }

    private void SetPlayerName(string playerName)
    {
        PlayerPrefs.SetString(NAME_KEY, playerName);
        PlayerPrefs.Save();
    }

    // 🎯 NEU: Auswahl speichern (wird bei jedem Button-Press aufgerufen)
    private void SaveSelection()
    {
        PlayerPrefs.SetInt(EYES_KEY, currentEyeIndex);
        PlayerPrefs.SetInt(MOUTH_KEY, currentMouthIndex);
        PlayerPrefs.SetInt(BODY_KEY, currentBodyIndex);
        PlayerPrefs.Save();  // WICHTIG: Sofort auf Disk!
    }

    // 🎯 NEU: Auswahl laden
    private void LoadSelection()
    {
        currentEyeIndex = PlayerPrefs.GetInt(EYES_KEY, 0);
        currentMouthIndex = PlayerPrefs.GetInt(MOUTH_KEY, 0);
        currentBodyIndex = PlayerPrefs.GetInt(BODY_KEY, 0);
        string savedName = PlayerPrefs.GetString(NAME_KEY, "Player"); // Default
    }

    // Buttons - JETZT mit automatischer Speicherung!
    public void NextEyeSkin()
    {
        currentEyeIndex = (currentEyeIndex + 1) % Eyes.Length;
        UpdateSkin();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void PreviousEyeSkin()
    {
        currentEyeIndex = (currentEyeIndex - 1 + Eyes.Length) % Eyes.Length;
        UpdateSkin();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void NextMouthSkin()
    {
        currentMouthIndex = (currentMouthIndex + 1) % Mouths.Length;
        UpdateSkin();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void PreviousMouthSkin()
    {
        currentMouthIndex = (currentMouthIndex - 1 + Mouths.Length) % Mouths.Length;
        UpdateSkin();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void NextBodySkin()
    {
        currentBodyIndex = (currentBodyIndex + 1) % Body.Length;
        UpdateSkin();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void PreviousBodySkin()
    {
        currentBodyIndex = (currentBodyIndex - 1 + Body.Length) % Body.Length;
        UpdateSkin();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    // 🎯 NEU: PUBLIC - Für andere Scenes abrufbar!
    public int GetEyeIndex() { return currentEyeIndex; }
    public int GetMouthIndex() { return currentMouthIndex; }
    public int GetBodyIndex() { return currentBodyIndex; }
    public string[] GetEyes() { return Eyes; }
    public string[] GetMouths() { return Mouths; }
    public string[] GetBody() { return Body; }

    private void UpdateSkin()
    {
        combinedSkin.Clear();

        AddSkinToCombined(Eyes[currentEyeIndex], "Eye");
        AddSkinToCombined(Mouths[currentMouthIndex], "Mouth");
        AddSkinToCombined(Body[currentBodyIndex], "Body");

        skeletonAnimation.Skeleton.SetSkin(combinedSkin);
        skeletonAnimation.Skeleton.SetToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);

        Slot slot = skeletonAnimation.Skeleton.FindSlot(colorSlotName);
        if (slot != null)
        {
            slot.SetColor(colorPicker.CurrentColor);
        }

        skeletonAnimation.Update(0);
        skeletonAnimation.LateUpdate();
    }

    private void AddSkinToCombined(string skinName, string category)
    {
        var skin = skeletonAnimation.Skeleton.Data.FindSkin(skinName);
        if (skin == null)
        {
            Debug.LogWarning($"Skin '{skinName}' for category '{category}' not found.");
            return;
        }
        combinedSkin.AddSkin(skin);
    }
}
