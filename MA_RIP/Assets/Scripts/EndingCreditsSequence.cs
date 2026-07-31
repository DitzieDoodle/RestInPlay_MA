using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// Steuert die Endsequenz: Schwarze Maske deckt den Screen ab, Dialog läuft
/// dahinter, am Ende der Dialogsequenz blendet die Maske aus, ein Song
/// blendet ein und die Credits starten. Über einen Button kann die Maske
/// wieder eingeblendet werden, um danach eine Scene zu laden.
/// </summary>
public class EndingCreditsSequence : MonoBehaviour
{
    [Header("Schwarze Maske")]
    [Tooltip("Vollflächiges schwarzes Canvas-Image, deckt den Screen während des Dialogs ab.")]
    [SerializeField] private Image maskImage;
    [SerializeField] private float maskFadeOutDuration = 1.5f;
    [SerializeField] private float maskFadeInDuration = 1.5f;

    [Header("Credits")]
    [Tooltip("Die CreditsScroller-Komponente. Sollte im Inspector standardmäßig DEAKTIVIERT sein, damit sie erst hier gestartet wird.")]
    [SerializeField] private CreditsScroller creditsScroller;

    [Header("Dialog")]
    [Tooltip("Wird beim Start dieser Sequenz ausgelöst - hier dein Dialogsystem anstoßen (z.B. StartDialogue()).")]
    public UnityEvent OnSequenceStart;

    [Header("Musik")]
    [Tooltip("AudioSource für den Song, der nach dem Fade-Out der Maske einblendet.")]
    [SerializeField] private AudioSource musicAudioSource; // Loop = true, Play On Awake = false
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private float musicTargetVolume = 0.8f;
    [SerializeField] private float musicFadeInDuration = 2f;
    [SerializeField] private float musicFadeOutDuration = 1f;

    [Header("Exit Button")]
    [Tooltip("Button, der die Maske wieder einblendet und danach eine Scene lädt.")]
    [SerializeField] private Button exitButton;
    [SerializeField] private string sceneToLoadOnExit;

    private void Awake()
    {
        if (exitButton != null)
        {
            exitButton.gameObject.SetActive(false);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    private void Start()
    {
        if (maskImage != null)
        {
            Color c = maskImage.color;
            c.a = 1f;
            maskImage.color = c;
            maskImage.gameObject.SetActive(true);
        }

        if (creditsScroller != null)
            creditsScroller.enabled = false;

        if (musicAudioSource != null)
            musicAudioSource.volume = 0f;

        OnSequenceStart?.Invoke();
    }

    /// <summary>
    /// Aus dem Dialogsystem aufrufen, sobald die Dialogsequenz zu Ende ist.
    /// Blendet die Maske aus, startet danach Musik und Credits.
    /// </summary>
    public void NotifyDialogFinished()
    {
        if (maskImage == null)
        {
            StartCredits();
            FadeInMusic();
            return;
        }

        maskImage
            .DOFade(0f, maskFadeOutDuration)
            .OnComplete(() =>
            {
                maskImage.gameObject.SetActive(false);
                StartCredits();
                FadeInMusic();

                if (exitButton != null)
                    exitButton.gameObject.SetActive(true);
            });
    }

    private void StartCredits()
    {
        if (creditsScroller != null)
            creditsScroller.enabled = true;
    }

    private void FadeInMusic()
    {
        if (musicAudioSource == null || musicClip == null) return;

        musicAudioSource.DOKill();

        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true;
        musicAudioSource.volume = 0f;
        musicAudioSource.Play();

        musicAudioSource.DOFade(musicTargetVolume, musicFadeInDuration);
    }

    private void FadeOutMusic()
    {
        if (musicAudioSource == null) return;

        musicAudioSource.DOKill();

        musicAudioSource
            .DOFade(0f, musicFadeOutDuration)
            .OnComplete(() => musicAudioSource.Stop());
    }

    private void OnExitButtonClicked()
    {
        if (exitButton != null)
            exitButton.interactable = false; // verhindert Doppelklick während des Fade-Outs

        if (creditsScroller != null)
            creditsScroller.enabled = false;

        FadeOutMusic();

        if (maskImage == null)
        {
            LoadExitScene();
            return;
        }

        maskImage.gameObject.SetActive(true);

        maskImage
            .DOFade(1f, maskFadeInDuration)
            .OnComplete(LoadExitScene);
    }

    private void LoadExitScene()
    {
        if (string.IsNullOrEmpty(sceneToLoadOnExit))
        {
            Debug.LogWarning("EndingCreditsSequence: Keine Scene für den Exit-Button angegeben!");
            return;
        }

        SceneManager.LoadScene(sceneToLoadOnExit);
    }
}