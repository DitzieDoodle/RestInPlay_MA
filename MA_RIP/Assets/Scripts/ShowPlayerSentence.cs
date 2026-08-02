using TMPro;
using UnityEngine;

public class ShowPlayerSentence : MonoBehaviour
{
    string sentenceKey => DenialGame.SaveSentenceKey;
    TMP_Text sentenceText;

    void Awake()
    {
        sentenceText = GetComponent<TMP_Text>();
    }
    float nextTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime)
        {
            nextTime = Time.time + 0.5f;
            string sentence = PlayerPrefs.GetString(sentenceKey, null);
            if (!string.IsNullOrWhiteSpace(sentence))
            {
                sentenceText.text = sentence;
                enabled = false;
            }
        }
    }
}
