using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{
    [SerializeField] public int value = 10;

    private Vector3 startPosition;

    public float speed;
    public float radius;
    [Range(0, 360)]
    public float angle;

    public float jumpForce = 100f;
    public float groundCheckRadius = 0.15f;

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask whatIsGround;

    public GameObject player;

    private Rigidbody2D rBody;
    private Animator anim;
    public bool isGrounded = false;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private Vector3 lastPosition;
    public bool isFacingLeft = true;
    private float countTime = 0f;
    private float waitTime = 100f;
    private float direction = -1f;

    void wait(float amountOfSeconds)
    {
        Invoke("CallMeWithWait", amountOfSeconds);
    }

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
        lastPosition = startPosition;
    }

    private bool GroundCheck()
    {
        if (isGrounded == false && Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, whatIsGround) == true)
        {
            rBody.AddForce(new Vector2(-(direction * (jumpForce/3.5f)), -jumpForce));

            //if (direction * jumpForce < 0)
            //{
            //    rBody.AddForce(new Vector2(direction * jumpForce, -jumpForce));
            //}
            //else
            //{
            //    rBody.AddForce(new Vector2(-(direction * jumpForce), -jumpForce));
            //}
        }
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, whatIsGround);
    }

    private void FixedUpdate()
    {
        countTime += 1f;
        anim.SetFloat("Velocity", rBody.velocity.y);

        isGrounded = GroundCheck();
        anim.SetBool("isGrounded", isGrounded);
        if (!isGrounded)
        {
            countTime = 0;
        }
        CheckFieldOfView();
        if (canSeePlayer && isGrounded && countTime > waitTime)
        {
            ChasePlayer();
        }
        else if (!canSeePlayer && isGrounded && countTime > waitTime)
        {
            ReturnHome();

        }
    }

    private void CheckFieldOfView()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.position, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
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
        direction *= -1f;
    }

    private void ReturnHome()
    {
        if (startPosition.x > transform.position.x && isFacingLeft == true)
        {
            //Frog needs to face right
            Flip();
        }
        else if (startPosition.x < transform.position.x && isFacingLeft == false)
        {
            //Frog needs to face left
            Flip();
        }

        Jump();
    }

    private void ChasePlayer()
    {
        if (player.transform.position.x > transform.position.x && isFacingLeft == true)
        {
            //Frog needs to face right
            Flip();
        }
        else if (player.transform.position.x < transform.position.x && isFacingLeft == false)
        {
            //Frog needs to face left
            Flip();
        }

        Jump();
    }

    private void Jump()
    {
        rBody.AddForce(new Vector2(direction * (jumpForce / 3.5f), jumpForce));
        isGrounded = false;
        //anim.SetTrigger("FrogJump");
    }
}
