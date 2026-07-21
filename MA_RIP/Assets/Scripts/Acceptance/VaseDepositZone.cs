using UnityEngine;

public class VaseDepositZone : MonoBehaviour
{
    public VaseBouquet vase;

    private PlayerFlowerCarry playerInZone;

    private void OnTriggerEnter(Collider other)
    {
        PlayerFlowerCarry carry = other.GetComponent<PlayerFlowerCarry>();
        if (carry != null)
        {
            playerInZone = carry;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerFlowerCarry carry = other.GetComponent<PlayerFlowerCarry>();
        if (carry != null && carry == playerInZone)
        {
            playerInZone = null;
        }
    }

    private void Update()
    {
        if (playerInZone == null) return;
        if (playerInZone.carriedFlower == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerInZone.DropCurrentFlowerToVase(vase);
        }
    }
}