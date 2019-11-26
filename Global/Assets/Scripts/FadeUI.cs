using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    enum FADE_TYPE
    {
        IN,
        OUT,
        FLASH,
        MAX
    }
    [SerializeField]
    private FADE_TYPE _fadeType;
    [SerializeField]
    private float _speed = 1;
    [SerializeField, Tooltip("ループフラグ")]
    private bool _loopFlag;

    private Image _image;
    // 現在の透明度
    private float _alpha;
    private float _nowTime;
    private const int _flashTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        _alpha = 1;
        _loopFlag = false;
        _image = GetComponent<Image>();
    }

    private void FadeIn()
    {
        _alpha += Time.deltaTime * _speed;
        if(_alpha > 1)
        {
            _alpha = 1;
            if(_loopFlag)
            {
                _fadeType = FADE_TYPE.OUT;
            }
        }
    }

    private void FadeOut()
    {
        _alpha -= Time.deltaTime * _speed;
        if (_alpha < 0)
        {
            _alpha = 0;
            if (_loopFlag)
            {
                _fadeType = FADE_TYPE.IN;
            }
        }
    }

    private void Flashing()
    {
        _nowTime += Time.deltaTime * _speed;
        if(_nowTime > _flashTime)
        {
            _nowTime = 0;
            _alpha = _alpha == 1 ? 0 : 1;
        }
    }

    public void FadeImage()
    {
        switch (_fadeType)
        {
            case FADE_TYPE.IN:
                FadeIn();
                break;
            case FADE_TYPE.OUT:
                FadeOut();
                break;
            case FADE_TYPE.FLASH:
                Flashing();
                break;
            case FADE_TYPE.MAX:
            default:
                break;
        }
    }

    private void SetAlpha()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _alpha);
    }

    // Update is called once per frame
    void Update()
    {
        FadeImage();
        SetAlpha();
    }
}
