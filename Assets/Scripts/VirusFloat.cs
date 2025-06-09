using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;


public class VirusFloat : MonoBehaviour
{
    public float minSpeed = 0.5f;
    public float maxSpeed = 3f;
    public float rotationSpeed = 30f;
    public float changeDirectionTime = 2f;

    public float boxWidth = 50f;
    public float boxHeight = 30f;
    public float boxDepth = 40f;
    public Vector3 centerPosition = Vector3.zero;

    private Vector3 direction;
    private float currentSpeed;
    private float timer;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.5f));
        PickNewDirectionAndSpeed();
    }

    void Update()
    {
        transform.position += direction * currentSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, rotationSpeed * 0.5f * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeed * 0.3f * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            PickNewDirectionAndSpeed();
            timer = 0f;
        }

        CheckCollisionAndBounce();
    }

    void PickNewDirectionAndSpeed()
    {
        Vector3 randomDir = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-0.5f, 0.5f),
            UnityEngine.Random.Range(-1f, 1f)
        );

        direction = randomDir.sqrMagnitude > 0.01f ? randomDir.normalized : Vector3.forward;
        currentSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
    }

    void CheckCollisionAndBounce()
    {
        Vector3 localPos = transform.position - centerPosition;

        float halfWidth = boxWidth / 2f;
        float halfHeight = boxHeight / 2f;
        float halfDepth = boxDepth / 2f;

        bool bounced = false;

        if (localPos.x < -halfWidth || localPos.x > halfWidth)
        {
            direction.x *= -1;
            bounced = true;
        }

        if (localPos.y < -halfHeight || localPos.y > halfHeight)
        {
            direction.y *= -1;
            bounced = true;
        }

        if (localPos.z < -halfDepth || localPos.z > halfDepth)
        {
            direction.z *= -1;
            bounced = true;
        }

        if (bounced)
        {
            direction.Normalize();
            currentSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        }
    }
}
