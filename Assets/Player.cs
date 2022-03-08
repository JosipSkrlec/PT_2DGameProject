using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float horizontalSpeed;

    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;

    //---

    [SerializeField]
    private Transform[] groundCheckPoints;

    [SerializeField]
    private LayerMask groundLayer;

    private bool jump;

    [SerializeField]
    private float jumpForce;

    private Collider2D[] groundCheckResults = new Collider2D[1];



    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }
    void Start()
    {

    }

    // Update is called per frame 
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        //jump = Input.GetButtonDown("Jump");

        //Debug.Log($"{nameof(horizontalInput)}:{horizontalInput}");
        myRigidbody2D.velocity = new Vector2(
            horizontalInput * horizontalSpeed, myRigidbody2D.velocity.y
            );

        if (IsFacingBackwards(horizontalInput))
        {
            HorizontalFlip();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump!");
            Jump();
        }

        myAnimator.SetFloat("horizontalSpeed", Mathf.Abs(horizontalInput * horizontalSpeed));
    }

    private void FixedUpdate()
    {
        bool isOnGround = CheckGround();

        if(jump && isOnGround)
        {
            Jump();
        }

        jump = false;
    }

    private bool CheckGround()
    {

        const float epsilon = 0.01f;
        if (Mathf.Abs(myRigidbody2D.velocity.y) > epsilon) 
        {
            return false;
        }

        for (int i = 0; i < groundCheckPoints.Length; i++)
        {

            if  (
                Physics2D.OverlapPointNonAlloc
                (groundCheckPoints[1].position, 
                groundCheckResults, 
                groundLayer) 
                > 0
                //&& Mathf.Abs (myRigidbody2D.velocity).y < 0.01f
                )
                return true;
        }

        return false; 
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
        //checks direction via the sign of the result, if input zero default to false
        return (transform.right.x * horizontalInput < 0);
    }
}
