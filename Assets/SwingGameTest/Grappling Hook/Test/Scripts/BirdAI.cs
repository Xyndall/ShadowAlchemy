using System.Collections;
using UnityEngine;

public class BirdAI : MonoBehaviour
{
    public float flySpeed = 3f;
    public float minRestTime = 2f;
    public float maxRestTime = 5f;
    public float minFlyTime = 2f;
    public float maxFlyTime = 5f;
    public Transform[] restPoints;
    public LayerMask obstacleLayer;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(FlyAndRestRoutine());
    }

    IEnumerator FlyAndRestRoutine()
    {
        while (true)
        {
            float flyDuration = Random.Range(minFlyTime, maxFlyTime);
            animator.SetBool("isFlying", true);
            

            float timer = 0f;
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            while (timer < flyDuration)
            {
                Vector3 potentialPosition = transform.position + randomDirection * flySpeed * Time.deltaTime;
                if (!Physics2D.Raycast(transform.position, randomDirection, flySpeed * Time.deltaTime, obstacleLayer))
                {
                    transform.position = potentialPosition;
                    if (randomDirection.x != 0)
                    {
                        transform.localScale = new Vector3(-Mathf.Sign(randomDirection.x), 1, 1);
                    }
                }
                else
                {
                    randomDirection = Random.insideUnitCircle.normalized;
                }
                timer += Time.deltaTime;
                yield return null;
            }

            // Fly smoothly to the exact rest point
            Transform restPoint = restPoints[Random.Range(0, restPoints.Length)];
            while (Vector3.Distance(transform.position, restPoint.position) > 0.01f)
            {
                Vector3 direction = (restPoint.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, restPoint.position, flySpeed * Time.deltaTime);
                if (direction.x != 0)
                {
                    transform.localScale = new Vector3(-Mathf.Sign(direction.x), 1, 1);
                }
                yield return null;
            }

            // Rest exactly at the rest point
            transform.position = restPoint.position;
            animator.SetBool("isFlying", false);
            
            yield return new WaitForSeconds(Random.Range(minRestTime, maxRestTime));
        }
    }
}