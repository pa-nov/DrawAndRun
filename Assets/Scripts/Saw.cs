using UnityEngine;

public class Saw : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(new(0, 0, 180 * Time.deltaTime));
    }
}