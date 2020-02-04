using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionData : MonoBehaviour
{
    private Transform _shadow;
    private Vector2 _shadowSize = new Vector2(0.0f, 0.0f);
    private bool _isCollected;

    [SerializeField]
    private GameObject _textPrefab;

    [Header("General")]
    [SerializeField]
    private int _value = 100;
    [SerializeField]
    private Color _color = new Color(1, 1, 1);
    [SerializeField]
    private string _playerName = "Player 1";

    // Start is called before the first frame update
    void Start()
    {
        _shadow = transform.Find("Shadow");
        _shadowSize = _shadow.GetComponent<Renderer>().bounds.size;
        _isCollected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollide(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollide(collision);
    }

    private void CheckCollide(Collider2D collision)
    {
        if (!_isCollected && collision.CompareTag("Player"))
        {
            Transform collisionShadow = collision.transform.parent.Find("Shadow");
            float collisionDepth = collisionShadow.GetComponent<Depth>().DepthSetting;
            Vector2 collisionShadowSize = collisionShadow.GetComponent<Renderer>().bounds.size;

            Rect shadowA = new Rect((Vector2)_shadow.position - _shadowSize / 2.0f, _shadowSize);
            Rect shadowB = new Rect((Vector2)collisionShadow.position - collisionShadowSize / 2.0f, collisionShadowSize);

            if (shadowA.Overlaps(shadowB, true))
            {
                Transform sprite = transform.Find("Sprite");
                sprite.GetComponent<Flasher>().StartFlash(0.2f);
                sprite.GetComponent<Animator>().SetBool("Collected", true);
                Destroy(gameObject, 0.3f);
                _isCollected = true;
                SoundManager.Instance.PlaySe("Coins 0" + Random.Range(1, 5));

                if(collision.transform.parent.name == _playerName)
                {
                    _value = 1000;
                }

                collision.GetComponentInParent<Money>().IncreaseMoney(_value);

                SpawnText(collision);
            }
        }
    }

    private void SpawnText(Collider2D collision)
    {
        Transform player = collision.transform;
        GameObject canvas = GameObject.Find("Canvas");

        GameObject text = Instantiate(_textPrefab, player.position, player.rotation) as GameObject;
        text.transform.SetParent(canvas.transform);
        text.SetActive(true);

        var offset = Random.Range(-player.GetComponent<Collider2D>().bounds.size.x / 2, player.GetComponent<Collider2D>().bounds.size.x / 2);
        text.transform.position = Camera.main.WorldToScreenPoint(new Vector3(player.position.x + offset, player.position.y + 1f, player.position.z));
        text.transform.rotation = Quaternion.Euler(new Vector3(0, 0));
        text.GetComponent<Text>().text = _value.ToString();

        text.GetComponent<Outline>().effectColor = _color;
    }
}
