using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleScript : MonoBehaviour
{
    private Vector3 startPosition;

    public float speed;
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject player;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private Vector3 lastPosition;
    public bool isFacingLeft = true;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
        lastPosition = startPosition;
    }

    private void Update()
    {
        CheckFieldOfView();
        if (canSeePlayer)
        {
            ChasePlayer();
        }
        else
        {
            ReturnHome();
        }
    }

    private void CheckFieldOfView()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.position, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void Flip()
    {
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;
        isFacingLeft = !isFacingLeft;
    }

    private void ReturnHome()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * speed);

        if (!isFacingLeft && lastPosition.x - transform.position.x < 0 || isFacingLeft && lastPosition.x - transform.position.x > 0)
        {
            Flip();
        }
        lastPosition = transform.position;
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);

        if (!isFacingLeft && lastPosition.x - transform.position.x < 0 || isFacingLeft && lastPosition.x - transform.position.x > 0)
        {
            Flip();
        }
        lastPosition = transform.position;
    }
}
