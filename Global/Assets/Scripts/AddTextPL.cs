using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTextPL : MonoBehaviour
{
    // 複製するオブジェクト
    [SerializeField]
    private List<GameObject> _prefabList = new List<GameObject>();
    [SerializeField]
    private PlayerManager _playerManager;
    // 増やしたオブジェクト
    private List<GameObject> _objects;

    // Start is called before the first frame update
    void Start()
    {
        CreateStatus();
    }
    
    private void CreateStatus()
    {
        for (int i = 0; i < _playerManager.GetPlayerList().Count; i++)
        {
            foreach (GameObject prefab in _prefabList)
            {
                _objects.Add(Instantiate(prefab, transform));
            }
        }
    }

    private void SetPoint()
    {
        // プレイヤーのリストを取得
        foreach (var player in _playerManager.GetPlayerList())
        {
            transform.position = Camera.main.WorldToScreenPoint(new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z));
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0));

        }
    }

    // Update is called once per frame
    void Update()
    {
        SetPoint();
    }
}
