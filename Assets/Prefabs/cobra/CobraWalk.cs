using UnityEngine;

public class CobraSwitcher : MonoBehaviour
{
    public GameObject crawlingCobra;
    public GameObject transitionCobra;
    public GameObject idleCobra;

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

        crawlingCobra.SetActive(true);
        transitionCobra.SetActive(false);
        idleCobra.SetActive(false);
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
                crawlingCobra.SetActive(false);
                transitionCobra.SetActive(true);

                hasSwitchedToTransition = true;
                transitionStartTime = Time.time;
            }
        }

        if (hasSwitchedToTransition && !hasSwitchedToIdle)
        {
            if (Time.time - transitionStartTime >= transitionDelay)
            {
                transitionCobra.SetActive(false);
                idleCobra.SetActive(true);

                hasSwitchedToIdle = true;
            }
        }
    }
}
