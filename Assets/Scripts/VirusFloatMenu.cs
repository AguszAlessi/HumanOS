using UnityEngine;
using System.Collections;

public class VirusFloatMenu : MonoBehaviour
{
    public float rotationSpeed = 30f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, rotationSpeed * 0.5f * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeed * 0.3f * Time.deltaTime);
    }
}
