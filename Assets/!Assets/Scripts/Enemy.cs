using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool _isPatrolling = true;
    [SerializeField] private float _walkSpeed;
    [Space(5)]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _wallPatrolPoint;
    [SerializeField] private Transform _groundPatrolPoint;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [Space(5)]
    [SerializeField] private Transform _agroDistanceTransform;

    private Rigidbody2D _thisRB;
    private Animator _thisAnim;

    // Start is called before the first frame update
    void Start()
    {
        _thisAnim = GetComponent<Animator>();
        _thisRB = GetComponent<Rigidbody2D>();

        StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPatrolling == false)
        {
            return;
        }

        bool groundCheck = Physics2D.OverlapCircle(_groundPatrolPoint.position,0.25f,_groundLayer);
        bool wallCheck = Physics2D.OverlapCircle(_wallPatrolPoint.position,0.25f,_wallLayer);

        // if point overlap with wall or if it doesnt overlap with ground entres to flip
        if (groundCheck == false || wallCheck == true)
        {
            HorizontalFlip();
        }

        var test = Physics2D.Raycast(transform.position, transform.right, _playerLayer);
        Debug.Log(test);
        if (test == true)
        {
            Debug.Log("FOUND " + test.transform.name);
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

    private void HorizontalFlip()
    {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_groundPatrolPoint.position, 0.25f);
        Gizmos.DrawSphere(_wallPatrolPoint.position, 0.25f);

        Gizmos.DrawSphere(_attackPoint.position, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position+ new Vector3(0.0f, 1.0f, 0.0f), _agroDistanceTransform.position);

    }

}
