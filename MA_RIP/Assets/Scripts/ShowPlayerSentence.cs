using TMPro;
using UnityEngine;

public class ShowPlayerSentence : MonoBehaviour
{
    TMP_Text sentenceText;

    void Awake()
    {
        sentenceText = GetComponent<TMP_Text>();
    }
    float nextTime = 0f;
    public string sentenceKey = "PlayerSentence";

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime)
        {
            nextTime = Time.time + 0.5f;
            string sentence = PlayerPrefs.GetString(sentenceKey, "");
            if (sentenceText != null)
            {
                sentenceText.text = sentence;
            }
        }
    }
}
