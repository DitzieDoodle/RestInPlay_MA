using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public void OnPickup()
    {
        // Hier kannst du spezifische Logik f�r das Aufheben hinzuf�gen (z. B. Soundeffekte)
        Debug.Log(gameObject.name + " wurde aufgenommen!");
    }

    public void OnDrop()
    {
        // Hier kannst du spezifische Logik f�r das Ablegen hinzuf�gen (z. B. Soundeffekte)
        Debug.Log(gameObject.name + " wurde abgelegt!");
    }
}
