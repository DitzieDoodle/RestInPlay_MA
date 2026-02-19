using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Header("Fade Einstellungen")]
    public Image fadeImage;
    public float fadeDuration = 0.8f;

    [Header("Ziel Scene")]
    public string sceneToLoad = "GameScene";

    void Start()
    {
        // NUR Fade-IN beim Start, KEINE Scene laden!
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
            StartCoroutine(FadeInOnly());  // NEUER: Nur Fade-IN!
        }
    }

    // Button ruft nur DAS auf
    public void StartFade()
    {
        StartCoroutine(FadeToScene());  // NEUER: Fade + Scene-Load
    }

    // NEU: Nur Fade-IN (ohne Scene-Load)
    private IEnumerator FadeInOnly()
    {
        yield return StartCoroutine(Fade(1f, 0f, false));  // false = keine Scene
    }

    // NEU: Fade + Scene-Load
    private IEnumerator FadeToScene()
    {
        yield return StartCoroutine(Fade(0f, 1f, true));  // true = Scene laden
    }

    // FIX: Parameter für Scene-Load hinzugefügt
    private IEnumerator Fade(float startAlpha, float endAlpha, bool loadScene)
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = startAlpha;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);
            float a = Mathf.Lerp(startAlpha, endAlpha, normalized);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        // Scene NUR laden wenn explizit angefordert!
        if (loadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
