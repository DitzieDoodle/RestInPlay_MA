using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class DepressionMinigameManager : MonoBehaviour
{
    public static DepressionMinigameManager Instance { get; private set; }

    [Header("Water")]
    [SerializeField] private Transform waterBlock;
    [SerializeField] private float minWaterY = -2f;
    [SerializeField] private float maxWaterY = 2f;
    [SerializeField] private float waterRiseDuration = 30f; // Zeit von min -> max, wenn nie unterbrochen
    [SerializeField] private float waterFallStep = 0.35f;
    [SerializeField] private float waterTweenDuration = 0.6f;

    [Header("Player Speed")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float speedAtMinWater = 1f;
    [SerializeField] private float speedAtMaxWater = 0.25f;

    [Header("Atmosphere")]
    [SerializeField] private CanvasGroup vignetteCanvasGroup;
    [SerializeField] private float vignetteAlphaAtMin = 0f;
    [SerializeField] private float vignetteAlphaAtMax = 0.75f;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float musicPitchAtMin = 1f;
    [SerializeField] private float musicPitchAtMax = 0.85f;

    [Header("Screen Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private string sceneToLoad; // Name der Scene, im Inspector setzen

    [Header("Candles")]
    [SerializeField] private List<Candle> candles = new List<Candle>();

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip waterMaxReachedSfx;
    [SerializeField] private AudioClip depressionWonSfx;



    private float currentWaterY;
    private bool minigameRunning;
    private bool allCandlesLit;
    private Candle currentTarget;
    private Tween waterTween;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentWaterY = minWaterY;
        if (waterBlock != null)
            waterBlock.position = new Vector3(waterBlock.position.x, currentWaterY, waterBlock.position.z);

        ApplyPlayerSpeed();
        ApplyAtmosphere();
    }

    private void Update()
    {
        if (!minigameRunning) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget != null)
                TryLightCandle(currentTarget);
        }
    }

    public void StartMinigame()
    {
        if (minigameRunning) return;

        minigameRunning = true;
        allCandlesLit = false;
        currentTarget = null;
        currentWaterY = minWaterY;

        foreach (var candle in candles)
            candle.ResetCandle();

        if (waterBlock != null)
            waterBlock.position = new Vector3(waterBlock.position.x, currentWaterY, waterBlock.position.z);

        PickRandomTarget();

        ApplyPlayerSpeed();
        ApplyAtmosphere();

        StartRiseTween(waterRiseDuration);
    }

    // Startet (bzw. setzt fort) das kontinuierliche Ansteigen zu maxWaterY über die übergebene Dauer
    private void StartRiseTween(float duration)
    {
        if (waterBlock == null) return;

        if (waterTween != null && waterTween.IsActive())
            waterTween.Kill();

        waterTween = waterBlock.DOMoveY(maxWaterY, duration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                currentWaterY = waterBlock.position.y;
                ApplyPlayerSpeed();
                ApplyAtmosphere();
            })
            .OnComplete(() =>
            {
                currentWaterY = maxWaterY;
                OnWaterMaxReached();
            });
    }

    private void PickRandomTarget()
    {
        ClearAllIndicators();

        List<Candle> available = new List<Candle>();
        foreach (var candle in candles)
        {
            if (!candle.IsLit)
                available.Add(candle);
        }

        if (available.Count == 0)
        {
            allCandlesLit = true;
            OnAllCandlesLit();
            return;
        }

        currentTarget = available[Random.Range(0, available.Count)];
        currentTarget.SetTarget(true);
    }

    public void TryLightCandle(Candle candle)
    {
        if (!minigameRunning) return;
        if (candle == null) return;
        if (candle.IsLit) return;
        if (candle != currentTarget) return;
        if (!candle.IsPlayerInRange) return;

        candle.Light();
        currentTarget = null;

        // Anstieg stoppen, Wasser kurz absenken, danach mit gleicher Geschwindigkeit weitersteigen lassen
        if (waterTween != null && waterTween.IsActive())
            waterTween.Kill();

        float targetY = Mathf.Max(currentWaterY - waterFallStep, minWaterY);

        waterBlock.DOMoveY(targetY, waterTweenDuration)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() =>
            {
                currentWaterY = waterBlock.position.y;
                ApplyPlayerSpeed();
                ApplyAtmosphere();
            })
            .OnComplete(() =>
            {
                currentWaterY = targetY;
                ApplyPlayerSpeed();
                ApplyAtmosphere();

                CheckWinState();

                if (minigameRunning && !allCandlesLit)
                {
                    PickRandomTarget();

                    // Restdauer proportional zur verbleibenden Strecke, damit die Steig-Geschwindigkeit konstant bleibt
                    float totalDistance = maxWaterY - minWaterY;
                    float remainingDistance = maxWaterY - currentWaterY;
                    float remainingDuration = totalDistance > 0f
                        ? waterRiseDuration * (remainingDistance / totalDistance)
                        : 0f;

                    StartRiseTween(remainingDuration);
                }
            });
    }

    private void CheckWinState()
    {
        foreach (var candle in candles)
        {
            if (!candle.IsLit)
                return;
        }

        allCandlesLit = true;
        OnAllCandlesLit();
    }

    private void OnAllCandlesLit()
    {
        minigameRunning = false;

        if (waterTween != null && waterTween.IsActive())
            waterTween.Kill();

        ClearAllIndicators();
        ResetPlayerSpeed();
        ResetAtmosphere();

        if (currentWaterY < maxWaterY)
        {
            DepressionWon();
        }
        else
        {
            OnWaterMaxReached();
        }
    }

    private void OnWaterMaxReached()
    {
        minigameRunning = false;
        allCandlesLit = false;

        if (waterTween != null && waterTween.IsActive())
            waterTween.Kill();

        ClearAllIndicators();
        ResetPlayerSpeed();
        ResetAtmosphere();

        if (audioSource != null && waterMaxReachedSfx != null)
            audioSource.PlayOneShot(waterMaxReachedSfx);

        Debug.Log("WaterMaxReached");

        FadeToBlackAndLoadScene();
    }

    private void FadeToBlackAndLoadScene()
    {
        Debug.Log("check1");
        if (fadeCanvasGroup == null)
        {
            Debug.LogWarning("Kein fadeCanvasGroup zugewiesen – Scene wird direkt geladen.");
            SceneManager.LoadScene(sceneToLoad);
            return;
        }

        Debug.Log("check");
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = true;

        fadeCanvasGroup.DOFade(1f, fadeDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(sceneToLoad);
               
            });
    }

    public void DepressionWon()
    {
        if (audioSource != null && depressionWonSfx != null)
            audioSource.PlayOneShot(depressionWonSfx);

        Debug.Log("DepressionWon() called");
    }

    private void ClearAllIndicators()
    {
        foreach (var candle in candles)
        {
            if (candle != null)
                candle.SetTarget(false);
        }
    }

    private void ApplyPlayerSpeed()
    {
        if (playerController == null) return;

        float t = Mathf.InverseLerp(minWaterY, maxWaterY, currentWaterY);
        float waterMultiplier = Mathf.Lerp(speedAtMinWater, speedAtMaxWater, t);

        playerController.SetWaterSpeedMultiplier(waterMultiplier);
    }

    private void ResetPlayerSpeed()
    {
        if (playerController == null) return;

        playerController.ResetWaterSpeedMultiplier();
    }

    private void ApplyAtmosphere()
    {
        float t = Mathf.InverseLerp(minWaterY, maxWaterY, currentWaterY);

        if (vignetteCanvasGroup != null)
            vignetteCanvasGroup.alpha = Mathf.Lerp(vignetteAlphaAtMin, vignetteAlphaAtMax, t);

        if (musicSource != null)
            musicSource.pitch = Mathf.Lerp(musicPitchAtMin, musicPitchAtMax, t);
    }

    private void ResetAtmosphere()
    {
        if (vignetteCanvasGroup != null)
            vignetteCanvasGroup.alpha = vignetteAlphaAtMin;

        if (musicSource != null)
            musicSource.pitch = musicPitchAtMin;
    }
}