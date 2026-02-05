using System.Collections;
using UnityEngine;

public class WobbleController : MonoBehaviour
{
    [Header("Material Settings")]
    public Material targetMaterial; // Dein Shader-Material
    public float transitionDuration = 1.0f; // Dauer des Fades


    private string colorProperty = "basecolor";
    private string wobbleProperty = "wobbleStrength";
 
    public void SetWobbleOne()
    {
        StartCoroutine(FadeShader(Color.white * 0.8f, 0.2f)); 
    }

    public void SetWobbleTwo()
    {
        StartCoroutine(FadeShader(Color.white * 1.0f, 0.1f)); 
    }

    public void SetWobbleOff()
    {
        Debug.Log("Wobble Off");
        StartCoroutine(FadeShader(Color.white, 0f)); 
    }

    // Coroutine zum sanften Übergang
    public IEnumerator FadeShader(Color targetColor, float targetWobble)
    {
        if (targetMaterial == null)
        {
            Debug.LogWarning("Kein Material zugewiesen!");
            yield break;
        }

        // Aktuelle Werte speichern
        Color startColor = targetMaterial.GetColor(colorProperty);
        float startWobble = targetMaterial.GetFloat(wobbleProperty);

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

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
    }
}
