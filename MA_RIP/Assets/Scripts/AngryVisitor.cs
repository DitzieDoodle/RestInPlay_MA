using UnityEngine;

public class AngryVisitor : MonoBehaviour
{
    public AudioSource shuffle;
    [Header("Sprites")]
    public SpriteRenderer[] sprites;
    [Range(0, 10)] public int currentIndex = 0; // Inspector-Feld



    void ShufflingPhotos()
    {
        shuffle.Play();
    }
    public void ShowOnly(int index)
    {
        if (sprites == null || sprites.Length == 0) return;
        if (index < 0 || index >= sprites.Length) return;

        currentIndex = index;

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] != null)
                sprites[i].enabled = (i == index);
        }
    }

    /// <summary>
    /// Schaltet zum nächsten Sprite im Array
    /// </summary>
    public void Next()
    {
        if (sprites == null || sprites.Length == 0) return;
        int nextIndex = (currentIndex + 1) % sprites.Length;
        ShowOnly(nextIndex);
    }

    /// <summary>
    /// Schaltet zum vorherigen Sprite im Array
    /// </summary>
    public void Previous()
    {
        if (sprites == null || sprites.Length == 0) return;
        int prevIndex = (currentIndex - 1 + sprites.Length) % sprites.Length;
        ShowOnly(prevIndex);
    }

}
