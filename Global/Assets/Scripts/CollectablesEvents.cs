using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesEvents : MonoBehaviour
{
    // アイテムの速度をリセットする
    public void ResetVelocity()
    {
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        OnCollider();
        SoundManager.Instance.PlaySe("Coins 0" + Random.Range(1, 5));
    }

    // プレイヤーが拾えるために、コライダーを開く
    public void OnCollider()
    {
        transform.parent.Find("Shadow").GetComponent<Collider2D>().enabled = true;
    }
}
