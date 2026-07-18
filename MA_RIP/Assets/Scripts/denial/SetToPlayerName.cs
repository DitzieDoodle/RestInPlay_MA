using TMPro;
using UnityEngine;

public class SetToPlayerName : MonoBehaviour
{
    public TMP_Text textComponent;

    void Awake()
    {
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<TMP_Text>();
        }
        if (textComponent == null)
        {
            Debug.LogError("TMP_Text component not found on this GameObject.");
            return;
        }

        // Set the text to the player's name
        textComponent.text = PlayerPrefs.GetString(SkinsMatcher.NAME_KEY, "Player");
    }
}
