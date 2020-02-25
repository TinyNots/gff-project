using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flasher : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Shader _shaderGUItext;
    private Shader _shaderSpritesDafult;

    // 初期化
    private void Start()
    {
        _renderer = transform.GetComponent<SpriteRenderer>();
        _shaderGUItext = Shader.Find("GUI/Text Shader");
        _shaderSpritesDafult = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
    }

    // スプライトを白くなる
    public void StartFlash(float time = 0.01f)
    {
        _renderer.material.shader = _shaderGUItext;
        _renderer.color = Color.white;
        StartCoroutine(FlashSprite(time));
    }

    private IEnumerator FlashSprite(float time)
    {
        while (Time.timeScale != 1.0f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(time);
        _renderer.material.shader = _shaderSpritesDafult;
        _renderer.color = Color.white;
    }
}
