using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    [SerializeField] public int value = 10;

    private Rigidbody2D rBody;

    //private float countTime = 0f;
    //private float waitTime = 150f;
    private float gravityScale = 0f;

    private Vector2 maxPosition;
    private Vector2 minPosition;

    // Start is called before the first frame update
    void Start()
    {
        maxPosition = new Vector2(transform.position.x, transform.position.y + 0.1f);
        minPosition = new Vector2(transform.position.x, transform.position.y - 0.1f);

        rBody = GetComponent<Rigidbody2D>();
        gravityScale = rBody.GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    void Update()
    {if (rBody.velocity.magnitude > 1)
        {
            rBody.velocity = Vector2.ClampMagnitude(rBody.velocity, 1);
        }
        else if (rBody.velocity.magnitude < -1)
        {
            rBody.velocity = Vector2.ClampMagnitude(rBody.velocity, -1);
        }

        if (transform.position.y >= maxPosition.y)
        {
            rBody.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        }
        else if (transform.position.y <= minPosition.y)
        {
            rBody.GetComponent<Rigidbody2D>().gravityScale = -gravityScale;
        }
    }
}
