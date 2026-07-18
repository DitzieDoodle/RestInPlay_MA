using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DenialGameHandler : MonoBehaviour
{
    public GameObject GameWonGameobject;

    CanvasGroup canvasGroup;
    List<DenialGame> denialGames = new List<DenialGame>();

    int currentGameIndex = 0;

    FlowerHandler flowerHandler;

    void Start()
    {
        flowerHandler = FindAnyObjectByType<FlowerHandler>();
        denialGames.AddRange(GetComponentsInChildren<DenialGame>(false));
        canvasGroup = GetComponent<CanvasGroup>();
        foreach (var game in denialGames)
        {
            game.gameObject.SetActive(false);
        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void StartGame()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1f, 0.5f);
        currentGameIndex = 0;
        if (denialGames.Count > 0)
        {
            for (int i = 0; i < denialGames.Count; i++)
            {
                denialGames[i].gameObject.SetActive(i == currentGameIndex);
            }
        }
    }

    public void NextGame()
    {
        if (denialGames.Count > 0)
        {
            denialGames[currentGameIndex].gameObject.SetActive(false);

            currentGameIndex++;

            if (currentGameIndex < denialGames.Count)
            {
                // Activate the next game
                denialGames[currentGameIndex].gameObject.SetActive(true);
            }
            else
            {
                OnCompleteGames();
            }
        }
    }

    private void OnCompleteGames()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0f, 0.5f);
        GameWonGameobject.SetActive(true);
    }
}
