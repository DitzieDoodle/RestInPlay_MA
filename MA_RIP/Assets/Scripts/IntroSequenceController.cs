using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Cinemachine;

public class IntroSequenceController : MonoBehaviour
{
    [Header("Referenzen")]
    [SerializeField] private Image blackFadeImage;      // Schwarzes Image in der Extra-Canvas
    [SerializeField] private GameObject fadeCanvasObject; // Optional: ganze Canvas, um sie danach zu deaktivieren
    [SerializeField] private Transform player;
    [SerializeField] private MonoBehaviour playerController; // z.B. dein PlayerMovement-Script

    [Header("Kameras")]
    [SerializeField] private CinemachineCamera stageVCam;   // Steht bereit, an beim Start
    [SerializeField] private CinemachineCamera playerVCam;  // Folgt dem Player, aus beim Start

    [Header("Landing Ziel")]
    [SerializeField] private Transform landingTarget; // Zielposition auf dem Boden

    [Header("Timings")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float descentDuration = 3f;
    [SerializeField] private Ease descentEase = Ease.OutQuad; // sanftes Abbremsen beim Landen
    [SerializeField] private float delayBeforeDescent = 0.3f; // kleine Pause nach dem Fade

    private void Awake()
    {
    }

    /// <summary>
    /// Startet die komplette Intro-Sequenz von auﬂen.
    /// </summary>
    public void StartIntroSequence()
    {
        Sequence introSequence = DOTween.Sequence();

        // 1. Canvas ausfaden
        introSequence.Append(blackFadeImage.DOFade(0f, fadeDuration));

        // Canvas GameObject danach deaktivieren (damit sie z.B. keine Raycasts mehr blockt)
        introSequence.AppendCallback(() =>
        {
            if (fadeCanvasObject != null)
                fadeCanvasObject.SetActive(false);
        });

        /*
        // Kurze Pause vor dem Fallen
        introSequence.AppendInterval(delayBeforeDescent);

        // 2. Player von oben auf Zielposition fallen lassen (nur Y-Achse)
        introSequence.Append(
            player.DOMoveY(landingTarget.position.y, descentDuration)
                  .SetEase(descentEase)
        );

        // 3. Nach der Landung: Controller aktivieren + Kamera umschalten
        introSequence.AppendCallback(() =>
        {
            if (playerController != null)
                playerController.enabled = true;
            else
                Debug.LogWarning("playerController ist nicht zugewiesen!");

            if (stageVCam != null)
                stageVCam.enabled = false;
            else
                Debug.LogWarning("stageVCam ist nicht zugewiesen!");

            if (playerVCam != null)
                playerVCam.enabled = true;
            else
                Debug.LogWarning("playerVCam ist nicht zugewiesen!");
        });
        */
    }
}