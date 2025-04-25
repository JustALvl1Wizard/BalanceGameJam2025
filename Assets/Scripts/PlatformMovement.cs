using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlatformMovement : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody rb;
    float inputX;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Update() => inputX = Input.GetAxis("Horizontal");

    void FixedUpdate()
    {
        Vector3 delta = Vector3.right * inputX * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + delta);
    }
}
