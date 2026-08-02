using System.Collections.Generic;
using System.Linq;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    public const string GameStateKey = "GameState";

    public UnityEvent OnGameCanComplete = new();
    public UnityEvent OnGameUpdated = new();

    FlowerHandler flowerHandler;
    ColorHandler colorHandler;
    PlayerController playerController;
    DialogueSystemEvents dialogueSystemEvents;
    Usable selectedUsable;
    Selector playerSelector;
    ProximitySelector proximitySelector;

    ColorHandler.ColorType currentGriefType;



    public Dictionary<ColorHandler.ColorType, bool> colorTypeCompleted { get; private set; } = new();

    public bool CanComplete { get; private set; } = false;

    public string GetGameStateKeyForGriefType(ColorHandler.ColorType griefType)
    {
        return $"{GameStateKey}_{griefType}";
    }


    void Start()
    {
        flowerHandler = FindAnyObjectByType<FlowerHandler>();
        colorHandler = FindAnyObjectByType<ColorHandler>();
        playerController = FindAnyObjectByType<PlayerController>();
        dialogueSystemEvents = FindAnyObjectByType<DialogueSystemEvents>();
        if (dialogueSystemEvents != null)
        {
            dialogueSystemEvents.conversationEvents.onConversationStart.AddListener(OnConversationStart);
            dialogueSystemEvents.conversationEvents.onConversationEnd.AddListener(OnConversationEnd);
        }
        playerSelector = playerController.GetComponent<Selector>();
        playerSelector?.onSelectedUsable.AddListener(OnUsableSelected);
        proximitySelector = playerController.GetComponent<ProximitySelector>();
        proximitySelector?.onSelectedUsable.AddListener(OnUsableSelected);

        colorTypeCompleted[ColorHandler.ColorType.Denial] = false;
        colorTypeCompleted[ColorHandler.ColorType.Anger] = false;
        colorTypeCompleted[ColorHandler.ColorType.Bargaining] = false;
        colorTypeCompleted[ColorHandler.ColorType.Depression] = false;
        colorTypeCompleted[ColorHandler.ColorType.Acceptance] = false;

        LoadGameState();

        OnGameUpdated?.Invoke();
    }

    private void LoadGameState()
    {
        foreach (var colorType in colorTypeCompleted.Keys.ToList())
        {
            string key = GetGameStateKeyForGriefType(colorType);
            bool isCompleted = PlayerPrefs.GetInt(key, 0) == 1;
            colorTypeCompleted[colorType] = isCompleted;
        }
    }

    private void SaveGameState()
    {
        foreach (var kvp in colorTypeCompleted)
        {
            string key = GetGameStateKeyForGriefType(kvp.Key);
            PlayerPrefs.SetInt(key, kvp.Value ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void SetFlowerNone()
    {
        SetLevel(currentGriefType, ColorHandler.ColorLevel.None);
    }

    public void SetFlowerMain()
    {
        SetLevel(currentGriefType, ColorHandler.ColorLevel.Main);
    }

    public void SetFlowerSecondary()
    {
        SetLevel(currentGriefType, ColorHandler.ColorLevel.Secondary);
    }

    public void SetFlowerTertiary()
    {
        SetLevel(currentGriefType, ColorHandler.ColorLevel.Tertiary);
    }

    public void SetGriefToDenial()
    {
        SetCurrentGriefType(ColorHandler.ColorType.Denial);
    }

    public void SetGriefToAnger()
    {
        SetCurrentGriefType(ColorHandler.ColorType.Anger);
    }

    public void SetGriefToBargaining()
    {
        SetCurrentGriefType(ColorHandler.ColorType.Bargaining);
    }

    public void SetGriefToDepression()
    {
        SetCurrentGriefType(ColorHandler.ColorType.Depression);
    }

    public void SetGriefToAcceptance()
    {
        SetCurrentGriefType(ColorHandler.ColorType.Acceptance);
    }



    public void SetCurrentGriefType(ColorHandler.ColorType griefType)
    {
        // This method can be used to set the current grief type if needed
        currentGriefType = griefType;
    }

    public void SetDenialLevelMain()
    {
        SetLevel(ColorHandler.ColorType.Denial, ColorHandler.ColorLevel.Main);
    }
    public void SetDenialLevelSecondary()
    {
        SetLevel(ColorHandler.ColorType.Denial, ColorHandler.ColorLevel.Secondary);
    }
    public void SetDenialLevelTertiary()
    {
        SetLevel(ColorHandler.ColorType.Denial, ColorHandler.ColorLevel.Tertiary);
        // Show Denial Extra GameObjects
    }

    public void SetAngerLevelMain()
    {
        SetLevel(ColorHandler.ColorType.Anger, ColorHandler.ColorLevel.Main);
    }
    public void SetAngerLevelSecondary()
    {
        SetLevel(ColorHandler.ColorType.Anger, ColorHandler.ColorLevel.Secondary);
    }
    public void SetAngerLevelTertiary()
    {
        SetLevel(ColorHandler.ColorType.Anger, ColorHandler.ColorLevel.Tertiary);
        // Show Anger Extra GameObjects
    }

    public void SetBargainingLevelMain()
    {
        SetLevel(ColorHandler.ColorType.Bargaining, ColorHandler.ColorLevel.Main);
    }
    public void SetBargainingLevelSecondary()
    {
        SetLevel(ColorHandler.ColorType.Bargaining, ColorHandler.ColorLevel.Secondary);
    }
    public void SetBargainingLevelTertiary()
    {
        SetLevel(ColorHandler.ColorType.Bargaining, ColorHandler.ColorLevel.Tertiary);
        // Show Bargaining Extra GameObjects
    }

    public void SetDepressionLevelMain()
    {
        SetLevel(ColorHandler.ColorType.Depression, ColorHandler.ColorLevel.Main);
    }
    public void SetDepressionLevelSecondary()
    {
        SetLevel(ColorHandler.ColorType.Depression, ColorHandler.ColorLevel.Secondary);
    }
    public void SetDepressionLevelTertiary()
    {
        SetLevel(ColorHandler.ColorType.Depression, ColorHandler.ColorLevel.Tertiary);
        // Show Depression Extra GameObjects
    }

    public void SetAcceptanceLevelMain()
    {
        SetLevel(ColorHandler.ColorType.Acceptance, ColorHandler.ColorLevel.Main);
    }
    public void SetAcceptanceLevelSecondary()
    {
        SetLevel(ColorHandler.ColorType.Acceptance, ColorHandler.ColorLevel.Secondary);
    }
    public void SetAcceptanceLevelTertiary()
    {
        SetLevel(ColorHandler.ColorType.Acceptance, ColorHandler.ColorLevel.Tertiary);
        // Show Acceptance Extra GameObjects
    }

    public void OnConversationEnd(Transform transform)
    {
        Debug.Log("Conversation ended with: " + transform.name);
        playerController.EnableMovement();
        if (selectedUsable != null)
        {
            selectedUsable.GetComponentInParent<NpcSquash>()?.StopTalkingSquash();
        }
        if (playerSelector) playerSelector.enabled = true;
        if (proximitySelector) proximitySelector.enabled = true;
    }

    public void OnConversationStart(Transform transform)
    {
        Debug.Log("Conversation started with: " + transform.name);
        playerController.DisableMovement();
        if (selectedUsable != null)
        {
            selectedUsable.GetComponentInParent<NpcSquash>()?.StartTalkingSquash();
        }
        if (playerSelector) playerSelector.enabled = false;
        if (proximitySelector) proximitySelector.enabled = false;
    }

    private void OnUsableSelected(Usable usable)
    {
        // Handle the usable selection here
        selectedUsable = usable;
    }

    private void SetLevel(ColorHandler.ColorType colorType, ColorHandler.ColorLevel colorLevel)
    {
        colorHandler.SetLevel(colorType, colorLevel);
        if (colorLevel == ColorHandler.ColorLevel.Main)
        {
            flowerHandler.RemoveFlowers(1);
        }
        else if (colorLevel == ColorHandler.ColorLevel.Secondary)
        {
            flowerHandler.RemoveFlowers(2);
        }
        else if (colorLevel == ColorHandler.ColorLevel.Tertiary)
        {
            flowerHandler.RemoveFlowers(3);
        }

        colorTypeCompleted[colorType] = true;

        CheckGameCompletion();
        SaveGameState();
        OnGameUpdated?.Invoke();
    }

    private void CheckGameCompletion()
    {
        foreach (var completed in colorTypeCompleted.Values)
        {
            if (!completed)
            {
                return; // If any color type is not completed, exit the method
            }
        }

        CanComplete = true;
        OnGameCanComplete?.Invoke();
    }
}
