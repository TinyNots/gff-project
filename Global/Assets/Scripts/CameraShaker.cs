using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Vector3 _amount = new Vector3(1.0f, 1.0f, 0.0f);
    private float _duration = 1.0f;
    private float _speed = 10.0f;
    private AnimationCurve _curve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
    private bool _deltaMovement = true;

    private Camera  _camera;
    private float   _time = 0.0f;
    private Vector3 _oldPos;
    private Vector3 _newPos;
    private float   _oldFoV;
    private float   _newFoV;
    private bool    _destroyAfterPlay;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public static void ShakeOnce(float duration = 1.0f,float speed = 10.0f,Vector3? amount = null,Camera camera = null,bool deltaMovement = true,AnimationCurve curve = null)
    {
        CameraShaker instance = ((camera != null) ? camera : Camera.main).gameObject.AddComponent<CameraShaker>();
        instance._duration = duration;
        instance._speed = speed;
        if(amount != null)
        {
            instance._amount = (Vector3)amount;
        }
        if(curve != null)
        {
            instance._curve = curve;
        }
        instance._deltaMovement = deltaMovement;

        instance._destroyAfterPlay = true;
        instance.Shake();
    }

    private void Shake()
    {
        ResetCamera();
        _time = _duration;
    }

    private void LateUpdate()
    {
        if(_time > 0)
        {
            _time -= Time.deltaTime;
            if(_time > 0)
            {
                //next position based on perlin noise
                _newPos = (Mathf.PerlinNoise(_time * _speed, _time * _speed * 2) - 0.5f) * _amount.x * transform.right * _curve.Evaluate(1f - _time / _duration) +
                          (Mathf.PerlinNoise(_time * _speed * 2, _time * _speed) - 0.5f) * _amount.y * transform.up * _curve.Evaluate(1f - _time / _duration);
                _newFoV = (Mathf.PerlinNoise(_time * _speed * 2, _time * _speed * 2) - 0.5f) * _amount.z * _curve.Evaluate(1f - _time / _duration);

                _camera.fieldOfView += (_newFoV - _oldFoV);
                _camera.transform.Translate(_deltaMovement ? (_newPos - _oldPos) : _newPos);

                _oldPos = _newPos;
                _oldFoV = _newFoV;
            }
            else
            {
                ResetCamera();
                if (_destroyAfterPlay)
                {
                    Destroy(this);
                }
            }
        }
    }

    private void ResetCamera()
    {
        _camera.transform.Translate(_deltaMovement ? -_oldPos : Vector3.zero);
        _camera.fieldOfView -= _oldFoV;

        _oldPos = _newPos = Vector3.zero;
        _oldFoV = _newFoV = 0f;
    }
}
