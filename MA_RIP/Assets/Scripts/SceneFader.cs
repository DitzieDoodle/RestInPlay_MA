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

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
            StartCoroutine(FadeInOnly()); 
        }
    }

    public void StartFade()
    {
        StartCoroutine(FadeToScene()); 
    }

    private IEnumerator FadeInOnly()
    {
        yield return StartCoroutine(Fade(1f, 0f, false));  // false = keine Scene
    }

    private IEnumerator FadeToScene()
    {
        yield return StartCoroutine(Fade(0f, 1f, true));  // true = Scene laden
    }

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

        if (loadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
