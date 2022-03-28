using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    public bool SwingEnabled = false;
    public bool ConnectedToSwingPoint = false;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundCheckPoints;
    [SerializeField] private LayerMask _groundLayer;

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

        if (Input.GetKey(KeyCode.A) && ConnectedToSwingPoint == false || Input.GetKey(KeyCode.D) && ConnectedToSwingPoint == false)
        {
            if (IsFacingBackwards(horizontalInput))
            {
                HorizontalFlip();
            }
            myAnimator.SetTrigger("Walk");
            myRigidbody2D.velocity = new Vector2(horizontalInput * _walkSpeed, myRigidbody2D.velocity.y);
        }
        else
        {
            myAnimator.SetTrigger("Idle");
        }

        if (Input.GetKey(KeyCode.C) && CheckGround() && ConnectedToSwingPoint == false)
        {
            myAnimator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.Space) && ConnectedToSwingPoint == false)
        {
            jump = true;
        }
        // swing mechanics
        if (Input.GetKey(KeyCode.LeftShift))
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
        if (Physics2D.OverlapCircle(_groundCheckPoints.position, 0.5f, _groundLayer))
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
        myRigidbody2D.AddForce(Vector2.up * _jumpForce);
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
        if (_groundCheckPoints == null)
        {
            return;
        }
        Gizmos.DrawSphere(_groundCheckPoints.position, 0.5f);
    }
}