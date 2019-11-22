using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField, Tooltip("無効化するオブジェクト")]
    GameObject[] _noActiveObj;
    [SerializeField]
    GameObject[] Buttons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PushButton()
    {
        foreach(var button in Buttons)
        {
            button.SetActive(true);
        }
        foreach (var obj in _noActiveObj)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.A))
        {
            PushButton();
        }
    }
}
