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
            if (!_playerManager.GetPlayerList()[i].TryGetComponent(out BetterPlayerControl _better))
            {
                break;
            }
            foreach (GameObject prefab in _prefabList)
            {
                GameObject obj = Instantiate(prefab, transform);
                if(obj.TryGetComponent(out WorldToScreenUI world))
                {
                    world.Target = _playerManager.GetPlayerList()[i];
                }
                if (obj.TryGetComponent(out Resurrection resurrection))
                {
                    _better.Resurrect = resurrection;
                }
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                obj.name = prefab.name + i;

                _objects.Add(obj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
