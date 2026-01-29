using UnityEngine;

public class CounterRotate : MonoBehaviour
{
    void Update()
    {
        float parentZ = transform.parent.eulerAngles.z;
        transform.rotation = Quaternion.Euler(0, 0, parentZ);
    }
}
