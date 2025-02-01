using UnityEngine;

public class DropArea : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Optional: Hier kannst du eine Nachricht oder Animation abspielen, wenn der Player den Bereich betritt
            Debug.Log("Ablagebereich erreicht!");
        }
    }
}
