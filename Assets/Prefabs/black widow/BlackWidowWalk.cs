using UnityEngine;

public class SpiderWalker : MonoBehaviour
{
    public float moveSpeed = 2.0f; 
    public float turnSpeed = 30.0f;   

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = 0;
        transform.eulerAngles = currentRotation;
    }
}
