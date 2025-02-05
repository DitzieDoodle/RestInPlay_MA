using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    public float pickupRange = 3f;  // Wie nah der Spieler am Objekt sein muss
    private GameObject pickedObject;
    private bool isHolding = false;

    public Vector3 holdOffset = new Vector3(0, 1, 2);  // Position des Objekts relativ zum Spieler (z. B. über der Hand)

    private Collider objectInRange = null;

    void Update()
    {
        // Wenn der Spieler E drückt und ein Objekt in Reichweite ist
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHolding && objectInRange != null)
            {
                PickupObject(objectInRange.gameObject);  // Objekt aufnehmen
            }
            else if (isHolding)
            {
                DropObject();  // Objekt ablegen
            }
        }

        // Wenn das Objekt gehalten wird, folge der Position des Spielers
        if (isHolding && pickedObject != null)
        {
            pickedObject.transform.position = transform.position + holdOffset;
        }
    }

    // Wenn der Spieler in den Triggerbereich des Pickup-Objekts tritt
    void OnTriggerEnter(Collider other)
    {
        // Überprüfe, ob der Spieler ein Pickup-Objekt betritt
        if (other.CompareTag("PickUp") && !isHolding)
        {
            objectInRange = other;
            Debug.Log("Ein Objekt ist im Bereich: " + other.gameObject.name);
        }
    }

    // Wenn der Spieler den Triggerbereich des Pickup-Objekts verlässt
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            objectInRange = null;
            Debug.Log("Das Objekt wurde aus dem Bereich entfernt.");
        }
    }

    void PickupObject(GameObject obj)
    {
        pickedObject = obj;
        isHolding = true;

        // Setze das Objekt als Kind des Spielers und halte es an der richtigen Position
        pickedObject.transform.SetParent(transform, true);
        pickedObject.GetComponent<Rigidbody>().isKinematic = true;

        // Call the OnPickup method from the PickupItem script
        pickedObject.GetComponent<PickUpItem>().OnPickup();
    }

    void DropObject()
    {
        if (pickedObject != null)
        {
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedObject.transform.SetParent(null, true);  // Objekt wird vom Spieler getrennt

            // Call the OnDrop method from the PickupItem script
            pickedObject.GetComponent<PickUpItem>().OnDrop();

            pickedObject = null;
            isHolding = false;
        }
    }
}
