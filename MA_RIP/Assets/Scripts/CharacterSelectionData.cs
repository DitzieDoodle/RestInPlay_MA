using UnityEngine;

public class CharacterSelectionData : MonoBehaviour
{
    public static CharacterSelectionData Instance;

    public int selectedEyesIndex;
    public int selectedMouthIndex;
    public int selectedBodyIndex;
    public int selectedColorIndex;

    const string EyesKey = "Char_Eyes";
    const string MouthKey = "Char_Mouth";
    const string BodyKey = "Char_Body";
    const string ColorKey = "Char_Color";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSelection();   // Beim Start aus PlayerPrefs laden
    }

    public void SaveSelection()
    {
        PlayerPrefs.SetInt(EyesKey, selectedEyesIndex);
        PlayerPrefs.SetInt(MouthKey, selectedMouthIndex);
        PlayerPrefs.SetInt(BodyKey, selectedBodyIndex);
        PlayerPrefs.SetInt(ColorKey, selectedColorIndex);
        PlayerPrefs.Save();   // WICHTIG: tatsächlich auf Disk schreiben
    }

    public void LoadSelection()
    {
        selectedEyesIndex = PlayerPrefs.GetInt(EyesKey, 0);   // 0 = Default
        selectedMouthIndex = PlayerPrefs.GetInt(MouthKey, 0);
        selectedBodyIndex = PlayerPrefs.GetInt(BodyKey, 0);
        selectedColorIndex = PlayerPrefs.GetInt(ColorKey, 0);
    }
}
