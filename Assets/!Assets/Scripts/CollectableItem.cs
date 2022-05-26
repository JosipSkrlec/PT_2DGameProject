using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("item collected!");

            collision.GetComponent<PlayerController>().CollectItem();
            // TODO  - play sound
            Destroy(this.gameObject);
        }
    }


}
