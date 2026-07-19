using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Graphics")]
    public Transform graphics; // Parent von Spine-Objekt

    Rigidbody rb;

    float inputX;
    float inputZ;

    // Original scale speichern, damit wir nicht die globale Skalierung ändern
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (graphics != null)
            originalScale = graphics.localScale;
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(inputX, 0f, inputZ).normalized * moveSpeed;

        // Rigidbody bewegen
        rb.linearVelocity = movement;
    }
    void Update()
    {
        // Input: Horizontal = X, Vertical = Z
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

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


