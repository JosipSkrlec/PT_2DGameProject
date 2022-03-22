using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool SwingEnabled = false;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheckPoints;
    [SerializeField] private LayerMask groundLayer;

    private Collider2D[] groundCheckResults = new Collider2D[1];
    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    private bool jump;


    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        //jump = Input.GetButtonDown("Jump");
        //Debug.Log($"{nameof(horizontalInput)}:{horizontalInput}");


        //if (IsFacingBackwards(horizontalInput))
        //{
        //    myAnimator.SetTrigger("Walk");
        //    HorizontalFlip();
        //}

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (IsFacingBackwards(horizontalInput))
            {
                HorizontalFlip();
            }
            myAnimator.SetTrigger("Walk");
            myRigidbody2D.velocity = new Vector2(horizontalInput * horizontalSpeed, myRigidbody2D.velocity.y);
        }
        else
        {
            myAnimator.SetTrigger("Idle");
        }

        if (Input.GetKey(KeyCode.C) && CheckGround())
        {
            myAnimator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKey(KeyCode.V))
        {
            SwingEnabled = true;
        }
        else
        {
            SwingEnabled = false;
        }
        //myAnimator.SetFloat("horizontalSpeed", Mathf.Abs(horizontalInput * horizontalSpeed));
    }

    private void FixedUpdate()
    {
        bool isOnGround = CheckGround();

        if (jump && isOnGround)
        {
            jump = false;
            isOnGround = false;
            Jump();
        }
    }

    private bool CheckGround()
    {
        if (Physics2D.OverlapCircle(groundCheckPoints.position, 0.5f, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Jump()
    {
        myRigidbody2D.AddForce(Vector2.up * jumpForce);
    }

    private void HorizontalFlip()
    {
        transform.Rotate(0, 180, 0);
    }

    private bool IsFacingBackwards(float horizontalInput)
    {
        return (transform.right.x * horizontalInput < 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(groundCheckPoints.position, 0.5f);
    }
}