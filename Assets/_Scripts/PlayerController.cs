using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float jumpForce = 500.0f;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject CanvasFail;
    [SerializeField] private GameObject CanvasWin;

    [SerializeField] private Text pointText;
    [SerializeField] private Text hpText;
    //[SerializeField] private GameObject CanvasIngame;

    // private variables
    private Rigidbody2D rBody;
    private Animator anim;
    private bool isGrounded = false;
    private bool isClimbing = false;
    private bool isFacingRight = true;
    //public AudioSource death_sound;

    private int score = 0;
    public int hp = 3;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        //text.text = 0;
        score = Convert.ToInt32(pointText.text);
        if (Convert.ToInt32(hpText.text) > 0)
        {
            hp = Convert.ToInt32(hpText.text);
        }
        hpText.text = Convert.ToString(hp);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {

            hp -= 1;
            hpText.text = Convert.ToString(hp);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something Triggered");

        if (collision.gameObject.tag == "Berry")
        {
            hp += 1;
            Debug.Log(hp);
            Destroy(collision.gameObject);
            hpText.text = Convert.ToString(hp);

        }

        if (collision.gameObject.tag == "ladder")
        {
            isClimbing = true;
            rBody.GetComponent<Rigidbody2D>().gravityScale = 0;

        }

        if (collision.gameObject.tag == "finish")
        {
            CanvasWin.SetActive(true);
        }

        if (collision.gameObject.tag == "Collectable")
        {
            score += collision.gameObject.GetComponent<GemScript>().value;
            Destroy(collision.gameObject);
            pointText.text = Convert.ToString(score);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Something Triggered");

        if (collision.gameObject.tag == "ladder")
        {
            isClimbing = false;
            rBody.GetComponent<Rigidbody2D>().gravityScale = 2;

        }

    }

    IEnumerator SetCanvas()
    {
        yield return new WaitForSeconds(2);

        CanvasFail.SetActive(true);      
    }

    private void FixedUpdate()
    {
        if (hp == 0)
        {
            //death_sound.Play();
            anim.SetTrigger("die");
            rBody.AddForce(new Vector2(0f, jumpForce));
            rBody.GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<PlayerController>().enabled = false;


            StartCoroutine(SetCanvas());
        }

        isGrounded = GroundCheck();
        Walk();
        Climb();
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

    public void Climb()
    {
        if (isClimbing == true)
        {
            float verti = Input.GetAxis("Vertical");
            rBody.velocity = new Vector2(rBody.velocity.x, verti * speed);
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
