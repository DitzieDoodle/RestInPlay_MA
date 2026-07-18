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
        // Hier können Sie die Logik implementieren, wenn ein Wort ausgewählt wird.
        // Zum Beispiel könnten Sie überprüfen, ob das ausgewählte Wort korrekt ist.
        if (selectedWord.IsRightWord)
        {
            HasRightWord = true;
            CurrentWordUi = selectedWord;
            OnRightWordSelectedEvent?.Invoke();
        }
        else
        {
            selectedWord.Deny();
        }
    }
}
