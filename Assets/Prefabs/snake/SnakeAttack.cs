using UnityEngine;

public class SnakeAttackSwitcher : MonoBehaviour
{
    public GameObject crawlingSnake;
    public GameObject attackSnake;

    public float moveSpeed = 2.0f;
    public float switchDistance = 1.0f;
    public float resetDelay = 2.0f;

    private Vector3 startPos;
    private bool isSwitching = false;
    private float resetTimer = 0f;

    void Start()
    {
        startPos = crawlingSnake.transform.position;

        crawlingSnake.SetActive(true);
        attackSnake.SetActive(false);
    }

    void Update()
    {
        if (!isSwitching)
        {
            crawlingSnake.transform.Translate(crawlingSnake.transform.forward * moveSpeed * Time.deltaTime, Space.World);

            float walked = Vector3.Distance(startPos, crawlingSnake.transform.position);
            if (walked >= switchDistance)
            {
                attackSnake.transform.position = crawlingSnake.transform.position;
                attackSnake.transform.rotation = crawlingSnake.transform.rotation;

                crawlingSnake.SetActive(false);
                attackSnake.SetActive(true);

                isSwitching = true;
                resetTimer = resetDelay;
            }
        }
        else
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0f)
            {
                ResetSnake();
            }
        }
    }

    void ResetSnake()
    {
        crawlingSnake.transform.position = startPos;
        crawlingSnake.SetActive(true);
        attackSnake.SetActive(false);

        isSwitching = false;
    }
}
