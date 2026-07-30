using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.Cinemachine;

/// <summary>
/// Zone, in der der Spieler SPACE gedrückt halten muss, um als Geist
/// in den Himmel aufzusteigen. Box Collider (Is Trigger = true) wird
/// als Zonenbegrenzung genutzt.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class AscensionZone : MonoBehaviour
{
    [Header("Player Erkennung")]
    [Tooltip("Nur Objekte mit diesem Tag lösen die Zone aus.")]
    [SerializeField] private string playerTag = "Player";

    [Header("Virtual Camera")]
    [SerializeField] private CinemachineCamera ascensionVCam;
    [Tooltip("Vertical FOV der VCam beim Betreten der Zone.")]
    [SerializeField] private float fovStart = 48.4f;
    [Tooltip("Vertical FOV wenn die Bar komplett voll ist (kleinerer Wert = näher rangezoomt).")]
    [SerializeField] private float fovEnd = 25f;

    [Header("Aufladen")]
    [Tooltip("Wie lange SPACE gehalten werden muss, bis die Bar voll ist (Sekunden).")]
    [SerializeField] private float holdDuration = 2f;

    [Header("UI")]
    [SerializeField] private GameObject progressBarRoot;
    [SerializeField] private Image progressBarFill; // Image Type = Filled, Fill Method = Horizontal

    [Header("Vignette")]
    [SerializeField] private Image vignetteImage; // Canvas-Image, startet mit Alpha 0

    [Header("Charging Sound")]
    [SerializeField] private AudioSource chargingAudioSource; // Loop = true, Play On Awake = false
    [SerializeField] private AudioClip chargingClip;
    [Tooltip("Pitch bei Fortschritt 0")]
    [SerializeField] private float chargingPitchStart = 0.9f;
    [Tooltip("Pitch bei Fortschritt 1 (voll aufgeladen)")]
    [SerializeField] private float chargingPitchEnd = 1.3f;

    [Header("Charge Complete SFX")]
    [Tooltip("Separate AudioSource für den One-Shot-Sound, damit sie sich nicht mit dem Charging-Loop ins Gehege kommt.")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip chargeCompleteSfx;

    [Header("Ascension")]
    [Tooltip("Scripts, die beim Aufsteigen deaktiviert werden (z.B. Movement-/Input-Controller).")]
    [SerializeField] private MonoBehaviour[] scriptsToDisableOnAscend;
    [SerializeField] private float ascendTargetY = 20f;
    [SerializeField] private float ascendTweenDuration = 3f;
    [SerializeField] private Ease ascendEase = Ease.InSine;

    [Header("Fade To White")]
    [SerializeField] private Image fadeImage; // Canvas-Image, startet mit Alpha 0
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Scene Load")]
    [SerializeField] private string sceneToLoad;

    private BoxCollider zoneCollider;
    private Transform playerInZone;
    private float progress; // 0..1
    private bool isAscending;

    private void Awake()
    {
        zoneCollider = GetComponent<BoxCollider>();
        zoneCollider.isTrigger = true;

        if (progressBarRoot != null)
            progressBarRoot.SetActive(false);

        if (ascensionVCam != null)
            ascensionVCam.gameObject.SetActive(false);

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }

        if (vignetteImage != null)
        {
            Color vc = vignetteImage.color;
            vc.a = 0f;
            vignetteImage.color = vc;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAscending) return;
        if (!other.CompareTag(playerTag)) return;

        playerInZone = other.transform;
        EnterZone();
    }

    private void OnTriggerExit(Collider other)
    {
        if (isAscending) return;
        if (playerInZone == null || other.transform != playerInZone) return;

        ExitZone();
    }

    private void EnterZone()
    {
        progress = 0f;
        UpdateProgressUI();

        if (progressBarRoot != null)
            progressBarRoot.SetActive(true);

        if (ascensionVCam != null)
        {
            ascensionVCam.gameObject.SetActive(true);
            ApplyZoom(0f);
        }
    }

    private void ExitZone()
    {
        playerInZone = null;
        progress = 0f;

        if (progressBarRoot != null)
            progressBarRoot.SetActive(false);

        if (ascensionVCam != null)
            ascensionVCam.gameObject.SetActive(false);

        UpdateVignette();
        UpdateChargingSound(false);
    }

    private void Update()
    {
        if (isAscending) return;
        if (playerInZone == null) return;

        if (Input.GetKey(KeyCode.Space))
        {
            progress += Time.deltaTime / holdDuration;
            progress = Mathf.Clamp01(progress);

            UpdateProgressUI();
            ApplyZoom(progress);
            UpdateVignette();
            UpdateChargingSound(true);

            if (progress >= 1f)
            {
                StartAscension();
            }
        }
        else
        {
            // Space losgelassen, bevor die Bar voll war -> Fortschritt zurücksetzen
            progress = 0f;
            UpdateProgressUI();
            ApplyZoom(0f);
            UpdateVignette();
            UpdateChargingSound(false);
        }
    }

    private void UpdateProgressUI()
    {
        if (progressBarFill != null)
            progressBarFill.fillAmount = progress;
    }

    private void ApplyZoom(float t)
    {
        if (ascensionVCam == null) return;

        ascensionVCam.Lens.FieldOfView = Mathf.Lerp(fovStart, fovEnd, t);
    }

    private void UpdateVignette()
    {
        if (vignetteImage == null) return;

        Color c = vignetteImage.color;
        c.a = progress;
        vignetteImage.color = c;
    }

    private void UpdateChargingSound(bool isCharging)
    {
        if (chargingAudioSource == null || chargingClip == null) return;

        if (isCharging)
        {
            if (!chargingAudioSource.isPlaying)
            {
                chargingAudioSource.clip = chargingClip;
                chargingAudioSource.loop = true;
                chargingAudioSource.Play();
            }

            chargingAudioSource.pitch = Mathf.Lerp(chargingPitchStart, chargingPitchEnd, progress);
        }
        else
        {
            if (chargingAudioSource.isPlaying)
                chargingAudioSource.Stop();
        }
    }

    private void StartAscension()
    {
        isAscending = true;
        UpdateChargingSound(false); // Loop stoppen, sobald der finale Aufstieg beginnt

        if (sfxAudioSource != null && chargeCompleteSfx != null)
            sfxAudioSource.PlayOneShot(chargeCompleteSfx);

        // Player-Controller deaktivieren, damit die Zone nicht mehr verlassen werden kann
        foreach (var script in scriptsToDisableOnAscend)
        {
            if (script != null)
                script.enabled = false;
        }

        if (progressBarRoot != null)
            progressBarRoot.SetActive(false);

        Vector3 targetPos = playerInZone.position;
        targetPos.y = ascendTargetY;

        playerInZone
            .DOMove(targetPos, ascendTweenDuration)
            .SetEase(ascendEase)
            .OnComplete(FadeToWhite);
    }

    private void FadeToWhite()
    {
        if (fadeImage == null)
        {
            LoadTargetScene();
            return;
        }

        fadeImage.gameObject.SetActive(true);

        fadeImage
            .DOFade(1f, fadeDuration)
            .OnComplete(LoadTargetScene);
    }

    private void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning("AscensionZone: Keine Scene zum Laden angegeben!");
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}