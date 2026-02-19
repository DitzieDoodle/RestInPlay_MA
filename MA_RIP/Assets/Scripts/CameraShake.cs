using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AudioSource shuffle;
    [Header("Sprites")]
    public SpriteRenderer[] sprites;


    void ShufflingPhotos()
    {
        shuffle.Play(); 
    }
    public void ShowOnly(int index)
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("SpriteSwitcher: No sprites assigned!");
            return;
        }

        if (index < 0 || index >= sprites.Length)
        {
            Debug.LogWarning($"SpriteSwitcher: Index {index} out of range!");
            return;
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] != null)
                sprites[i].enabled = (i == index);
        }
    }

}
