using TMPro;
using UnityEngine;

public class FlowerUi : MonoBehaviour
{
    public TMP_Text flowerCountText;
    FlowerHandler flowerHandler;

    void Start()
    {
        flowerHandler = FindAnyObjectByType<FlowerHandler>();
        flowerHandler.OnFlowerChangedEvent.AddListener(UpdateFlowerCount);
        UpdateFlowerCount();
    }

    void UpdateFlowerCount()
    {
        flowerCountText.text = flowerHandler.FlowerCount.ToString();
    }
}
