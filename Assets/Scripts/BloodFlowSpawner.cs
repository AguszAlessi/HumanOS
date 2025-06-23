using UnityEngine;
using System.Collections.Generic;

public class BloodFlowSpawner : MonoBehaviour
{
    public GameObject bloodCellPrefab;
    public int cantidad = 10;
    public float speedMin = 0.5f;
    public float speedMax = 2.0f;
    public float cylinderRadius = 1.5f;
    public float cylinderLength = 20f;
    public Vector3 flowDirection = Vector3.back;
    public float speed = 1.5f;
    public float despawnZ = -25f;

    private List<GameObject> bloodCells = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < cantidad; i++)
        {
            SpawnBloodCell();
        }
    }

    void SpawnBloodCell()
    {
        Vector2 circle = Random.insideUnitCircle * cylinderRadius;
        float z = Random.Range(-cylinderLength / 2f, cylinderLength / 2f);
        Vector3 spawnPosition = transform.position + new Vector3(circle.x, circle.y, z);

        GameObject cell = Instantiate(bloodCellPrefab, spawnPosition, Random.rotation);
        cell.transform.SetParent(transform);
        float velocidad = Random.Range(speedMin, speedMax);
        cell.AddComponent<BloodCellMover>().Initialize(flowDirection, velocidad, despawnZ, this);
        bloodCells.Add(cell);
    }

    public void Recycle(GameObject cell)
    {
        Vector2 circle = Random.insideUnitCircle * cylinderRadius;
        float z = cylinderLength / 2f;
        cell.transform.position = transform.position + new Vector3(circle.x, circle.y, z);
        float nuevaVelocidad = Random.Range(speedMin, speedMax);
        cell.GetComponent<BloodCellMover>().SetSpeed(nuevaVelocidad);
    }
}