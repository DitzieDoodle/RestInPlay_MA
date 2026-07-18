using UnityEngine;
using UnityEngine.Events;
using PixelCrushers.DialogueSystem;
public class FlowerHandler : MonoBehaviour
{
    public int InitialFlowerCount = 2;
    public const string FlowerCountKey = "FlowerCount";
    public UnityEvent OnFlowerChangedEvent = new();
    public int FlowerCount { get; private set; } = 0;

    void Start()
    {
        FlowerCount = PlayerPrefs.GetInt(FlowerCountKey, InitialFlowerCount);
        OnFlowerChangedEvent?.Invoke();
        PushToDialogueSystem();
    }

    public void Initialize()
    {
        FlowerCount = InitialFlowerCount;
        PlayerPrefs.SetInt(FlowerCountKey, FlowerCount);
        PlayerPrefs.Save();
        OnFlowerChangedEvent?.Invoke();
        PushToDialogueSystem();
    }

    void PushToDialogueSystem()
    {
        DialogueLua.SetVariable("FlowerCount", FlowerCount);
    }

    public void AddFlower()
    {
        FlowerCount++;
        PlayerPrefs.SetInt(FlowerCountKey, FlowerCount);
        PlayerPrefs.Save();
        OnFlowerChangedEvent?.Invoke();
        PushToDialogueSystem();
    }

    public void RemoveFlowers(int count)
    {
        FlowerCount = Mathf.Max(0, FlowerCount - count);
        PlayerPrefs.SetInt(FlowerCountKey, FlowerCount);
        PlayerPrefs.Save();
        OnFlowerChangedEvent?.Invoke();
        PushToDialogueSystem();
    }

    public void RemoveOneFlower()
    {
        RemoveFlowers(1);
        Debug.Log("Removed one flower. Current count: " + FlowerCount);
    }

    public void RemoveTwoFlowers()
    {
        RemoveFlowers(2);
        Debug.Log("Removed two flower. Current count: " + FlowerCount);
    }

    public void RemoveThreeFlowers()
    {
        RemoveFlowers(3);
        Debug.Log("Removed three flower. Current count: " + FlowerCount);
    }
}