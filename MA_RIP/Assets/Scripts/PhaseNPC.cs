using UnityEngine;

public class PhaseNPC : MonoBehaviour
{
    [SerializeField] private string phaseName = "Anger";

    public void GiveFlowers(int amount)
    {
        if (GriefProgressManager.Instance == null) return;

        GriefProgressManager.Instance.SetFlowers(phaseName, amount);
        Debug.Log(phaseName + " set to " + amount + " flowers");
    }

    public string GetPhaseName()
    {
        return phaseName;
    }
}