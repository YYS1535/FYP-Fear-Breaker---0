using UnityEngine;

public class SnakeSwitcher : MonoBehaviour
{
    public GameObject crawlingSnake;
    public GameObject transitionSnake;
    public GameObject idleSnake;

    public float moveSpeed = 2.0f;
    public float switchDistance = 1.0f;
    public float transitionDelay = 2.0f;

    private Vector3 startPos;
    private Vector3 moveDirection;
    private bool hasSwitchedToTransition = false;
    private bool hasSwitchedToIdle = false;
    private float transitionStartTime;

    void Start()
    {
        startPos = transform.position;
        moveDirection = transform.forward; 

        crawlingSnake.SetActive(true);
        transitionSnake.SetActive(false);
        idleSnake.SetActive(false);
    }

    void Update()
    {
        if (!hasSwitchedToTransition)
        {
            // Move consistently in the saved direction
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            float walked = Vector3.Distance(startPos, transform.position);
            if (walked >= switchDistance)
            {
                crawlingSnake.SetActive(false);
                transitionSnake.SetActive(true);

                hasSwitchedToTransition = true;
                transitionStartTime = Time.time;
            }
        }

        if (hasSwitchedToTransition && !hasSwitchedToIdle)
        {
            if (Time.time - transitionStartTime >= transitionDelay)
            {
                transitionSnake.SetActive(false);
                idleSnake.SetActive(true);

                hasSwitchedToIdle = true;
            }
        }
    }
}
