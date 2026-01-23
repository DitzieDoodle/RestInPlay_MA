using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LocalPostProcessing : MonoBehaviour
{
    public Volume[] volumes; // Alle Volumes, die dieser Trigger steuert
    public bool fade = false;
    public float fadeSpeed = 1f;

    private bool inside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
        }
    }

    private void Update()
    {
        foreach (var vol in volumes)
        {
            if (vol != null)
            {
                if (fade)
                {
                    vol.weight = Mathf.Lerp(vol.weight, inside ? 1f : 0f, Time.deltaTime * fadeSpeed);
                }
                else
                {
                    vol.weight = inside ? 1f : 0f;
                }
            }
        }
    }
}
