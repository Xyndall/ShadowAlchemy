using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMove : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Transform target;

    void Start()
    {
        target = pointB;
    }

    void Update()
    {
        MoveTowardsTarget();
        if(target == pointB)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(target == pointA)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void MoveTowardsTarget()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                target = target == pointA ? pointB : pointA;
            }
        }
    }
}
