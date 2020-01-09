using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _playerGroup;
    [SerializeField]
    private GameObject _playerPrefab;
    private int _playerTotalIndex;

    [SerializeField]
    private GameObject _idPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _playerTotalIndex = 0;
        _playerGroup = new List<GameObject>();
        if (_playerPrefab == null)
        {
            Debug.LogError("Player prefab is missing");
            return;
        }

        for (int i = 1; i <= 2; i++)
        {
            GameObject player = Instantiate(_playerPrefab);
            player.name = "Player " + i;
            player.transform.parent = gameObject.transform;

            GameObject id = Instantiate(_idPrefab, player.transform);
            Text text = id.transform.Find("Text").GetComponent<Text>();

            text.text = "P" + i;

            _playerGroup.Add(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerTotalIndex < 4)
        {
            int currentIndex = GamePadManager.Instance.GetControllerByButton("Start");
            if (currentIndex != 0 && !GamePadManager.Instance.GetGamepad(currentIndex).HaveTarget)
            {
                GamePadManager.Instance.GetGamepad(currentIndex).HaveTarget = true;
                _playerGroup[_playerTotalIndex].GetComponent<BetterPlayerControl>().SetControllerIndex(currentIndex);
                _playerTotalIndex++;
            }
        }

        int flagCount = 0;
        foreach (GameObject player in _playerGroup)
        {
            if (player.GetComponent<Character>().IsDie)
            {
                flagCount++;
            }
        }

        if (flagCount >= _playerGroup.Count)
        {
            SceneCtl.instance.NextScene(SceneCtl.SCENE_ID.RESULT);
        }
    }
}