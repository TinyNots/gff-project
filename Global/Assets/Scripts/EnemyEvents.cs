using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private Transform[] _coinPrefabs;

    [SerializeField]
    private int min = 1;
    [SerializeField]
    private int max = 1;

    public void SpawnCoin()
    {
        int randomNumber = Random.Range(min, max);
        for (int i = 0; i < randomNumber; i++)
        {
            Transform coin = Instantiate(_coinPrefabs[Random.Range(0, _coinPrefabs.Length)], transform.parent.Find("Shadow").position, Quaternion.identity);
            float speed = 200.0f * Time.deltaTime;
            coin.GetComponent<Rigidbody2D>().velocity = Random.onUnitSphere * speed;
        }
    }

    public void ResetVelocity()
    {
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
    }

    public void ShakeScreen()
    {
        CameraShaker.ShakeOnce(0.5f, 2.0f, new Vector3(1.0f, 1.0f, 0.0f) * 1.0f);
    }

    public void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
