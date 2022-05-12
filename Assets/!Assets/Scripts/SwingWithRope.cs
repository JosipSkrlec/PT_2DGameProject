using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWithRope : MonoBehaviour
{
    [SerializeField] private bool _connectionEnabled = true;
    [SerializeField] private float _swingConnectionRadius;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private DistanceJoint2D _distanceJoint2D;
    [SerializeField] private LineRenderer _lineRenderer; // izgled line-a koji povezuje playera i grab point kada su konektani!

    private void Start()
    {
        // TODO - postaviti ovo drugacije jer u slucaju okretanja grab-a nece funkcionirati line renrerer
        _lineRenderer.SetPosition(0, this.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (!_connectionEnabled)
        {
            return;
        }

        Collider2D overlayColliders = Physics2D.OverlapCircle(transform.position, _swingConnectionRadius, _playerMask);

        if (overlayColliders != null)
        {
            PlayerInside();
            PlayerController player = overlayColliders.GetComponent<PlayerController>();

            if (player.SwingEnabled)
            {
                if (player.ConnectedToSwingPoint == true)
                {
                    // TODO - do some offset to player position!
                    _lineRenderer.SetPosition(1, player.transform.position);

                    return;
                }
            
                player.ConnectedToSwingPoint = true;
                float distance = Vector2.Distance(overlayColliders.transform.position, transform.position);
                _distanceJoint2D.distance = distance;
                //Debug.Log("Player Connected to swing point!");
                _distanceJoint2D.connectedBody = overlayColliders.transform.GetComponent<Rigidbody2D>();
                // automatic disconnection!
                //StartCoroutine(StopSwing());
              
            }
            else
            {
                //_distanceJoint2D.enabled = false;
                player.ConnectedToSwingPoint = false;
                _distanceJoint2D.connectedBody = null;
                //Debug.Log("Player Disconnected to swing point!");
            }
        }
        else
        {
            PlayerOutside();
        }

        //foreach (var hitCollider in overlayColliders)
        //{
        //    Debug.Log("Connected!");
        //    _distanceJoint2D.connectedBody = hitCollider.transform.GetComponent<Rigidbody2D>();
        //}

    }

    private void PlayerInside()
    {
        _spriteRenderer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        _spriteRenderer.color = Color.green;
    }

    private void PlayerOutside()
    {
        _lineRenderer.SetPosition(1, this.transform.position);

        _spriteRenderer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _spriteRenderer.color = Color.red;
    }


    // TODO - napraviti da se odspoji pritiskom na gumb iz player-a!
    IEnumerator StopSwing()
    {
        yield return new WaitForSeconds(0.75f);

        _distanceJoint2D.enabled = false;
        _distanceJoint2D.connectedBody = null;
        Debug.Log("Player Disconnected to swing point!");


        yield return new WaitForSeconds(2.0f);

        _distanceJoint2D.enabled = true;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _swingConnectionRadius);
    }

}
