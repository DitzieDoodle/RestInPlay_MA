using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorHandler : MonoBehaviour
{
    public enum ColorType
    {
        Acceptance,
        Anger,
        Denial,
        Depression,
        Bargaining,
    }

    public enum ColorLevel
    {
        None = 0,
        Main = 1,
        Secondary = 2,
        Tertiary = 3,
    }

    [System.Serializable]
    public class ColorMapping
    {
        public ColorType colorType;
        public ColorLevel colorSlot;
        public Color color;

        public ColorMapping(ColorType type, ColorLevel slot, Color col)
        {
            colorType = type;
            colorSlot = slot;
            color = col;
        }
    }


    public List<ColorMapping> colorMap = new List<ColorMapping>
    {
        new ColorMapping(ColorType.Denial, ColorLevel.Main, Color.yellow),
        new ColorMapping(ColorType.Denial, ColorLevel.Secondary, Color.yellow),
        new ColorMapping(ColorType.Anger, ColorLevel.Main, Color.red),
        new ColorMapping(ColorType.Anger, ColorLevel.Secondary, Color.red),
        new ColorMapping(ColorType.Bargaining, ColorLevel.Main, Color.magenta),
        new ColorMapping(ColorType.Bargaining, ColorLevel.Secondary, Color.magenta),
        new ColorMapping(ColorType.Depression, ColorLevel.Main, Color.blue),
        new ColorMapping(ColorType.Depression, ColorLevel.Secondary, Color.blue),
        new ColorMapping(ColorType.Acceptance, ColorLevel.Main, Color.green),
        new ColorMapping(ColorType.Acceptance, ColorLevel.Secondary, Color.green),
    };

    Dictionary<ColorType, ColorLevel> colorLevels = new Dictionary<ColorType, ColorLevel>();
    Dictionary<(ColorType, ColorLevel), Color> colorMappingsDict = new Dictionary<(ColorType, ColorLevel), Color>();

    void Start()
    {
        colorLevels[ColorType.Acceptance] = ColorLevel.None;
        colorLevels[ColorType.Anger] = ColorLevel.None;
        colorLevels[ColorType.Denial] = ColorLevel.None;
        colorLevels[ColorType.Depression] = ColorLevel.None;
        colorLevels[ColorType.Bargaining] = ColorLevel.None;

        colorMappingsDict.Clear();
        foreach (var mapping in colorMap)
        {
            colorMappingsDict[(mapping.colorType, mapping.colorSlot)] = mapping.color;
        }

        UpdateColorableSprites();
    }

    public void AddLevelAcceptance()
    {
        AddLevel(ColorType.Acceptance);
    }

    public void AddLevelAnger()
    {
        AddLevel(ColorType.Anger);
    }

    public void AddLevelDenial()
    {
        AddLevel(ColorType.Denial);
    }

    public void AddLevelDepression()
    {
        AddLevel(ColorType.Depression);
    }

    public void AddLevelBargaining()
    {
        AddLevel(ColorType.Bargaining);
    }

    public void SetLevel(ColorType colorType, ColorLevel level)
    {
        colorLevels[colorType] = level;
        UpdateColorableSprites();
    }

    public void AddLevel(ColorType colorType)
    {
        if (colorLevels.ContainsKey(colorType))
        {
            ColorLevel currentLevel = colorLevels[colorType];
            ColorLevel nextLevel = GetNextLevel(currentLevel);
            colorLevels[colorType] = nextLevel;
        }

        UpdateColorableSprites();
    }

    public void UpdateColorableSprites()
    {
        ColorableSprite[] colorableSprites = FindObjectsByType<ColorableSprite>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        var colors = new List<Color>();
        colors.AddRange(GetColorsForColorType(ColorType.Acceptance));
        colors.AddRange(GetColorsForColorType(ColorType.Anger));
        colors.AddRange(GetColorsForColorType(ColorType.Denial));
        colors.AddRange(GetColorsForColorType(ColorType.Depression));
        colors.AddRange(GetColorsForColorType(ColorType.Bargaining));

        if (colors.Count == 0)
        {
            colors.Add(Color.white); // Default color if no colors are active
        }

        int spritesPerColor = Mathf.CeilToInt((float)colorableSprites.Length / colors.Count);
        for (int i = 0; i < colors.Count; i++)
        {
            Color color = colors[i];
            for (int j = 0; j < spritesPerColor; j++)
            {
                int spriteIndex = i * spritesPerColor + j;
                if (spriteIndex < colorableSprites.Length)
                {
                    colorableSprites[spriteIndex].GetComponent<SpriteRenderer>().color = color;
                }
            }
        }
    }

    private List<Color> GetColorsForColorType(ColorType colorType)
    {
        List<Color> colors = new List<Color>();

        foreach (var mapping in colorMap)
        {
            if (mapping.colorType == colorType && IsLevelActive(colorType, mapping.colorSlot))
            {
                colors.Add(mapping.color);
            }
        }

        return colors;
    }

    private ColorLevel GetNextLevel(ColorLevel currentLevel)
    {
        switch (currentLevel)
        {
            case ColorLevel.None:
                return ColorLevel.Main;
            case ColorLevel.Main:
                return ColorLevel.Secondary;
            case ColorLevel.Secondary:
                return ColorLevel.Secondary;
            default:
                return ColorLevel.None;
        }
    }
    private bool IsLevelActive(ColorType colorType, ColorLevel colorLevel)
    {
        return colorLevels.ContainsKey(colorType) && colorLevels[colorType] >= colorLevel;
    }
}