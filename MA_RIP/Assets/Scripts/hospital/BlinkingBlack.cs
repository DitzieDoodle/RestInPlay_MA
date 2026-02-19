using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlinkingBlack : MonoBehaviour
{
    [Header("Ziel Scene")]
    public string sceneToLoad = "GameScene";

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f; // Dauer in Sekunden

    // Methode 1: Fade zu Schwarz (Alpha 1)
    public void FadeToBlack()
    {
        StartCoroutine(FadeTo(1f));
    }

    // Methode 2: Fade zu transparent (Alpha 0)
    public void Clear()
    {
        StartCoroutine(FadeTo(0f));
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        fadeImage.color = new Color(0, 0, 0, fadeImage.color.a); // Schwarz halten
        Color startColor = fadeImage.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha); // Genaues Ende
    }

    public void OnMessage(string message)
    {
        if (message == "FadeToBlack") FadeToBlack();
        else if (message == "FadeToTransparent") Clear();
    }
}
