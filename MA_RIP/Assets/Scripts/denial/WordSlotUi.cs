using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WordSlotUi : MonoBehaviour
{
    public bool HasRightWord { get; private set; } = false;
    public UnityEvent OnRightWordSelectedEvent = new();
    public List<WordUi> wordUis = new List<WordUi>();
    public WordUi CurrentWordUi { get; private set; } = null;

    void Start()
    {
        foreach (var wordUi in wordUis)
        {
            wordUi.SetParentSlot(this);
        }
    }

    public void OnWordSelected(WordUi selectedWord)
    {
        if (selectedWord.IsRightWord)
        {
            HasRightWord = true;
            CurrentWordUi = selectedWord;
            selectedWord.PlayCorrectFeedback();
            OnRightWordSelectedEvent?.Invoke();
        }
        else
        {
            selectedWord.Deny();
        }
    }
}