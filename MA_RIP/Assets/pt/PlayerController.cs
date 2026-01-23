using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Graphics")]
    public Transform graphics; // Parent von Spine-Objekt

    private float currentVelocity = 0f;
    private float inputX;
    private Vector3 velocity = Vector3.zero; // Für SmoothDamp

    // Original scale speichern, damit wir nicht die globale Skalierung ändern
    private Vector3 originalScale;

    void Start()
    {
        if (graphics != null)
            originalScale = graphics.localScale;
    }
    void Update()
    {
        // Input: Horizontal = X, Vertical = Z
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        // Bewegung auf der XZ-Ebene
        Vector3 movement = new Vector3(inputX, 0f, inputZ).normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Flip Spine Grafik nur bei X-Bewegung
        if (graphics != null)
        {
            Vector3 scale = originalScale;
            if (inputX > 0.01f)
                scale.x = Mathf.Abs(originalScale.x); // nach rechts
            else if (inputX < -0.01f)
                scale.x = -Mathf.Abs(originalScale.x); // nach links
            graphics.localScale = scale;
        }
    }
}


