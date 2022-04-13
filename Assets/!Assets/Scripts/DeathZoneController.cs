using UnityEngine;

/// <summary>
/// control when player fell down over the map bounds!
/// </summary>
public class DeathZoneController : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private Vector3 _startPositionOfPlayer; // TODO - maybe we can do some checkpoints! + health!

    private void Awake()
    {
        _startPositionOfPlayer = _player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player death!");
            // TODO - maybe do game restart or something when player fell down!
            collision.gameObject.GetComponent<Transform>().position = _startPositionOfPlayer;
        }
    }
}
