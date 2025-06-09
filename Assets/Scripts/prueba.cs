using UnityEngine;

public class VirusTestMove : MonoBehaviour
{
    public Vector3 direction = new Vector3(1, 0, 0);
    public float speed = 1f;

    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }
}