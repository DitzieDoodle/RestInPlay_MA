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
            carry.currentDepositZone = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerFlowerCarry carry = other.GetComponent<PlayerFlowerCarry>();
        if (carry != null && carry == playerInZone)
        {
            playerInZone = null;
            carry.currentDepositZone = null;
        }
    }
}