using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.UI;
using System;
using TMPro;

public class SkinsMatcher : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private SkeletonGraphic skeletonGraphic;
    [SerializeField] private GhostColorPicker colorPicker;
    [SerializeField] private bool isHatsEnabled = false;
    private Skin combinedSkinAnimation;
    private Skin combinedSkinGraphic;

    // Arrays der Skin-Namen
    private string[] Hats = { "Hat_A", "Hat_B", "Hat_C", "Hat_D", "Hat_E", "Hat_F", "Hat_G", };
    private string[] Eyes = { "Eyes_A", "Eyes_B", "Eyes_C", "Eyes_D", "Eyes_E", "Eyes_F", "Eyes_G", "Eyes_H" };
    private string[] Mouths = { "Mouth_A", "Mouth_B", "Mouth_C", "Mouth_D", "Mouth_E", "Mouth_F", "Mouth_G" };
    private string[] Body = { "Body_A", "Body_B", "Body_C", "Body_D" };

    private int currentEyeIndex = 0;
    private int currentMouthIndex = 0;
    private int currentBodyIndex = 0;
    private int currentHatIndex = 0;

    // PlayerPrefs Keys
    private const string HAT_KEY = "Char_Hat";
    private const string EYES_KEY = "Char_Eyes";
    private const string MOUTH_KEY = "Char_Mouth";
    private const string BODY_KEY = "Char_Body";
    public const string NAME_KEY = "Char_Name";
    public const string BASE_NAME = "Player";

    [Header("UI Buttons")]
    public Button nextHatButton;
    public Button previousHatButton;
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
        if (skeletonAnimation == null && skeletonGraphic == null)
        {
            Debug.LogError("SkeletonAnimation and SkeletonGraphic references are missing!");
            return;
        }

        // 🎯 NEU: Gespeicherte Auswahl laden
        LoadSelection();

        combinedSkinAnimation = new Skin("combined-skin");
        UpdateSkinAnimation();

        combinedSkinGraphic = new Skin("combined-skin");
        UpdateSkinGraphic();

        // Buttons verknüpfen
        if (nextHatButton != null) nextHatButton.onClick.AddListener(NextHatSkin);
        if (previousHatButton != null) previousHatButton.onClick.AddListener(PreviousHatSkin);
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
        PlayerPrefs.SetInt(HAT_KEY, currentHatIndex);
        PlayerPrefs.Save();  // WICHTIG: Sofort auf Disk!
    }

    // 🎯 NEU: Auswahl laden
    private void LoadSelection()
    {
        currentEyeIndex = PlayerPrefs.GetInt(EYES_KEY, 0);
        currentMouthIndex = PlayerPrefs.GetInt(MOUTH_KEY, 0);
        currentBodyIndex = PlayerPrefs.GetInt(BODY_KEY, 0);
        currentHatIndex = PlayerPrefs.GetInt(HAT_KEY, 0);
        string savedName = PlayerPrefs.GetString(NAME_KEY, "Player");
        if (playerNameInputField != null)
        {
            playerNameInputField.text = savedName;
        }
    }

    public void NextHatSkin()
    {
        currentHatIndex = (currentHatIndex + 1) % Hats.Length;
        UpdateSkinAnimation();
        UpdateSkinGraphic();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void PreviousHatSkin()
    {
        currentHatIndex = (currentHatIndex - 1 + Hats.Length) % Hats.Length;
        UpdateSkinAnimation();
        UpdateSkinGraphic();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    // Buttons - JETZT mit automatischer Speicherung!
    public void NextEyeSkin()
    {
        currentEyeIndex = (currentEyeIndex + 1) % Eyes.Length;
        UpdateSkinAnimation();
        UpdateSkinGraphic();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void PreviousEyeSkin()
    {
        currentEyeIndex = (currentEyeIndex - 1 + Eyes.Length) % Eyes.Length;
        UpdateSkinAnimation();
        UpdateSkinGraphic();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void NextMouthSkin()
    {
        currentMouthIndex = (currentMouthIndex + 1) % Mouths.Length;
        UpdateSkinAnimation();
        UpdateSkinGraphic();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void PreviousMouthSkin()
    {
        currentMouthIndex = (currentMouthIndex - 1 + Mouths.Length) % Mouths.Length;
        UpdateSkinAnimation();
        UpdateSkinGraphic();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void NextBodySkin()
    {
        currentBodyIndex = (currentBodyIndex + 1) % Body.Length;
        UpdateSkinAnimation();
        UpdateSkinGraphic();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    public void PreviousBodySkin()
    {
        currentBodyIndex = (currentBodyIndex - 1 + Body.Length) % Body.Length;
        UpdateSkinAnimation();
        UpdateSkinGraphic();
        SaveSelection();  // 🎯 AUTOMATISCH speichern
    }

    // 🎯 NEU: PUBLIC - Für andere Scenes abrufbar!
    public int GetHatIndex() { return currentHatIndex; }
    public int GetEyeIndex() { return currentEyeIndex; }
    public int GetMouthIndex() { return currentMouthIndex; }
    public int GetBodyIndex() { return currentBodyIndex; }
    public string[] GetEyes() { return Eyes; }
    public string[] GetMouths() { return Mouths; }
    public string[] GetBody() { return Body; }

    private void UpdateSkinAnimation()
    {
        if (skeletonAnimation == null)
        {
            return;
        }

        combinedSkinAnimation.Clear();

        if (isHatsEnabled)
        {
            AddSkinToCombinedAnimation(Hats[currentHatIndex], "Hat");
        }
        AddSkinToCombinedAnimation(Eyes[currentEyeIndex], "Eye");
        AddSkinToCombinedAnimation(Mouths[currentMouthIndex], "Mouth");
        AddSkinToCombinedAnimation(Body[currentBodyIndex], "Body");


        skeletonAnimation.Skeleton.SetSkin(combinedSkinAnimation);
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

    private void UpdateSkinGraphic()
    {
        if (skeletonGraphic == null)
        {
            return;
        }

        combinedSkinGraphic.Clear();

        if (isHatsEnabled)
        {
            AddSkinToCombinedAnimationGraphic(Hats[currentHatIndex], "Hat");
        }
        AddSkinToCombinedAnimationGraphic(Eyes[currentEyeIndex], "Eye");
        AddSkinToCombinedAnimationGraphic(Mouths[currentMouthIndex], "Mouth");
        AddSkinToCombinedAnimationGraphic(Body[currentBodyIndex], "Body");

        skeletonGraphic.Skeleton.SetSkin(combinedSkinGraphic);
        skeletonGraphic.Skeleton.SetToSetupPose();
        skeletonGraphic.AnimationState.Apply(skeletonGraphic.Skeleton);

        Slot slot = skeletonGraphic.Skeleton.FindSlot(colorSlotName);
        if (slot != null)
        {
            slot.SetColor(colorPicker.CurrentColor);
        }

        skeletonGraphic.Update(0);
        skeletonGraphic.LateUpdate();
    }

    private void AddSkinToCombinedAnimation(string skinName, string category)
    {
        var skin = skeletonAnimation.Skeleton.Data.FindSkin(skinName);
        if (skin == null)
        {
            Debug.LogWarning($"Skin '{skinName}' for category '{category}' not found.");
            return;
        }
        combinedSkinAnimation.AddSkin(skin);
    }

    private void AddSkinToCombinedAnimationGraphic(string skinName, string category)
    {
        var skin = skeletonGraphic.Skeleton.Data.FindSkin(skinName);
        if (skin == null)
        {
            Debug.LogWarning($"Skin '{skinName}' for category '{category}' not found.");
            return;
        }
        combinedSkinGraphic.AddSkin(skin);
    }
}
