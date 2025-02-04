using UnityEngine;
using Spine.Unity;
using Spine;

public class SkinsMatcher : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation; // Referenz zum Spine-Charakter
    private Skin combinedSkin;  // Der kombinierte Skin
    private string[] Eyes = { "Eyes_A", "Eyes_B", "Eyes_C" }; // Augen-Skins
    private string[] Mouths = { "Mouth_A", "Mouth_B", "Mouth_C" }; // Mund-Skins
    private string[] Body = { "Body_A", "Body_B", "Body_C" }; // Körper-Skins
    private int currentEyeIndex = 0;
    private int currentMouthIndex = 0;
    private int currentBodyIndex = 0;

    void Start()
    {
        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation reference is missing!");
            return;
        }

        UpdateSkin();
    }

    public void NextEyeSkin()
    {
        currentEyeIndex = (currentEyeIndex + 1) % Eyes.Length;
        UpdateSkin();
    }

    public void NextMouthSkin()
    {
        currentMouthIndex = (currentMouthIndex + 1) % Mouths.Length;
        UpdateSkin();
    }

    public void NextBodySkin()
    {
        currentBodyIndex = (currentBodyIndex + 1) % Body.Length;
        UpdateSkin();
    }

    private void UpdateSkin()
    {
        combinedSkin = new Skin("combined-skin");

        AddSkinToCombined(Eyes[currentEyeIndex], "Eye");
        AddSkinToCombined(Mouths[currentMouthIndex], "Mouth");
        AddSkinToCombined(Body[currentBodyIndex], "Body");

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

        combinedSkin.AddSkin(skin);
    }
}