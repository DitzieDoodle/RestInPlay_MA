using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject flame;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lightSfx;

    public bool IsLit { get; private set; }
    public bool IsTarget { get; private set; }

    private void Start()
    {
        ResetCandle();
    }

    public void ResetCandle()
    {
        IsLit = false;
        IsTarget = false;

        if (indicator != null)
            indicator.SetActive(false);

        if (flame != null)
            flame.SetActive(false);
    }

    public void SetTarget(bool state)
    {
        if (IsLit) return;

        IsTarget = state;

        if (indicator != null)
            indicator.SetActive(state);
    }

    public void Light()
    {
        if (IsLit) return;

        IsLit = true;
        IsTarget = false;

        if (indicator != null)
            indicator.SetActive(false);

        if (flame != null)
            flame.SetActive(true);

        if (audioSource != null && lightSfx != null)
            audioSource.PlayOneShot(lightSfx);
    }

    private void OnMouseDown()
    {
        if (DepressionMinigameManager.Instance != null)
            DepressionMinigameManager.Instance.TryLightCandle(this);
    }
}