// using System.Collections.Generic;
// using UnityEngine;

// public class ColorHandler : MonoBehaviour
// {
//     public enum ColorType
//     {
//         Acceptance,
//         Anger,
//         Denial,
//         Depression,
//         Bargaining,
//     }


//     public List<(ColorType colorType, ColorableSprite.Index colorSlot, Color color)> colorMappings = new List<(ColorType, ColorableSprite.Index, Color)>
//     {
//         (ColorType.Acceptance, ColorableSprite.Index.Main, Color.green),
//         (ColorType.Anger, ColorableSprite.Index.Main, Color.red),
//         (ColorType.Denial, ColorableSprite.Index.Main, Color.yellow),
//         (ColorType.Depression, ColorableSprite.Index.Main, Color.blue),
//         (ColorType.Bargaining, ColorableSprite.Index.Main, Color.magenta),
//         (ColorType.Acceptance, ColorableSprite.Index.Secondary, Color.green),
//         (ColorType.Anger, ColorableSprite.Index.Secondary, Color.red),
//         (ColorType.Denial, ColorableSprite.Index.Secondary, Color.yellow),
//         (ColorType.Depression, ColorableSprite.Index.Secondary, Color.blue),
//         (ColorType.Bargaining, ColorableSprite.Index.Secondary, Color.magenta),
//     };

//     Dictionary<ColorType, ColorableSprite.Index> colorLevels = new Dictionary<ColorType, ColorableSprite.Index>();
//     Dictionary<(ColorType, ColorableSprite.Index), Color> colorMappingsDict = new Dictionary<(ColorType, ColorableSprite.Index), Color>();

//     void Start()
//     {
//         colorLevels[ColorType.Acceptance] = ColorableSprite.Index.None;
//         colorLevels[ColorType.Anger] = ColorableSprite.Index.None;
//         colorLevels[ColorType.Denial] = ColorableSprite.Index.None;
//         colorLevels[ColorType.Depression] = ColorableSprite.Index.None;
//         colorLevels[ColorType.Bargaining] = ColorableSprite.Index.None;

//         colorMappingsDict.Clear();
//         foreach (var mapping in colorMappings)
//         {
//             colorMappingsDict[(mapping.colorType, mapping.colorSlot)] = mapping.color;
//         }
//     }

//     public void AddLevel(ColorType colorType)
//     {
//         if (colorLevels.ContainsKey(colorType))
//         {
//             ColorableSprite.Index currentLevel = colorLevels[colorType];
//             ColorableSprite.Index nextLevel = GetNextLevel(currentLevel);
//             colorLevels[colorType] = nextLevel;
//         }

//         UpdateColorableSprites();
//     }

//     private void UpdateColorableSprites()
//     {
//         ColorableSprite[] colorableSprites = FindObjectsByType<ColorableSprite>(FindObjectsInactive.Include, FindObjectsSortMode.None);

//         Dictionary<ColorType, List<ColorableSprite>> spritesByColorType = new Dictionary<ColorType, List<ColorableSprite>>();


//         foreach (ColorableSprite sprite in colorableSprites)
//         {
//             if (colorMappingsDict.TryGetValue((sprite.colorType, sprite.colorSlot), out Color color))
//             {
//                 sprite.GetComponent<SpriteRenderer>().color = color;
//             }
//             else
//             {
//                 sprite.GetComponent<SpriteRenderer>().color = Color.white; // Default-Farbe, wenn keine Zuordnung gefunden wurde
//             }
//         }
//     }

//     private ColorableSprite.Index GetNextLevel(ColorableSprite.Index currentLevel)
//     {
//         switch (currentLevel)
//         {
//             case ColorableSprite.Index.None:
//                 return ColorableSprite.Index.Main;
//             case ColorableSprite.Index.Main:
//                 return ColorableSprite.Index.Secondary;
//             case ColorableSprite.Index.Secondary:
//                 return ColorableSprite.Index.Secondary;
//             default:
//                 return ColorableSprite.Index.None;
//         }
//     }
// }