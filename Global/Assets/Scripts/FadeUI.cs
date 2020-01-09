using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    enum IMAGE_TYPE
    {
        SPRITE,
        IMAGE,
        TEXT,
        MAX
    }
    enum FADE_TYPE
    {
        IN,
        OUT,
        FLASH,
        MAX
    }

    [SerializeField]
    private IMAGE_TYPE _type = IMAGE_TYPE.SPRITE;
    [SerializeField]
    private FADE_TYPE _fadeType;
    [SerializeField]
    private float _speed = 1;
    [SerializeField, Tooltip("ループフラグ")]
    private bool _loopFlag = false;
    [SerializeField, Tooltip("フェイドの有効化フラグ")]
    private bool _active = false;
    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }
    private Image _image;
    private Text _text;
    private Color _color;
    // 現在の透明度
    private float _alpha;
    private float _nowTime;
    private const int _flashTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        _alpha = 1;
        switch (_type)
        {
            case IMAGE_TYPE.SPRITE:
                break;
            case IMAGE_TYPE.IMAGE:
                _image = GetComponent<Image>();
                _color = _image.color;
                break;
            case IMAGE_TYPE.TEXT:
                _text = GetComponent<Text>();
                _color = _text.color;
                break;
            case IMAGE_TYPE.MAX:
            default:
                break;
        }
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
        // 有効化されてないなら処理をしない
        if(!_active)
        {
            return;
        }
        // それぞれのフェイドのタイプ
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
        switch (_type)
        {
            case IMAGE_TYPE.SPRITE:
                break;
            case IMAGE_TYPE.IMAGE:
                _image.color = new Color(_color.r, _color.g, _color.b, _alpha);
                break;
            case IMAGE_TYPE.TEXT:
                _text.color = new Color(_color.r, _color.g, _color.b, _alpha);
                break;
            case IMAGE_TYPE.MAX:
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FadeImage();
        SetAlpha();
    }
}