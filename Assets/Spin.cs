using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
