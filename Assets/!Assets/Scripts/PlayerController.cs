using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerStates")]
    public bool SwingEnabled = false;
    public bool ConnectedToSwingPoint = false;
    [Space(5)]
    [Header("Player adjustable settings!")]
    [SerializeField] private int _health = 100; // 100
    [SerializeField] private int _lives = 3; // 3
    [SerializeField] private float _damage = 25.0f; // 25.0f
    [SerializeField] private float _walkSpeed; // 5.0f
    [SerializeField] private float _jumpForce; // 750.0f
    [SerializeField] private float _swingForce; // 6.0f
    [Space(5)]
    [SerializeField] private Transform _groundCheckPoints;
    [SerializeField] private LayerMask _groundLayer;
    [Space(5)]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayer;

    #region Private fields
    private Collider2D[] _groundCheckResults = new Collider2D[1];
    private Rigidbody2D _myRigidbody2D;
    private Animator _myAnimator;
    private bool _jump;
    private int _playerMaxHealth;
    private bool _isAttacking = false;
    #endregion

    private void Awake()
    {
        _myRigidbody2D = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();

        if (_lives >= 0)
        {
            PlayerUIController.Instance.UpdatePlayerHealthInUI(_lives);
        }
        else
        {
            _lives = 1;
            PlayerUIController.Instance.UpdatePlayerHealthInUI(_lives);
        }

        _playerMaxHealth = _health;
    }

    void Update()
    {
        if (_isAttacking == true)
        {
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        //jump = Input.GetButtonDown("Jump");
        //Debug.Log($"{nameof(horizontalInput)}:{horizontalInput}");


        //if (IsFacingBackwards(horizontalInput))
        //{
        //    myAnimator.SetTrigger("Walk");
        //    HorizontalFlip();
        //}

        // swing add force testings!
        //if (Input.GetKey(KeyCode.B))
        //{
        //    //myRigidbody2D.AddForce(new Vector2(_test, _test));
        //    //myRigidbody2D.AddTorque(_test);

        //    myRigidbody2D.AddForce(transform.right * _test);

        //    //myRigidbody2D.AddRelativeForce(new Vector2(_test, _test),ForceMode2D.Force);

        //}

        if (Input.GetKey(KeyCode.A) /*&& ConnectedToSwingPoint == false*/ || Input.GetKey(KeyCode.D) /*&& ConnectedToSwingPoint == false*/)
        {
            if (IsFacingBackwards(horizontalInput))
            {
                HorizontalFlip();
            }

            if (ConnectedToSwingPoint == false)
            {
                _myAnimator.SetTrigger("Walk");
                _myRigidbody2D.velocity = new Vector2(horizontalInput * _walkSpeed, _myRigidbody2D.velocity.y);
            }
            else
            {
                // TODO - potencially we can add swing animation!
                _myRigidbody2D.AddForce(transform.right * _swingForce); // swing add force!
            }

        }
        else
        {
            _myAnimator.SetTrigger("Idle");
        }

        if (Input.GetKeyDown(KeyCode.C) && CheckGroundedState() && ConnectedToSwingPoint == false)
        {
            if (_isAttacking == false)
            {
                _isAttacking = true;
                _myAnimator.SetTrigger("Attack");

            }
            //TakeDamageToEnemy();
        }

        if (Input.GetKeyDown(KeyCode.Space) && ConnectedToSwingPoint == false)
        {
            //_jump = true;
            bool isOnGround = CheckGroundedState();

            if (isOnGround)
            {
                Jump();
            }
            else
            {
                Debug.Log("Player is not in the ground!");
            }

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
        //bool isOnGround = CheckGroundedState();

        //if (_jump && isOnGround)
        //{
        //    _jump = false;
        //    isOnGround = false;
        //    Jump();
        //}
    }

    private bool CheckGroundedState()
    {
        if (Physics2D.OverlapCircle(_groundCheckPoints.position, 0.15f, _groundLayer))
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
        _myRigidbody2D.AddForce(Vector2.up * _jumpForce);
    }

    public void TakeDamageToEnemy()
    {
        //bool attack = Physics2D.OverlapCircle(_attackPoint.position, 0.25f, _enemyLayer);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackPoint.position, 0.25f,_enemyLayer);

        if (colliders.Length > 0)
        {
            colliders[0].GetComponent<EnemyController>().TakeDamageToEnemy(_damage);
        }

    }

    // called on the end of animation as EVENT!
    public void AttackFinished()
    {
        _isAttacking = false;
    }

    private void HorizontalFlip()
    {
        transform.Rotate(0, 180, 0);
    }

    private bool IsFacingBackwards(float horizontalInput)
    {
        return (transform.right.x * horizontalInput < 0);
    }

    public void TakeDamageToPlayer(float damage)
    {
        Debug.Log("Take damage to player, damage = " + damage);
        _health -= (int)damage;

        // TODO - do life taking if player health reaches < -0.0f
        if (_health <= 0)
        {
            _lives -= 1;

            if (_lives <= 0)
            {
                PlayerUIController.Instance.UpdatePlayerHealthInUI(0);
                Debug.LogError("There is no more lives -> player is Death!");
            }
            else
            {
                _health = _playerMaxHealth;
                PlayerUIController.Instance.UpdatePlayerHealthInUI(_lives);

            }
        }

        PlayerUIController.Instance.UpdatePlayerHealthUI(_playerMaxHealth,_health);
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheckPoints == null)
        {
            return;
        }
        Gizmos.DrawSphere(_groundCheckPoints.position, 0.15f);
    }

}