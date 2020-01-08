using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectName : MonoBehaviour
{
    [SerializeField, Tooltip("登録できる名前の長さ")]
    private int _nameLength;
    // 表示する名前
    private Text _text;
    private Controller _gamePad;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = " ";
        for (int i = 0; i < _nameLength; i++)
        {
            _text.text += "- ";
        }
        Test();
    }
    public void Test()
    {
        GamePadManager.Instance.GetGamepad(1).HaveTarget = true;
        _gamePad = GamePadManager.Instance.GetGamepad(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (_gamePad.GetStickL().Y > 0.5)
        {
            Debug.Log("上方向");
        }
        else if (_gamePad.GetStickL().Y < -0.5)
        {
            Debug.Log("下方向");
        }
    }
}
