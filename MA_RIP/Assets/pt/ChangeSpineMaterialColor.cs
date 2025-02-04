using UnityEngine;
using Spine.Unity;

public class ChangeSpineMaterialColor : MonoBehaviour
{
    [SerializeField] private SkeletonRenderer skeletonRenderer;  // Referenz zum SkeletonRenderer
    [SerializeField] private Color targetColor = Color.red;     // Zielfarbe, die auf das Material angewendet werden soll

    void Start()
    {
        // �berpr�fen, ob der SkeletonRenderer gesetzt wurde
        if (skeletonRenderer == null)
        {
            Debug.LogError("SkeletonRenderer reference is missing!");
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
}
