using UnityEngine;

public class ThoughtWheelRotator : MonoBehaviour
{
    public float rotationSpeed = 20f;

    void Update()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // Uhrzeigersinn
    }
}
