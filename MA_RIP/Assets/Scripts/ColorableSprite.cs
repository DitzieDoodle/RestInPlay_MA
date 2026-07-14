using UnityEngine;

public class ColorableSprite : MonoBehaviour
{
    public enum Index
    {
        None,
        Main,
        Secondary,
    }

    public Index ColorSlot = Index.Main;
}