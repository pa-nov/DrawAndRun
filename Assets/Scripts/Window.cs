using UnityEngine;

public class Window : MonoBehaviour
{
    private void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, Mathf.PingPong(Time.time * 0.7f, 1) * 50 - 25);
    }
}