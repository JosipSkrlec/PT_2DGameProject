using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private bool _connectionEnabled = true;
    [SerializeField] private float _swingConnectionRadius;
    [SerializeField] private LayerMask _playerMask;

    [SerializeField] private DistanceJoint2D _distanceJoint2D;

    // Update is called once per frame
    void Update()
    {
        Collider2D overlayColliders = Physics2D.OverlapCircle(transform.position, _swingConnectionRadius, _playerMask);

        if (overlayColliders != null)
        {
            Debug.Log("Connected!");
            _distanceJoint2D.connectedBody = overlayColliders.transform.GetComponent<Rigidbody2D>();

            StartCoroutine(StopSwing());
        }

        //foreach (var hitCollider in overlayColliders)
        //{
        //    Debug.Log("Connected!");
        //    _distanceJoint2D.connectedBody = hitCollider.transform.GetComponent<Rigidbody2D>();
        //}

    }

    IEnumerator StopSwing()
    {
        yield return new WaitForSeconds(0.75f);

        _distanceJoint2D.enabled = false;
        _distanceJoint2D.connectedBody = null;

        yield return new WaitForSeconds(2.0f);

        _distanceJoint2D.enabled = true;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _swingConnectionRadius);
    }



}
