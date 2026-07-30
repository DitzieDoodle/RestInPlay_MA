using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject flame;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lightSfx;
    [SerializeField] private string playerTag = "Player";

    public bool IsLit { get; private set; }
    public bool IsTarget { get; private set; }
    public bool IsPlayerInRange { get; private set; }

    private void Start()
    {
        ResetCandle();
    }

    public void ResetCandle()
    {
        IsLit = false;
        IsTarget = false;
        IsPlayerInRange = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            IsPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
            IsPlayerInRange = false;
    }
}