using UnityEngine;

public class LeaningCubeStackSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public int stackSize = 5;
    public float cubeSize = 1f;
    public float maxInitialTilt = 5f;
    public bool randomTiltDirection = true;
    public Vector3 fixedTiltAxis = Vector3.forward;

    void Start()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Assign a cubePrefab!");
            return;
        }

        // where the bottom cube should sit
        Collider pc = GetComponent<Collider>();
        float y0 = pc.bounds.max.y + cubeSize * 0.5f;

        // pick a random lean
        Vector3 axis = randomTiltDirection
            ? new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized
            : fixedTiltAxis.normalized;
        float angle = Random.Range(-maxInitialTilt, maxInitialTilt);
        Quaternion bottomRot = Quaternion.AngleAxis(angle, axis);

        // spawn bottom cube
        GameObject prev = Instantiate(cubePrefab, new Vector3(transform.position.x, y0, transform.position.z), bottomRot);
        prev.tag = "StackCube";

        // spawn the rest along its local up
        for (int i = 1; i < stackSize; i++)
        {
            Vector3 pos = prev.transform.position + prev.transform.up * cubeSize;
            GameObject next = Instantiate(cubePrefab, pos, prev.transform.rotation);
            next.tag = "StackCube";
            prev = next;
        }
    }
}