using UnityEngine;
using System.IO;
using SFB;

public class ImageLoader : MonoBehaviour
{
    public Renderer targetRenderer; // Fläche für das Bild
    private const string ImageKey = "SavedUserImage"; //String zum speichern

    void Start()
    {
        LoadFromPrefs();
    }
    public void LoadImage()
    {
        // 🔹 RICHTIGER Filter
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "PNG", "JPG", "JPEG"),
            new ExtensionFilter("All Files", "*")
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Bild auswählen", "", extensions, false);

        if (paths.Length > 0 && File.Exists(paths[0]))
        {
            byte[] fileData = File.ReadAllBytes(paths[0]);

            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(fileData);

            ApplyTexture(tex);

            SaveToPrefs(tex);//Speichern
        }
    }

    void ApplyTexture(Texture2D tex)
    {
        Material mat = targetRenderer.material;
        mat.mainTexture = tex;

        float imageRatio = (float)tex.width / tex.height;   // Bildformat
        float planeRatio = targetRenderer.bounds.size.x / targetRenderer.bounds.size.y; // Flächenformat

        Vector2 tiling = Vector2.one;
        Vector2 offset = Vector2.zero;

        if (imageRatio > planeRatio)
        {
            // Bild ist breiter als die Fläche → oben/unten Rand
            tiling.y = planeRatio / imageRatio;
            offset.y = (1f - tiling.y) / 2f;
        }
        else
        {
            // Bild ist höher → links/rechts Rand
            tiling.x = imageRatio / planeRatio;
            offset.x = (1f - tiling.x) / 2f;
        }

        mat.mainTextureScale = tiling;
        mat.mainTextureOffset = offset;
    }

    void SaveToPrefs(Texture2D tex)
    {
        byte[] pngData = tex.EncodeToPNG();
        string base64 = System.Convert.ToBase64String(pngData);

        PlayerPrefs.SetString(ImageKey, base64);
        PlayerPrefs.Save();
    }

    // 📂 LADEN
    void LoadFromPrefs()
    {
        if (!PlayerPrefs.HasKey(ImageKey)) return;

        string base64 = PlayerPrefs.GetString(ImageKey);
        byte[] pngData = System.Convert.FromBase64String(base64);

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(pngData);

        ApplyTexture(tex);
    }
}
