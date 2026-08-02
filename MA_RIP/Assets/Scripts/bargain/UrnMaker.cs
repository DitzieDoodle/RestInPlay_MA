using System;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Audio;

public class UrnMaker : MonoBehaviour
{
    public const string UrnStateKey = "UrnState";

    public enum UrnComponent
    {
        Urn,
        Decor,
        Color
    }

    public class SaveStateData
    {
        public string Key { get; set; }
        public UrnComponent Component { get; set; }
        public Action LoadAction { get; set; }
    }

    public List<SaveStateData> saveStateDataArray = new List<SaveStateData>();

    [Header("Sprites")]
    public Sprite metal;
    public Sprite wood;
    public Sprite ceramic;
    public Sprite jar;

    [Header("Decor")]
    public Sprite whoops;
    public Sprite cross;
    public Sprite flower;
    public Sprite star;
    public Sprite heart;

    [Header("Sounds")]

    public AudioSource audioSource;

    public AudioClip metalSFX;
    public AudioClip woodSFX;
    public AudioClip ceramicSFX;
    public AudioClip jarSFX;

    public AudioClip whoopsSFX;
    public AudioClip crossSFX;
    public AudioClip flowerSFX;
    public AudioClip starSFX;
    public AudioClip heartSFX;

    public SpriteRenderer UrnRenderer;
    public SpriteRenderer DecorRenderer;


    bool initialized = false;

    void Awake()
    {
        // AudioSource entweder von diesem GameObject oder neu erstellen
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        FillSaveStateData();
    }

    void FillSaveStateData()
    {
        saveStateDataArray = new List<SaveStateData>
        {
            new SaveStateData { Key = "UrnWood", Component = UrnComponent.Urn, LoadAction = SetSpriteWood},
            new SaveStateData { Key = "UrnMetal", Component = UrnComponent.Urn, LoadAction = SetSpriteMetal},
            new SaveStateData { Key = "UrnCeramic", Component = UrnComponent.Urn, LoadAction = SetSpriteCeramic},
            new SaveStateData { Key = "UrnJar", Component = UrnComponent.Urn, LoadAction = SetSpriteJar},
            new SaveStateData { Key = "DecorWhoops", Component = UrnComponent.Decor, LoadAction = SetWhoops},
            new SaveStateData { Key = "DecorCross", Component = UrnComponent.Decor, LoadAction = SetCross},
            new SaveStateData { Key = "DecorFlower", Component = UrnComponent.Decor, LoadAction = SetFlower},
            new SaveStateData { Key = "DecorStar", Component = UrnComponent.Decor, LoadAction = SetStar},
            new SaveStateData { Key = "DecorHeart", Component = UrnComponent.Decor, LoadAction = SetHeart},
            new SaveStateData { Key = "ColorNatural", Component = UrnComponent.Color, LoadAction = SetColorNatural},
            new SaveStateData { Key = "ColorMellow", Component = UrnComponent.Color, LoadAction = SetColorMellow},
            new SaveStateData { Key = "ColorPop", Component = UrnComponent.Color, LoadAction = SetColorPop},
            new SaveStateData { Key = "ColorCool", Component = UrnComponent.Color, LoadAction = SetColorCool}
        };
    }

    string GetSavedStateKey(UrnComponent component)
    {
        return UrnStateKey + component.ToString();
    }

    void Start()
    {
        LoadSavedUrnState();
        initialized = true;
    }

    void LoadSavedUrnState()
    {
        var urnValue = PlayerPrefs.GetString(GetSavedStateKey(UrnComponent.Urn), null);
        var decorValue = PlayerPrefs.GetString(GetSavedStateKey(UrnComponent.Decor), null);
        var colorValue = PlayerPrefs.GetString(GetSavedStateKey(UrnComponent.Color), null);

        if (!string.IsNullOrEmpty(urnValue))
        {
            var urnState = saveStateDataArray.Find(s => s.Key == urnValue);
            urnState?.LoadAction.Invoke();
        }

        if (!string.IsNullOrEmpty(decorValue))
        {
            var decorState = saveStateDataArray.Find(s => s.Key == decorValue);
            decorState?.LoadAction.Invoke();
        }

        if (!string.IsNullOrEmpty(colorValue))
        {
            var colorState = saveStateDataArray.Find(s => s.Key == colorValue);
            colorState?.LoadAction.Invoke();
        }
    }

    private void SetUrnSprite(Sprite sprite, AudioClip sfx)
    {
        if (sprite == null || UrnRenderer == null) return;
        UrnRenderer.sprite = sprite;

        if (initialized)
        {
            audioSource.clip = sfx;
            audioSource.Play();
        }
    }

    // Urn sprite changer
    public void SetSpriteMetal()
    {
        SetUrnSprite(metal, metalSFX);
        Save("UrnMetal");
        Debug.Log("Sprite is now Metal");
    }

    public void SetSpriteWood()
    {
        SetUrnSprite(wood, woodSFX);
        Save("UrnWood");
        Debug.Log("Sprite is now Wood");
    }

    public void SetSpriteCeramic()
    {
        SetUrnSprite(ceramic, ceramicSFX);
        Save("UrnCeramic");
        Debug.Log("Sprite is now Ceramic");
    }

    public void SetSpriteJar()
    {
        SetUrnSprite(jar, jarSFX);
        Save("UrnJar");
        Debug.Log("Sprite is now a Jar haha");
    }

    // Color Changer

    public void SetColorNatural()
    {
        SetColorFromHex("#AB8777");
        Save("ColorNatural");
    }

    public void SetColorMellow()
    {
        SetColorFromHex("#F1E363");
        Save("ColorMellow");
    }

    public void SetColorPop()
    {
        SetColorFromHex("#F94FFF");
        Save("ColorPop");
    }
    public void SetColorCool()
    {
        SetColorFromHex("#589DFF");
        Save("ColorCool");
    }


    // decor sprite changer
    private void SetDecorSprite(Sprite sprite, AudioClip sfx)
    {
        if (sprite == null || DecorRenderer == null) return;
        DecorRenderer.sprite = sprite;

        if (initialized)
        {
            audioSource.clip = sfx;
            audioSource.Play();
        }
    }

    public void SetWhoops()
    {
        SetDecorSprite(whoops, whoopsSFX);
        Save("DecorWhoops");
    }
    public void SetCross()
    {
        SetDecorSprite(cross, crossSFX);
        Save("DecorCross");
    }

    public void SetFlower()
    {
        SetDecorSprite(flower, flowerSFX);
        Save("DecorFlower");
    }
    public void SetStar()
    {
        SetDecorSprite(star, starSFX);
        Save("DecorStar");
    }
    public void SetHeart()
    {
        SetDecorSprite(heart, heartSFX);
        Save("DecorHeart");
    }

    public void SetColorFromHex(string hexCode)
    {
        if (UrnRenderer == null) return;

        Color color;
        // Wichtig: Hex-Code muss mit # beginnen
        if (!hexCode.StartsWith("#"))
            hexCode = "#" + hexCode;

        if (ColorUtility.TryParseHtmlString(hexCode, out color))
        {
            UrnRenderer.color = color;
        }
        else
        {
            Debug.LogWarning("Ungültiger Hex-Farbcode: " + hexCode);
        }

    }
    private void Save(string key)
    {
        var saveState = saveStateDataArray.Find(s => s.Key == key);
        if (saveState != null)
        {
            PlayerPrefs.SetString(GetSavedStateKey(saveState.Component), saveState.Key);
        }
        else
        {
            Debug.LogError("SaveStateData not found for key: " + key);
        }
        PlayerPrefs.Save();
    }
}