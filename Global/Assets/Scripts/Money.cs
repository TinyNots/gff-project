using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    private int _money;

    // Start is called before the first frame update
    void Start()
    {
        _money = 0;
    }

    public void IncreaseMoney(int value)
    {
        _money += value;
    }

    public int GetMoney()
    {
        return _money;
    }
}
