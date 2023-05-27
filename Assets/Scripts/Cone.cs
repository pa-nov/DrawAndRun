using UnityEngine;

public class Cone : MonoBehaviour
{
    private void FixedUpdate()
    {
        float PosY = Mathf.Clamp01(Mathf.PingPong(Time.time * 0.3f, 1) * 2 - 0.5f) - 1;
        this.transform.localPosition = new(this.transform.localPosition.x, PosY * 2, this.transform.localPosition.z);
    }
}