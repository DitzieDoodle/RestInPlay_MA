using UnityEngine;

public class GriefGameObjectHolder : MonoBehaviour
{
    [SerializeField] ColorHandler.ColorType griefType;
    GameHandler gameHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameHandler = FindAnyObjectByType<GameHandler>();
        if (gameHandler != null)
        {
            gameHandler.OnGameUpdated.AddListener(UpdateGriefGameObjects);
            UpdateGriefGameObjects();
        }
    }

    void UpdateGriefGameObjects()
    {
        if (gameHandler.colorTypeCompleted.TryGetValue(griefType, out bool isCompleted))
        {
            SetChildrenActive(isCompleted);
        }
        else
        {
            SetChildrenActive(false);
        }
    }

    void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
