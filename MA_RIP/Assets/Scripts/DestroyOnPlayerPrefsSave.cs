using UnityEngine;

public class DestroyOnPlayerPrefsSave : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var key = "AlreadyRun" + gameObject.name;
        var isRun = (PlayerPrefs.GetInt(key, 0) == 1) ? true : false;
        if (isRun)
        {
            Destroy(gameObject);
        }
        else
        {
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
        }
    }
}
