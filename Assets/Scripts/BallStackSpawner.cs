using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStackSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public int stackSize = 5;
    public float ballDiameter = 1f;

    void Start()
    {
        StartCoroutine(SpawnStack());
    }

    IEnumerator SpawnStack()
    {
        if (ballPrefab == null) yield break;
        Collider platCol = GetComponent<Collider>();
        float yStart = platCol.bounds.max.y + ballDiameter * 0.5f;

        var rigidbodies = new List<Rigidbody>();

        // 1) Spawn as kinematic so they don’t collide instantly
        for (int i = 0; i < stackSize; i++)
        {
            Vector3 pos = transform.position + Vector3.up * (yStart + i * ballDiameter);
            var ball = Instantiate(ballPrefab, pos, Quaternion.identity);
            var rb = ball.GetComponent<Rigidbody>();
            if (rb == null) rb = ball.AddComponent<Rigidbody>();

            rb.isKinematic = true;
            rigidbodies.Add(rb);
        }

        // 2) Wait one physics frame so all transforms settle cleanly
        yield return new WaitForFixedUpdate();

        // 3) Turn gravity/physics back on
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
    }
}
