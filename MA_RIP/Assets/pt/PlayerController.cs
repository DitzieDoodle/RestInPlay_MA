using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f;  // Wie schnell der Player beschleunigt
    public float deceleration = 10f;  // Wie schnell der Player abbremst

    public AudioClip interactionSound;

    private Rigidbody rb;
    private AudioSource audioSource;
    private Vector3 moveDirection;
    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Eingabe erfassen (WASD)
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D oder Pfeiltasten links/rechts
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S oder Pfeiltasten oben/unten

        // Bewege dich RELATIV zur Boden-Plane
        Vector3 forward = Vector3.Cross(transform.right, Vector3.up).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

        Vector3 targetDirection = (forward * moveZ + right * moveX).normalized;

        // Sanftes Beschleunigen & Abbremsen
        if (targetDirection.magnitude > 0)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetDirection * moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // Interaktion mit "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void FixedUpdate()
    {
        // Bewegung mit Rigidbody anwenden
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    void Interact()
    {
        if (interactionSound != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
        Debug.Log("Interaktion ausgelöst!");
    }
}
