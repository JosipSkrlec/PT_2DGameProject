using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _horizontalSpeed;
    [Space(5)]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _wallPatrolPoint;
    [SerializeField] private Transform _groundPatrolPoint;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    private Rigidbody2D _thisRB;
    private Animator _thisAnim;
    float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        _thisAnim = GetComponent<Animator>();
        _thisRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool groundCheck = Physics2D.OverlapCircle(_groundPatrolPoint.position,0.25f,_groundLayer);
        bool wallCheck = Physics2D.OverlapCircle(_wallPatrolPoint.position,0.25f,_wallLayer);

        // if point overlap with wall or if it doesnt overlap with ground entres to flip
        if (groundCheck == false || wallCheck == true)
        {
            HorizontalFlip();
        }
    }

    private void FixedUpdate()
    {
       _thisRB.velocity = new Vector2(transform.right.x * _horizontalSpeed, _thisRB.velocity.y);
    }

    private void HorizontalFlip()
    {
        transform.Rotate(0, 180, 0);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_groundPatrolPoint.position, 0.25f);
        Gizmos.DrawSphere(_wallPatrolPoint.position, 0.25f);
        Gizmos.DrawSphere(_attackPoint.position, 0.25f);
    }


}
