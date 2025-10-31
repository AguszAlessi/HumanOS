using UnityEngine;

public class SyringePhysics : MonoBehaviour
{
    public Rigidbody rb;
    public float launchForce = 20f; // Fuerza inicial del lanzamiento
    public float angle = 45f; // Ángulo de lanzamiento en grados
    public Transform target; // Opcional: objetivo al que apuntar

    private bool hasLaunched = false;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        // Asegura que la gravedad esté activada
        if (rb != null)
            rb.useGravity = true;
    }

    // Llama a esta función para lanzar la jeringa
    public void Launch(Vector3 direction, float distance)
    {
        if (rb == null || hasLaunched) return;
        hasLaunched = true;

        // Calcula la fuerza necesaria para llegar a la distancia deseada
        float rad = angle * Mathf.Deg2Rad;
        float g = Physics.gravity.y;
        float v = Mathf.Sqrt(Mathf.Abs(distance * -g / Mathf.Sin(2 * rad)));

        // Calcula la dirección de lanzamiento con el ángulo
        Vector3 launchDir = Quaternion.AngleAxis(-angle, Vector3.right) * direction.normalized;
        rb.AddForce(launchDir * v, ForceMode.VelocityChange);
    }

    // Lanza la jeringa en la dirección indicada, con física realista
    public void LaunchForward()
    {
        if (rb == null || hasLaunched) return;
        hasLaunched = true;

        // Dirección de lanzamiento: adelante local
        Vector3 direction = transform.forward;
        float rad = angle * Mathf.Deg2Rad;
        float g = Mathf.Abs(Physics.gravity.y);
        float v = launchForce; // Puedes ajustar launchForce en el inspector

        // Aplica el ángulo de lanzamiento
        Vector3 launchDir = Quaternion.AngleAxis(-angle, transform.right) * direction;
        rb.AddForce(launchDir * v, ForceMode.VelocityChange);
    }

    // Ejemplo: lanzar hacia un objetivo

}
