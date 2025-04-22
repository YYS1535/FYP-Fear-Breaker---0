using UnityEngine;

public class SpiderWalker : MonoBehaviour
{
    public float moveSpeed = 2.0f;    // Forward speed
    public float turnSpeed = 30.0f;   // Degrees per second

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Turn slowly to create a circular path
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = 0;
        transform.eulerAngles = currentRotation;
    }
}
