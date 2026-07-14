using System.Collections;
using UnityEngine;

public class WobbleController : MonoBehaviour
{
    [Header("Material Settings")]
    [Tooltip("Material mit deinem Wobble-Shader")]
    public Material targetMaterial;

    [Tooltip("Dauer des Übergangs zwischen Wobble-Stufen")]
    public float transitionDuration = 1.0f;

    [Header("Shader Property Names")]
    [Tooltip("Name der Color-Property im Shader, z.B. _BaseColor oder _Color")]
    public string colorProperty = "_BaseColor";
    [Tooltip("Name der Wobble-Property im Shader, z.B. _WobbleStrength")]
    public string wobbleProperty = "_WobbleStrength";

    [Header("Wobble Stufen (im Inspector setzbar)")]
    public Color wobbleOneColor = Color.white * 0.8f;
    public float wobbleOneStrength = 0.2f;

    public Color wobbleTwoColor = Color.white * 1.0f;
    public float wobbleTwoStrength = 0.5f;

    public Color wobbleOffColor = Color.white;
    public float wobbleOffStrength = 0f;

    private Coroutine currentFade;


    public void SetWobbleOne()
    {
        StartFade(wobbleOneColor, wobbleOneStrength);
    }

    public void SetWobbleTwo()
    {
        StartFade(wobbleTwoColor, wobbleTwoStrength);
    }

    public void SetWobbleOff()
    {
        Debug.Log("Wobble Off");
        StartFade(wobbleOffColor, wobbleOffStrength);
    }

    private void StartFade(Color targetColor, float targetWobble)
    {
        if (targetMaterial == null)
        {
            Debug.LogWarning("WobbleController: Kein Material zugewiesen!");
            return;
        }

        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeShader(targetColor, targetWobble));
    }

    private IEnumerator FadeShader(Color targetColor, float targetWobble)
    {
        // Aktuelle Werte speichern
        Color startColor;
        float startWobble;

        // Safety: falls Property nicht existiert, früh raus
        if (!targetMaterial.HasProperty(colorProperty))
        {
            Debug.LogWarning($"WobbleController: Color-Property '{colorProperty}' existiert nicht im Material.");
            yield break;
        }

        if (!targetMaterial.HasProperty(wobbleProperty))
        {
            Debug.LogWarning($"WobbleController: Wobble-Property '{wobbleProperty}' existiert nicht im Material.");
            yield break;
        }

        startColor = targetMaterial.GetColor(colorProperty);
        startWobble = targetMaterial.GetFloat(wobbleProperty);

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);

            // Lerp Farbe & Wobble
            Color newColor = Color.Lerp(startColor, targetColor, t);
            float newWobble = Mathf.Lerp(startWobble, targetWobble, t);

            targetMaterial.SetColor(colorProperty, newColor);
            targetMaterial.SetFloat(wobbleProperty, newWobble);

            yield return null;
        }

        // Sicherstellen, dass Zielwerte exakt gesetzt werden
        targetMaterial.SetColor(colorProperty, targetColor);
        targetMaterial.SetFloat(wobbleProperty, targetWobble);

        currentFade = null;
    }
}