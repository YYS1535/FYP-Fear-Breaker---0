using UnityEngine;

public class TarantulaWalker : MonoBehaviour
{
    public float moveSpeed = 2.0f;    // Forward speed
    public float turnSpeed = 30.0f;   // Degrees per second

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Turn slowly (Y axis)
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        // Lock Z rotation to 0 (preserve only X and Y rotation)
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = 0;
        transform.eulerAngles = currentRotation;
    }
}
