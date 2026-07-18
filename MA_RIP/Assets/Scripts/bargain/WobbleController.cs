using UnityEngine;

public class MaterialWobbleController : MonoBehaviour
{
    [Header("Material Settings")]
    public Material targetMaterial;
    public string wobbleProperty = "WobbleStrength";

    public float wobbleOneStrength = 0.2f;
    public float wobbleTwoStrength = 0.5f;
    public float wobbleOffStrength = 0f;

    public void SetWobbleOne()
    {
        SetWobble(wobbleOneStrength);
    }

    public void SetWobbleTwo()
    {
        SetWobble(wobbleTwoStrength);
    }

    public void SetWobbleOff()
    {
        SetWobble(wobbleOffStrength);
        Debug.Log("WobbleOff");
    }

    // ▼ Generische Setter-Methode (optional)
    public void SetWobble(float value)
    {
        if (targetMaterial == null)
        {
            Debug.LogWarning("MaterialWobbleController: Kein Material zugewiesen.");
            return;
        }

        if (!targetMaterial.HasProperty(wobbleProperty))
        {
            Debug.LogWarning($"MaterialWobbleController: Property '{wobbleProperty}' existiert nicht im Material.");
            return;
        }

        targetMaterial.SetFloat(wobbleProperty, value);
        Debug.Log($"MaterialWobbleController: {wobbleProperty} = {value}");
    }
}