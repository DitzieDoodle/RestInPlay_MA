using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPrefsReset : MonoBehaviour
{
    [Header("Hold-to-Reset Settings")]
    public KeyCode resetKey = KeyCode.R;
    public float holdDuration = 8f;

    [Header("UI Feedback")]
    public Image radialFillImage; // Image Type: Filled, Fill Method: Radial 360
    public GameObject feedbackContainer; // optional: Parent-Objekt, um die UI ein-/auszublenden

    private float holdTimer = 0f;
    private bool isResetting = false;

    void Start()
    {
        if (radialFillImage != null)
        {
            radialFillImage.fillAmount = 0f;
        }

        if (feedbackContainer != null)
        {
            feedbackContainer.SetActive(false);
        }
    }

    void Update()
    {
        if (isResetting) return; // verhindert doppeltes Auslösen

        if (Input.GetKey(resetKey))
        {
            holdTimer += Time.deltaTime;

            if (feedbackContainer != null && !feedbackContainer.activeSelf)
            {
                feedbackContainer.SetActive(true);
            }

            if (radialFillImage != null)
            {
                radialFillImage.fillAmount = Mathf.Clamp01(holdTimer / holdDuration);
            }

            if (holdTimer >= holdDuration)
            {
                TriggerReset();
            }
        }
        else
        {
            // Taste losgelassen, bevor die Zeit voll war -> zurücksetzen
            if (holdTimer > 0f)
            {
                holdTimer = 0f;

                if (radialFillImage != null)
                {
                    radialFillImage.fillAmount = 0f;
                }

                if (feedbackContainer != null)
                {
                    feedbackContainer.SetActive(false);
                }
            }
        }
    }

    void TriggerReset()
    {
        isResetting = true;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Alle PlayerPrefs wurden durch Halten von " + resetKey + " gelöscht. Szene wird neu geladen.");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}