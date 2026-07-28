using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Tooltip("Manueller Multiplikator für andere Effekte, z.B. Bild tragen.")]
    [Range(0.1f, 1f)]
    public float speedMultiplier = 1f;

    [Tooltip("Zusätzlicher Multiplikator für Wasser/Depression-Minigame.")]
    [Range(0.1f, 1f)]
    public float waterSpeedMultiplier = 1f;

    [Header("Graphics")]
    public Transform graphics;

    Rigidbody rb;
    float inputX;
    float inputZ;
    private bool movementEnabled = true;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (graphics != null)
            originalScale = graphics.localScale;
    }

    void FixedUpdate()
    {
        float finalSpeedMultiplier = speedMultiplier * waterSpeedMultiplier;
        Vector3 movement = new Vector3(inputX, 0f, inputZ).normalized * moveSpeed * finalSpeedMultiplier;

        if (movementEnabled)
            rb.linearVelocity = movement;
        else
            rb.linearVelocity = Vector3.zero;
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

        if (graphics != null && movementEnabled)
        {
            Vector3 scale = originalScale;
            if (inputX > 0.01f)
                scale.x = Mathf.Abs(originalScale.x);
            else if (inputX < -0.01f)
                scale.x = -Mathf.Abs(originalScale.x);

            graphics.localScale = scale;
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void SetWaterSpeedMultiplier(float multiplier)
    {
        waterSpeedMultiplier = Mathf.Clamp(multiplier, 0.1f, 1f);
    }

    public void ResetWaterSpeedMultiplier()
    {
        waterSpeedMultiplier = 1f;
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }

    public void DisableMovement()
    {
        movementEnabled = false;
    }

    public void ResetSpeedMultiplier()
    {
        speedMultiplier = 1f;
    }
}