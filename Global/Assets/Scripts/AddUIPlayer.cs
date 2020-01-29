using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddUIPlayer : MonoBehaviour
{
    // 複製するオブジェクト
    [SerializeField]
    private List<GameObject> _prefabList = new List<GameObject>();
    // プレイヤーマネージャークラス
    [SerializeField, Tooltip("プレイヤーマネージャークラス")]
    private PlayerManager _playerManager = null;
    // 増やしたオブジェクト
    private List<GameObject> _objects;

    // Start is called before the first frame update
    void Start()
    {
        _objects = new List<GameObject>();

        CreateUI();
    }
    
    private void CreateUI()
    {
        for (int i = 0; i < _playerManager.GetPlayerList().Count; i++)
        {
            foreach (GameObject prefab in _prefabList)
            {
                GameObject obj = Instantiate(prefab, transform);
                WorldToScreenUI world = obj.GetComponent<WorldToScreenUI>();
                world.Target = _playerManager.GetPlayerList()[i];
                obj.transform.parent = this.transform;
                obj.name = obj.name + _playerManager.GetPlayerList()[i].name;
                _objects.Add(obj);
            }
        }
    }

    private void SetUI()
    {
        for(int i = 0; i < _playerManager.GetPlayerList().Count; i++)
        {
            WorldToScreenUI world = GetComponent<WorldToScreenUI>();
            world.Target = _playerManager.GetPlayerList()[i];
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}
