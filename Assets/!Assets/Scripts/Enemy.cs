using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool _isPatrolling = true;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _agroDistance;
    [Space(5)]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _wallPatrolPoint;
    [SerializeField] private Transform _groundPatrolPoint;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    //[Space(5)]
    //[SerializeField] private Transform _agroDistanceTransform;

    private Rigidbody2D _thisRB;
    private Animator _thisAnim;
    private RaycastHit objectHit;

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

        var agroRaycast = Physics2D.Raycast(
            transform.position + new Vector3(0.0f, 1.0f, 0.0f),
            transform.position + new Vector3(_agroDistance, 1.0f, 0.0f),
            _playerLayer
        );

        //if (agroRaycast == true)
        //{
        //    Debug.Log("FOUND " + agroRaycast.transform.name);
        //}

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_groundPatrolPoint.position, 0.25f);
        Gizmos.DrawSphere(_wallPatrolPoint.position, 0.25f);

        Gizmos.DrawSphere(_attackPoint.position, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position+ new Vector3(0.0f, 1.0f, 0.0f), transform.position + new Vector3(-_agroDistance, 1.0f, 0.0f));



    }

}
