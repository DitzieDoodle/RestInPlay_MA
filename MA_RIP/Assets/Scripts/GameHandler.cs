using UnityEngine;

public class GameHandler : MonoBehaviour
{
    FlowerHandler flowerHandler;
    ColorHandler colorHandler;

    void Start()
    {
        flowerHandler = FindAnyObjectByType<FlowerHandler>();
        colorHandler = FindAnyObjectByType<ColorHandler>();
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
    }
}
