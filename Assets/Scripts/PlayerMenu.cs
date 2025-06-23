using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Fijar posición actual y desactivar gravedad y movimiento
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Update()
    {
        // No se hace nada en Update: el jugador queda totalmente quieto
    }
}
