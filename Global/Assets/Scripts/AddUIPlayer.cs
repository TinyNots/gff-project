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
    private List<Character> _charList;

    // Start is called before the first frame update
    void Start()
    {
        _objects = new List<GameObject>();
        _charList = new List<Character>();
        CreateUI();
    }
    
    private void CreateUI()
    {
        for (int i = 0; i < _playerManager.GetPlayerList().Count; i++)
        {
            if (!_playerManager.GetPlayerList()[i].TryGetComponent(out Character character))
            {
                break;
            }
            _charList.Add(character);
            foreach (GameObject prefab in _prefabList)
            {
                GameObject obj = Instantiate(prefab, transform);
                if(obj.TryGetComponent(out WorldToScreenUI world))
                {
                    world.Target = _playerManager.GetPlayerList()[i];
                }
                if (obj.TryGetComponent(out Resurrection resurrection))
                {
                    resurrection.SetPlayer(character);
                }
                obj.transform.parent = this.transform;
                obj.SetActive(character.IsDie);
                obj.name = prefab.name + i;

                _objects.Add(obj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in _objects)
        {
            if (obj.TryGetComponent(out Resurrection resurrection))
            {
                resurrection.gameObject.SetActive(resurrection.GetChara.IsDie);
            }
        }
    }
}
