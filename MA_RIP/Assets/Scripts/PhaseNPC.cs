using UnityEngine;

public class PhaseNPC : MonoBehaviour
{
    [SerializeField] private string phaseName = "Anger";
    public AudioSource flower;

    public void GiveFlowers(int amount)
    {
        if (GriefProgressManager.Instance == null) return;

        GriefProgressManager.Instance.SetFlowers(phaseName, amount);
        Debug.Log(phaseName + " set to " + amount + " flowers");


    }

    //Testmethode die ich im dialog "AngryTest" aufrufe, um zu check ob das DS sie findet
    public void Testing()
    {
        Debug.Log("flowerGiven");
        flower.Play();
    }



    public string GetPhaseName()
    {
        return phaseName;
    }
}