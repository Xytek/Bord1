using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public bool isDead = false;
    private float speed = 10;
    private float jumpForce = 500;
    private int extraJumps = 2;
    private float checkRadius = 0.05f;

    private bool canMove = true;
    private Vector2 lastPosition;
    private Vector2 beforeLastPosition;

    private bool isGrounded;
    private int extraJumpsLeft;
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;

    private Vector3 oldPosition;

    [SerializeField]
    AudioSource JumpSound = null;
    bool isJumpSoundPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (isGrounded == true)
            {
                extraJumpsLeft = extraJumps;
            }
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && extraJumpsLeft > 0 && !isDead)
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(transform.up * jumpForce);
                extraJumpsLeft--;
                if (!isJumpSoundPlayed)
                {
                JumpSound.Play();
                    isJumpSoundPlayed = true;
                }
                
            }
            else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && extraJumpsLeft == 0 && isGrounded == true && !isDead)
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(transform.up * jumpForce);
            }

            if(!JumpSound.isPlaying)
            {
                isJumpSoundPlayed = false;
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
            moveInput = Input.GetAxis("Horizontal");

            if (lastPosition.y == rb.position.y && beforeLastPosition.y == lastPosition.y && isGrounded == false)
                canMove = false;
           
            if (lastPosition.x != rb.position.x || isGrounded)
                canMove = true;

            if (canMove && !isDead)
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

            beforeLastPosition = lastPosition;
            lastPosition = rb.position;

        }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        if (!isLocalPlayer)
        {

            if (oldPosition.x != transform.position.x)
            {
                //Must have moved.


                if (oldPosition.x - transform.position.x > 0)
                {
                    //Moved left
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else if (oldPosition.x - transform.position.x < 0)
                {
                    //Moved right
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

            }
            oldPosition = transform.position;

        }




    }



    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
