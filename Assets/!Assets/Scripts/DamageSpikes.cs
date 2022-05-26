using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageSpikes : MonoBehaviour
{
    [SerializeField] private float _spikeDamage;
    [SerializeField] private float _spikeCooldown;
    [SerializeField] private float _animateDuration;

    [SerializeField] private BoxCollider2D _boxCollider2D;

    private Vector3 _startingSpikePosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("item collected!");

            _startingSpikePosition = this.transform.position;

            _boxCollider2D.enabled = false;

            collision.GetComponent<PlayerController>().TakeDamageToPlayer(_spikeDamage);

            this.gameObject.transform.DOMove(
                new Vector3(transform.position.x,transform.position.y -1.0f,transform.position.z),
                _animateDuration,
                false)
            .SetEase(Ease.InOutQuad);

            StartCoroutine(ActivateSpikeAgain());

            // TODO  - play sound
            //Destroy(this.gameObject);
        }
    }

    private IEnumerator ActivateSpikeAgain()
    {
        yield return new WaitForSeconds(_spikeCooldown);

        this.gameObject.transform.DOMove(
            new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z),
            _animateDuration,
            false)
        .SetEase(Ease.InOutQuad).OnComplete(()=> _boxCollider2D.enabled = true);

    }

}
