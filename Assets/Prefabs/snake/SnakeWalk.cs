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
    private bool hasSwitchedToTransition = false;
    private bool hasSwitchedToIdle = false;
    private float transitionStartTime;

    void Start()
    {
        startPos = crawlingSnake.transform.position;

        crawlingSnake.SetActive(true);
        transitionSnake.SetActive(false);
        idleSnake.SetActive(false);
    }

    void Update()
    {
        if (!hasSwitchedToTransition)
        {
            crawlingSnake.transform.Translate(crawlingSnake.transform.forward * moveSpeed * Time.deltaTime, Space.World);

            float walked = Vector3.Distance(startPos, crawlingSnake.transform.position);
            if (walked >= switchDistance)
            {
                // Switch to transition animation
                transitionSnake.transform.position = crawlingSnake.transform.position;
                transitionSnake.transform.rotation = crawlingSnake.transform.rotation;

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
                idleSnake.transform.position = transitionSnake.transform.position;
                idleSnake.transform.rotation = transitionSnake.transform.rotation;

                transitionSnake.SetActive(false);
                idleSnake.SetActive(true);

                hasSwitchedToIdle = true;
            }
        }
    }
}
