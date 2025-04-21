using UnityEngine;

public class BlackWidowSwitcher : MonoBehaviour
{
    public GameObject blackwidowStatic;
    public GameObject blackwidowAttack;

    public float switchToAttackDelay = 2f;
    public float switchToStaticDelay = 3f;

    private float stateTimer = 0f;
    private float currentDelay;
    private Animator animator;

    void Start()
    {
        blackwidowStatic.SetActive(true);
        blackwidowAttack.SetActive(false);

        animator = blackwidowAttack.GetComponent<Animator>();
        currentDelay = switchToAttackDelay;
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= currentDelay)
        {
            // Toggle between static and attack
            bool goingToAttack = blackwidowStatic.activeSelf;

            blackwidowStatic.SetActive(!goingToAttack);
            blackwidowAttack.SetActive(goingToAttack);

            if (goingToAttack && animator != null)
            {
                animator.Play("blackwidowAttack", 0, 0f);
                currentDelay = switchToStaticDelay;
            }
            else
            {
                currentDelay = switchToAttackDelay;
            }

            stateTimer = 0f;
        }
    }
}
