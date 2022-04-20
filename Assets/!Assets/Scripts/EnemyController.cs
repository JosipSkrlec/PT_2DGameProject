using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool _isPatrolling = true;
    [SerializeField] private bool _isAttacking = false;
    [Space(5)]
    [SerializeField] private int _health = 100;
    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _agroDistance = 4.0f;
    [SerializeField] private float _enemyDamage = 20.0f;
    [SerializeField] private float _attackDelay = 2.0f;
    [Space(5)]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Image _healthBarImage;
    [Space(3)]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _wallPatrolPoint;
    [SerializeField] private Transform _groundPatrolPoint;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    #region private Fields
    private Rigidbody2D _thisRB;
    private Animator _thisAnim;
    private RaycastHit objectHit;
    private bool _readyToAttack;
    private float _enemyMaxHealth;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _readyToAttack = true;
        _thisAnim = GetComponent<Animator>();
        _thisRB = GetComponent<Rigidbody2D>();
        _enemyMaxHealth = _health;

        StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO - check if i can refractor this part!
        bool enemyCheck = Physics2D.OverlapCircle(_wallPatrolPoint.position, 0.25f, _enemyLayer);
        bool groundCheck = Physics2D.OverlapCircle(_groundPatrolPoint.position, 0.25f, _groundLayer);
        bool wallCheck = Physics2D.OverlapCircle(_wallPatrolPoint.position, 0.25f, _wallLayer);
        // if point overlap with wall or if it doesnt overlap with ground entres to flip
        if (groundCheck == false || wallCheck == true || enemyCheck == true)
        {
            HorizontalFlip();
        }

        // raycast fron enemy to forward!
        var agroRaycast = Physics2D.Linecast(
            transform.position + new Vector3(0.0f, 1.0f, 0.0f),
            transform.position + new Vector3(-_agroDistance, 1.0f, 0.0f),
            _playerLayer
        );

        if (agroRaycast == true)
        {
            _walkSpeed = 2.0f;
            _isAttacking = true;
        }
        else
        {
            _walkSpeed = 1.0f;
            _isAttacking = false;
        }


        if (_isAttacking == true)
        {
            bool attack = Physics2D.OverlapCircle(_attackPoint.position, 0.25f, _playerLayer);
            //Debug.Log("attack overlap circle = " + attack);
            if (attack == true)
            {
                if (_readyToAttack)
                {
                    // TODO - take damage to player if he is in the front!!!
                    _playerController.TakeDamageToPlayer(_enemyDamage);
                    _readyToAttack = false;
                    _thisAnim.SetTrigger("Attack");
                    StartCoroutine(AttackDelay());
                    _isPatrolling = false;

                }
            }
            else
            {
                _thisAnim.SetTrigger("Walk");
            }
        }

    }

    private void FixedUpdate()
    {
        if (_isPatrolling == false)
        {
            return;
        }

        _thisRB.velocity = new Vector2(transform.right.x * _walkSpeed, _thisRB.velocity.y);
    }


    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_attackDelay); // TODO - do something better here !

        _readyToAttack = true;
        _isPatrolling = true;

    }

    private void HorizontalFlip()
    {
        _agroDistance *= -1; // to change the direction of raycast!
        transform.Rotate(0, 180, 0);
    }

    private void StopPatroling()
    {
        _thisAnim.SetTrigger("Idle");

        _isPatrolling = false;
    }

    private void StartPatrolling()
    {
        _thisAnim.SetTrigger("Walk");

        _isPatrolling = true;

    }

    public void TakeDamageToEnemy(float damage)
    {
        _health -= (int)damage;

        _healthBarImage.fillAmount = _health / _enemyMaxHealth;

        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_groundPatrolPoint.position, 0.25f);
        Gizmos.DrawSphere(_wallPatrolPoint.position, 0.25f);

        Gizmos.DrawSphere(_attackPoint.position, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0.0f, 1.0f, 0.0f), transform.position + new Vector3(-_agroDistance, 1.0f, 0.0f));

    }

}


// old code!
//Vector3 fwd = transform.TransformDirection(Vector3.forward);
//Debug.DrawRay(transform.position, fwd * 50, Color.green);
//if (Physics.Raycast(transform.position, fwd, out objectHit, 50))
//{
//    //do something if hit object ie
//    if (objectHit.transform.name == "Player")
//    {
//        Debug.Log("Close to enemy");
//    }
//}