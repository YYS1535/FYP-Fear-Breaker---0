using UnityEngine;

public class SnakeAttackSwitcher : MonoBehaviour
{
    public GameObject crawlingSnake;
    public GameObject attackSnake;

    public float moveSpeed = 2.0f;
    public float switchDistance = 1.0f;

    private Vector3 startPos;
    private bool hasSwitchedToAttack = false;

    void Start()
    {
        startPos = crawlingSnake.transform.position;

        crawlingSnake.SetActive(true);
        attackSnake.SetActive(false);
    }

    void Update()
    {
        if (!hasSwitchedToAttack)
        {
            crawlingSnake.transform.Translate(crawlingSnake.transform.forward * moveSpeed * Time.deltaTime, Space.World);

            float walked = Vector3.Distance(startPos, crawlingSnake.transform.position);
            if (walked >= switchDistance)
            {
                attackSnake.transform.position = crawlingSnake.transform.position;
                attackSnake.transform.rotation = crawlingSnake.transform.rotation;

                crawlingSnake.SetActive(false);
                attackSnake.SetActive(true);

                hasSwitchedToAttack = true;
            }
        }
    }
}
