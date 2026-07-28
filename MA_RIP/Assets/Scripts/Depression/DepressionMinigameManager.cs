using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DepressionMinigameManager : MonoBehaviour
{
    public static DepressionMinigameManager Instance { get; private set; }

    [Header("Water")]
    [SerializeField] private Transform waterBlock;
    [SerializeField] private float minWaterY = -2f;
    [SerializeField] private float maxWaterY = 2f;
    [SerializeField] private float waterRiseInterval = 3f;
    [SerializeField] private float waterRiseStep = 0.1f;
    [SerializeField] private float waterFallStep = 0.35f;

    [Header("Candles")]
    [SerializeField] private List<Candle> candles = new List<Candle>();

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip waterMaxReachedSfx;
    [SerializeField] private AudioClip depressionWonSfx;

    [Header("Screen Fader")]
    [SerializeField] private SceneFader screenFader;

    private float currentWaterY;
    private bool minigameRunning;
    private bool allCandlesLit;
    private Candle currentTarget;
    private Coroutine waterRiseCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentWaterY = minWaterY;
        SetWaterY(currentWaterY);
    }

    public void StartMinigame()
    {
        if (minigameRunning) return;

        minigameRunning = true;
        allCandlesLit = false;
        currentTarget = null;

        foreach (var candle in candles)
            candle.ResetCandle();

        if (waterRiseCoroutine != null)
            StopCoroutine(waterRiseCoroutine);

        waterRiseCoroutine = StartCoroutine(WaterRiseLoop());

        PickRandomTarget();
    }

    private IEnumerator WaterRiseLoop()
    {
        while (minigameRunning && !allCandlesLit)
        {
            yield return new WaitForSeconds(waterRiseInterval);

            if (!minigameRunning || allCandlesLit)
                yield break;

            currentWaterY = Mathf.Min(currentWaterY + waterRiseStep, maxWaterY);
            SetWaterY(currentWaterY);

            if (currentWaterY >= maxWaterY)
            {
                OnWaterMaxReached();
                yield break;
            }
        }
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

        candle.Light();
        currentTarget = null;

        currentWaterY = Mathf.Max(currentWaterY - waterFallStep, minWaterY);
        SetWaterY(currentWaterY);

        if (currentWaterY >= maxWaterY)
        {
            OnWaterMaxReached();
            return;
        }

        CheckWinState();

        if (minigameRunning && !allCandlesLit)
            PickRandomTarget();
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

        if (waterRiseCoroutine != null)
            StopCoroutine(waterRiseCoroutine);

        ClearAllIndicators();

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

        if (waterRiseCoroutine != null)
            StopCoroutine(waterRiseCoroutine);

        ClearAllIndicators();

        if (audioSource != null && waterMaxReachedSfx != null)
            audioSource.PlayOneShot(waterMaxReachedSfx);

        if (screenFader != null)
            screenFader.FadeToScene();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    private void SetWaterY(float y)
    {
        if (waterBlock == null) return;

        Vector3 pos = waterBlock.position;
        pos.y = y;
        waterBlock.position = pos;
    }
}