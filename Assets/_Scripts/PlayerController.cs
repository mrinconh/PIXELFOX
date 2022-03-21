using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float jumpForce = 500.0f;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject CanvasFail;
    [SerializeField] private GameObject CanvasWin;
    //[SerializeField] private GameObject CanvasIngame;

    // private variables
    private Rigidbody2D rBody;
    private Animator anim;
    private bool isGrounded = false;
    //private bool isClimbing = false;
    private bool isFacingRight = true;
    //public AudioSource death_sound;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            //death_sound.Play();
            anim.SetTrigger("die");
            rBody.AddForce(new Vector2(0f, jumpForce));
            rBody.GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<PlayerController>().enabled = false;


            StartCoroutine(SetCanvas());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something Triggered");

        if (collision.gameObject.tag == "ladder")
        {
            rBody.GetComponent<Rigidbody2D>().gravityScale = -2f;
        }

        if (collision.gameObject.tag == "ground")
        {
            rBody.GetComponent<Rigidbody2D>().gravityScale = 2f;
        }

        if (collision.gameObject.tag == "finish")
        {
            CanvasWin.SetActive(true);
        }

    }

    IEnumerator SetCanvas()
    {
        yield return new WaitForSeconds(2);

        CanvasFail.SetActive(true);
        
        //CanvasIngame.SetActive(false);        
    }

    private void FixedUpdate()
    {
        isGrounded = GroundCheck();
        Walk();
        Jump();

    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, whatIsGround);

    }

    private void Flip()
    {
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;
        isFacingRight = !isFacingRight;
    }

    public void Walk()
    {
        float horiz = Input.GetAxis("Horizontal");
        rBody.velocity = new Vector2(horiz * speed, rBody.velocity.y);
        

        if (isFacingRight && rBody.velocity.x < 0 || !isFacingRight && rBody.velocity.x > 0)
        {
            Flip();
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            anim.SetBool("run", true);

        }
        else
        {
            anim.SetBool("run", false);
        }



    }

    public void Jump()
    {
        if (isGrounded & Input.GetAxis("Jump") != 0)
        {
            rBody.AddForce(new Vector2(0f, jumpForce));
            isGrounded = false;
            anim.SetTrigger("jump");
        }
    }


}
