using UnityEngine;

public class PlayerPrefsClearOnAwake : MonoBehaviour
{
    void Awake()
    {
        // Löscht alle gespeicherten PlayerPrefs beim Start
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Alle PlayerPrefs wurden in Awake gelöscht.");
    }
}