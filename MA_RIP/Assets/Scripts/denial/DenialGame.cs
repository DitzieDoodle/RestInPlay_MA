using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DenialGame : MonoBehaviour
{
    List<WordSlotUi> wordSlots = new List<WordSlotUi>();
    public Button nextButton;
    public string SaveSentenceKey = "";
    public bool PrefixWithPlayerName = true;

    DenialGameHandler denialGameHandler;

    void Start()
    {
        // Find all WordSlotUi components in the scene and add them to the list
        wordSlots.AddRange(GetComponentsInChildren<WordSlotUi>());
        nextButton.onClick.AddListener(OnNextButtonClicked);

        nextButton.interactable = false; // Disable the button initially

        denialGameHandler = FindAnyObjectByType<DenialGameHandler>();
        foreach (var wordSlot in wordSlots)
        {
            wordSlot.OnRightWordSelectedEvent.AddListener(() =>
            {
                if (AllWordSlotsHaveRightWords())
                {
                    nextButton.interactable = true; // Enable the button when all slots have the right word
                }
            });
        }
    }

    private bool AllWordSlotsHaveRightWords()
    {
        foreach (var wordSlot in wordSlots)
        {
            if (!wordSlot.HasRightWord)
            {
                return false;
            }
        }
        return true;
    }

    private void OnNextButtonClicked()
    {
        SaveSentence();
        denialGameHandler.NextGame();
    }

    private void SaveSentence()
    {
        if (!string.IsNullOrEmpty(SaveSentenceKey))
        {
            string sentence = "";
            foreach (var wordSlot in wordSlots)
            {
                sentence += wordSlot.CurrentWordUi.GetWordText() + " ";
            }
            sentence = sentence.Trim(); // Remove any trailing space

            if (PrefixWithPlayerName)
            {
                string playerName = PlayerPrefs.GetString(SkinsMatcher.NAME_KEY, SkinsMatcher.BASE_NAME);
                sentence = $"{playerName} {sentence}";
            }

            PlayerPrefs.SetString(SaveSentenceKey, sentence);
        }
    }
}
