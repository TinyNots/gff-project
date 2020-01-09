using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldToScreenUI : MonoBehaviour
{
    [SerializeField, Tooltip("ターゲットプレイヤー")]
    private GameObject _target;
    [SerializeField, Tooltip("オフセット")]
    private Vector3 _offset;
    private RectTransform _rectTrans;
    public GameObject Target
	{
		get { return _target; }
		set { _target = value; }
	}
    // Start is called before the first frame update
    void Start()
    {
        _rectTrans = GetComponent<RectTransform>();
    }

    private void ScreenToWorld()
    {
        _rectTrans.position = Camera.main.WorldToScreenPoint(_target.transform.position + _offset);
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        ScreenToWorld();
    }
}
