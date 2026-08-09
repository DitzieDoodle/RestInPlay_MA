using UnityEngine;
using Spine.Unity;

public class ChangeSpineMaterialColor : MonoBehaviour
{
    [SerializeField] private SkeletonRenderer skeletonRenderer;  // Referenz zum SkeletonRenderer
    [SerializeField] private SkeletonGraphic skeletonGraphic;  // Referenz zum SkeletonGraphic
    [SerializeField] private Color targetColor = Color.red;     // Zielfarbe, die auf das Material angewendet werden soll

    void Start()
    {
        SetSkeletonRendererColor();
        // SetSkeletonGraphicColor();
    }

    void SetSkeletonRendererColor()
    {
        // �berpr�fen, ob der SkeletonRenderer gesetzt wurde
        if (skeletonRenderer == null)
        {
            return;
        }

        // Hole das Material des SkeletonRenderers
        Material material = skeletonRenderer.GetComponent<Renderer>().material;

        // �berpr�fen, ob das Material existiert
        if (material != null)
        {
            // Setze die Farbe des Materials (in vielen Spine-Shadern ist der Parameter "_Color" f�r die Hauptfarbe zust�ndig)
            material.SetColor("_Color", targetColor);  // "_Color" oder "Color" je nach Shader
        }
        else
        {
            Debug.LogError("Material not found!");
        }
    }

    void SetSkeletonGraphicColor()
    {
        // �berpr�fen, ob der SkeletonGraphic gesetzt wurde
        if (skeletonGraphic == null)
        {
            return;
        }

        skeletonGraphic.color = targetColor;  // Setze die Farbe direkt auf das SkeletonGraphic
    }
}
