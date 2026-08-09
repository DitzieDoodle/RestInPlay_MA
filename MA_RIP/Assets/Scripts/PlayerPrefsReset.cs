using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPrefsReset : MonoBehaviour
{
    [Header("Reload Only (R)")]
    public KeyCode reloadKey = KeyCode.R;
    public float reloadHoldDuration = 8f;

    [Header("Reload + Clear PlayerPrefs (T)")]
    public KeyCode reloadAndClearKey = KeyCode.T;
    public float clearHoldDuration = 8f;

    [Header("UI Feedback")]
    public Image radialFillImage; // Image Type: Filled, Fill Method: Radial 360
    public GameObject feedbackContainer; // optional: Parent-Objekt, um die UI ein-/auszublenden

    private float holdTimer = 0f;
    private bool isResetting = false;
    private KeyCode? activeKey = null; // welche Taste aktuell gehalten wird

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

        bool reloadHeld = Input.GetKey(reloadKey);
        bool clearHeld = Input.GetKey(reloadAndClearKey);

        // Verhindert, dass beide Tasten gleichzeitig einen Reset triggern
        if (reloadHeld && clearHeld)
        {
            ResetTimer();
            return;
        }

        if (reloadHeld)
        {
            HandleHold(reloadKey, reloadHoldDuration);
        }
        else if (clearHeld)
        {
            HandleHold(reloadAndClearKey, clearHoldDuration);
        }
        else
        {
            ResetTimer();
        }
    }

    void HandleHold(KeyCode key, float duration)
    {
        // Falls vorher eine andere Taste gehalten wurde, Timer neu starten
        if (activeKey != key)
        {
            activeKey = key;
            holdTimer = 0f;
        }

        holdTimer += Time.deltaTime;

        if (feedbackContainer != null && !feedbackContainer.activeSelf)
        {
            feedbackContainer.SetActive(true);
        }

        if (radialFillImage != null)
        {
            radialFillImage.fillAmount = Mathf.Clamp01(holdTimer / duration);
        }

        if (holdTimer >= duration)
        {
            if (key == reloadAndClearKey)
            {
                TriggerReloadAndClear();
            }
            else
            {
                TriggerReloadOnly();
            }
        }
    }

    void ResetTimer()
    {
        if (holdTimer > 0f || activeKey != null)
        {
            holdTimer = 0f;
            activeKey = null;

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

    void TriggerReloadOnly()
    {
        isResetting = true;
        Debug.Log("Szene wird durch Halten von " + reloadKey + " neu geladen (PlayerPrefs bleiben erhalten).");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void TriggerReloadAndClear()
    {
        isResetting = true;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Alle PlayerPrefs wurden durch Halten von " + reloadAndClearKey + " gelöscht. Szene wird neu geladen.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}