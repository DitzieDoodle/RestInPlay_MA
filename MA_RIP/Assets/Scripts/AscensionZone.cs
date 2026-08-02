using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.Cinemachine;

/// <summary>
/// Zone, in der der Spieler SPACE gedr�ckt halten muss, um als Geist
/// in den Himmel aufzusteigen. Box Collider (Is Trigger = true) wird
/// als Zonenbegrenzung genutzt.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class AscensionZone : MonoBehaviour
{
    [Header("Player Erkennung")]
    [Tooltip("Nur Objekte mit diesem Tag l�sen die Zone aus.")]
    [SerializeField] private string playerTag = "Player";

    [Header("Virtual Camera")]
    [SerializeField] private CinemachineCamera ascensionVCam;
    [Tooltip("Vertical FOV der VCam beim Betreten der Zone.")]
    [SerializeField] private float fovStart = 48.4f;
    [Tooltip("Vertical FOV wenn die Bar komplett voll ist (kleinerer Wert = n�her rangezoomt).")]
    [SerializeField] private float fovEnd = 25f;

    [Header("Aufladen")]
    [Tooltip("Wie lange SPACE gehalten werden muss, bis die Bar voll ist (Sekunden).")]
    [SerializeField] private float holdDuration = 2f;

    [Header("UI")]
    [SerializeField] private GameObject progressBarRoot;
    [SerializeField] private Image progressBarFill; // Image Type = Filled, Fill Method = Horizontal
    [Tooltip("Das 'Hold Space' TMP-Textobjekt. Wird nur angezeigt, solange der Player in der Zone steht.")]
    [SerializeField] private GameObject holdSpaceHint;

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
    [Tooltip("Separate AudioSource f�r den One-Shot-Sound, damit sie sich nicht mit dem Charging-Loop ins Gehege kommt.")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip chargeCompleteSfx;

    [Header("Zone Ambient Sound (Fade In/Out)")]
    [Tooltip("Eigene AudioSource, l�uft dauerhaft auf Loop solange der Player in der Zone steht, ohne Pitch-/Charge-Bezug.")]
    [SerializeField] private AudioSource ambientAudioSource; // Loop = true, Play On Awake = false
    [SerializeField] private AudioClip ambientClip;
    [SerializeField] private float ambientTargetVolume = 1f;
    [SerializeField] private float ambientFadeInDuration = 1f;
    [SerializeField] private float ambientFadeOutDuration = 1f;

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

    bool isEnabled = false;

    GameHandler gameHandler;

    private void Awake()
    {
        Initialize();

        gameHandler = FindAnyObjectByType<GameHandler>();
    }

    void Initialize()
    {
        zoneCollider = GetComponent<BoxCollider>();
        zoneCollider.isTrigger = true;

        if (progressBarRoot != null)
            progressBarRoot.SetActive(false);

        if (holdSpaceHint != null)
            holdSpaceHint.SetActive(false);

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

        if (ambientAudioSource != null)
        {
            ambientAudioSource.loop = true;
            ambientAudioSource.volume = 0f;
        }
    }

    void Start()
    {
        CheckEnabled();
    }

    void CheckEnabled()
    {
        if (gameHandler == null) return;

        if (!gameHandler.CanComplete)
        {
            isEnabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            gameHandler.OnGameCanComplete.AddListener(CheckEnabled);
            return;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        Initialize();
        isEnabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled) return;
        if (isAscending) return;
        if (!other.CompareTag(playerTag)) return;

        playerInZone = other.transform;
        EnterZone();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isEnabled) return;
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

        if (holdSpaceHint != null)
            holdSpaceHint.SetActive(true);

        if (ascensionVCam != null)
        {
            ascensionVCam.gameObject.SetActive(true);
            ApplyZoom(0f);
        }

        FadeInAmbientSound();
    }

    private void ExitZone()
    {
        playerInZone = null;
        progress = 0f;

        if (progressBarRoot != null)
            progressBarRoot.SetActive(false);

        if (holdSpaceHint != null)
            holdSpaceHint.SetActive(false);

        if (ascensionVCam != null)
            ascensionVCam.gameObject.SetActive(false);

        UpdateVignette();
        UpdateChargingSound(false);
        FadeOutAmbientSound();
    }

    private void Update()
    {
        if (!isEnabled) return;
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
            // Space losgelassen, bevor die Bar voll war -> Fortschritt zur�cksetzen
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

    private void FadeInAmbientSound()
    {
        if (ambientAudioSource == null || ambientClip == null) return;

        ambientAudioSource.DOKill();

        if (!ambientAudioSource.isPlaying)
        {
            ambientAudioSource.clip = ambientClip;
            ambientAudioSource.loop = true;
            ambientAudioSource.volume = 0f;
            ambientAudioSource.Play();
        }

        ambientAudioSource.DOFade(ambientTargetVolume, ambientFadeInDuration);
    }

    private void FadeOutAmbientSound()
    {
        if (ambientAudioSource == null) return;

        ambientAudioSource.DOKill();

        ambientAudioSource
            .DOFade(0f, ambientFadeOutDuration)
            .OnComplete(() => ambientAudioSource.Stop());
    }

    private void StartAscension()
    {
        isAscending = true;
        UpdateChargingSound(false); // Loop stoppen, sobald der finale Aufstieg beginnt

        if (holdSpaceHint != null)
            holdSpaceHint.SetActive(false);

        // Player als Tracking Target entfernen -> Kamera bleibt stehen,
        // man sieht den Player (Schatten) aus der Vcam-Position aufsteigen.
        if (ascensionVCam != null)
            ascensionVCam.Target.TrackingTarget = null;

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