// ThoughtWheel.cs
using UnityEngine;

public class CircularLayout : MonoBehaviour
{
    public float radius = 250f;        // Kreisradius
    public float rotationSpeed = 20f;  // Drehgeschwindigkeit

    void LateUpdate()
    {
        int count = transform.childCount;
        if (count == 0) return;

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            RectTransform child = transform.GetChild(i) as RectTransform;
            float angle = i * angleStep * Mathf.Deg2Rad;
            child.anchoredPosition = new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );

            // Gegenrotation, damit Text gerade bleibt
            child.rotation = Quaternion.identity;
        }

        // Parent drehen
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
