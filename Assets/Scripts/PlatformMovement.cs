using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlatformMovement : MonoBehaviour
{
    [Tooltip("Units per second.")]
    public float speed = 5f;

    [Tooltip("Left/right bounds for X position.")]
    public float minX = -10f;
    public float maxX = 10f;

    Rigidbody rb;
    float inputX;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // Calculate movement delta
        Vector3 delta = Vector3.right * inputX * speed * Time.fixedDeltaTime;

        // Compute new unclamped position
        Vector3 newPos = rb.position + delta;

        // Clamp X
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        // Move there
        rb.MovePosition(newPos);
    }
}