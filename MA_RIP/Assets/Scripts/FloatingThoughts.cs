using UnityEngine;

public class FloatingThoughts : MonoBehaviour
{
    public Vector2 areaSize = new Vector2(600, 400); // Fläche, in der Gedanken spawnen

    public void Arrange()
    {
        foreach (RectTransform child in transform)
        {
            float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float y = Random.Range(-areaSize.y / 2, areaSize.y / 2);
            child.anchoredPosition = new Vector2(x, y);

            // Text gerade halten
            var text = child.GetComponentInChildren<UnityEngine.UI.Text>();
            if (text != null) text.transform.rotation = Quaternion.identity;
        }
    }
}
