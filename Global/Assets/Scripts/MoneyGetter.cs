using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGetter : MonoBehaviour
{
    [SerializeField]
    private string _playerName = "Player 1";

    private GameObject _player;
    private Text _scoreText;

    private void Start()
    {
        _scoreText = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        if(_player == null)
        {
            _player = GameObject.Find(_playerName);
        }

        _scoreText.text = _player.GetComponent<Money>().GetMoney().ToString();
    }
}
