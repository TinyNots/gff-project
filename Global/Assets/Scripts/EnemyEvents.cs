using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private Transform _coinPrefab;

    public void SpawnCoin(int min = 1, int max = 1)
    {
        int randomNumber = Random.Range(min, max);
        for (int i = 0; i < randomNumber; i++)
        {
            Transform coin = Instantiate(_coinPrefab, transform.parent.Find("Shadow").position, Quaternion.identity);
            float speed = 150.0f * Time.deltaTime;
            coin.GetComponent<Rigidbody2D>().velocity = Random.onUnitSphere * speed;
        }
    }

    public void ResetVelocity()
    {
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
    }
}
