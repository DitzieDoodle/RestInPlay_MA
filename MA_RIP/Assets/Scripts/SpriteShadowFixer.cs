using UnityEngine;

public class SpriteShadowFixer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var spriteRenderers = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.TryGetComponent<IgnoreShadowFixer>(out var fixer))
            {
                continue;
            }

            if (spriteRenderer.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.Off)
            {
                spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }

            spriteRenderer.receiveShadows = true;
        }
    }
}
