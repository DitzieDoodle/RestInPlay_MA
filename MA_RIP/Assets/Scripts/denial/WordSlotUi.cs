using System.Collections.Generic;
using UnityEngine;

public class WordSlotUi : MonoBehaviour
{
    public List<WordUi> wordUis = new List<WordUi>();


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
            Debug.Log("Richtiges Wort ausgewählt!");
        }
        else
        {
            Debug.Log("Falsches Wort ausgewählt!");
        }
    }
}
