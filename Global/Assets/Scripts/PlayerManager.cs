using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _playerGroup;
    [SerializeField]
    private GameObject _playerPrefab;
    private int _playerTotalIndex;

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

        for (int i = 1; i <= 4; i++)
        {
            GameObject player = Instantiate(_playerPrefab);
            player.name = "Player " + i;
            player.transform.parent = gameObject.transform;
            _playerGroup.Add(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int currentIndex = GamePadManager.Instance.GetControllerByButton("Start");
        if(currentIndex != 0)
        {
            _playerGroup[_playerTotalIndex].GetComponent<PlayerMovement>().SetControllerIndex(currentIndex);
            _playerTotalIndex++;
        }
    }
}
