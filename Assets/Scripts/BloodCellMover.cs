using UnityEngine;

public class BloodCellMover : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    public float despawnZ = -25f; 
    private BloodFlowSpawner spawner;

    public void Initialize(Vector3 dir, float spd, float despawn, BloodFlowSpawner refSpawner)
    {
        direction = dir.normalized;
        speed = spd;
        despawnZ = despawn;
        spawner = refSpawner;
    }
    public void SetSpeed(float spd)
    {
        speed = spd;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (transform.localPosition.z < despawnZ)
        {
            spawner.Recycle(gameObject);
        }
    }
}