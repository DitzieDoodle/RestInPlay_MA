using UnityEngine;
using Spine.Unity;
using Spine;

public class CharacterLoader : MonoBehaviour
{
    [Header("3 Separate SkeletonAnimations")]
    [SerializeField] private SkeletonAnimation eyeSkeleton;     // Augen-Skeleton
    [SerializeField] private SkeletonAnimation mouthSkeleton;   // Mund-Skeleton  
    [SerializeField] private SkeletonAnimation bodySkeleton;    // Body-Skeleton

    [SerializeField] private UnityEngine.Color defaultColor = Color.white;

    // Exakt dieselben Keys wie in SkinsMatcher!
    private const string EYES_KEY = "Char_Eyes";
    private const string MOUTH_KEY = "Char_Mouth";
    private const string BODY_KEY = "Char_Body";

    // Skin Arrays (identisch zu SkinsMatcher!)
    private string[] Eyes = { "Eyes_A", "Eyes_B", "Eyes_C" };
    private string[] Mouths = { "Mouth_A", "Mouth_B", "Mouth_C" };
    private string[] Body = { "Body_A", "Body_B", "Body_C" };

    private string colorSlotName = "Bodies";

    void Start()
    {
        LoadAndApplyCharacter();
    }

    private void LoadAndApplyCharacter()
    {
        // Aus PlayerPrefs laden
        int eyeIndex = PlayerPrefs.GetInt(EYES_KEY, 0);
        int mouthIndex = PlayerPrefs.GetInt(MOUTH_KEY, 0);
        int bodyIndex = PlayerPrefs.GetInt(BODY_KEY, 0);

        Debug.Log($"Char geladen: Eyes={Eyes[eyeIndex]}, Mouth={Mouths[mouthIndex]}, Body={Body[bodyIndex]}");

        // 🔥 JEDEM Skeleton seinen Skin einzeln zuweisen!
        LoadSkinToSkeleton(eyeSkeleton, Eyes[eyeIndex], "Eye");
        LoadSkinToSkeleton(mouthSkeleton, Mouths[mouthIndex], "Mouth");
        LoadSkinToSkeleton(bodySkeleton, Body[bodyIndex], "Body");

        // Farbe nur auf Body-Skeleton anwenden
        if (bodySkeleton != null)
        {
            ApplyColorToSlot(bodySkeleton, defaultColor);
        }
    }

    private void LoadSkinToSkeleton(SkeletonAnimation skeleton, string skinName, string category)
    {
        if (skeleton == null)
        {
            Debug.LogWarning($"{category}-Skeleton fehlt!");
            return;
        }

        var skin = skeleton.Skeleton.Data.FindSkin(skinName);
        if (skin == null)
        {
            Debug.LogWarning($"Skin '{skinName}' für {category} nicht gefunden.");
            return;
        }

        // Skin direkt auf dieses Skeleton setzen
        skeleton.Skeleton.SetSkin(skin);
        skeleton.Skeleton.SetToSetupPose();
        skeleton.AnimationState.Apply(skeleton.Skeleton);
        skeleton.Update(0);
        skeleton.LateUpdate();
    }

    private void ApplyColorToSlot(SkeletonAnimation skeleton, Color color)
    {
        Slot slot = skeleton.Skeleton.FindSlot(colorSlotName);
        if (slot != null)
        {
            slot.SetColor(color);
        }
        else
        {
            Debug.LogWarning($"Color-Slot '{colorSlotName}' nicht gefunden!");
        }
    }
}
