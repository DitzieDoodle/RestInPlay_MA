using System.Collections.Generic;
using UnityEngine;

public class FlowerHolder : MonoBehaviour
{
    [SerializeField] ColorHandler.ColorType griefType;
    [SerializeField] List<Transform> flowers = new List<Transform>();

    ColorHandler colorHandler;


    void Start()
    {
        colorHandler = FindAnyObjectByType<ColorHandler>();
        if (colorHandler != null)
        {
            colorHandler.OnColorChangedEvent.AddListener(UpdateFlowerHolder);
            UpdateFlowerHolder();
        }

        UpdateFlowerHolder();
    }

    void UpdateFlowerHolder()
    {
        int levelValue = colorHandler.GetLevelValue(griefType);
        Debug.Log($"Updating FlowerHolder for {griefType} with level {levelValue}");

        for (int i = 0; i < flowers.Count; i++)
        {
            if (i < levelValue)
            {
                flowers[i].gameObject.SetActive(true);
            }
            else
            {
                flowers[i].gameObject.SetActive(false);
            }
        }
    }
}
