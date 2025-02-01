using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public void OnPickup()
    {
        // Hier kannst du spezifische Logik für das Aufheben hinzufügen (z. B. Soundeffekte)
        Debug.Log(gameObject.name + " wurde aufgenommen!");
    }

    public void OnDrop()
    {
        // Hier kannst du spezifische Logik für das Ablegen hinzufügen (z. B. Soundeffekte)
        Debug.Log(gameObject.name + " wurde abgelegt!");
    }
}
