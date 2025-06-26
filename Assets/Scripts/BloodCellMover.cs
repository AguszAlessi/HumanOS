using UnityEngine;

public class BloodCellMover : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    public float despawnZ = -25f; 
    private BloodFlowSpawner spawner;

    [Header("Rotación")]
    public float rotationSpeed = 180f; // grados por segundo
    private Vector3 rotationAxis;

    public void Initialize(Vector3 dir, float spd, float despawn, BloodFlowSpawner refSpawner)
    {
        direction = dir.normalized;
        speed = spd;
        despawnZ = despawn;
        spawner = refSpawner;

        // Eje de rotación aleatorio para cada glóbulo rojo (opcional)
        rotationAxis = Random.onUnitSphere;
    }

    public void SetSpeed(float spd)
    {
        speed = spd;
    }

    void Update()
    {
        // Movimiento lineal
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Rotación continua
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);

        // Reaparecer si pasa el límite
        if (transform.localPosition.z < despawnZ)
        {
            spawner.Recycle(gameObject);
        }
    }
}
    