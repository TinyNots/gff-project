using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField]
    private Transform _character;
    [SerializeField]
    private Transform _sprite;
    private bool waiting = false;

    [SerializeField]
    private Sprite _tester;

    private SpriteRenderer _sr;
    [SerializeField]
    private Transform _ghostsParent;
    [SerializeField]
    private Color _trailColor;
    [SerializeField]
    private Color _fadeColor;
    [SerializeField]
    private float ghostInterval = 0.2f;
    [SerializeField]
    private float _fadeTime;

    private void Start()
    {
        _sr = transform.GetComponent<SpriteRenderer>();
    }

    public void SpawnGhost()
    {
        for (int i = 0; i < _ghostsParent.childCount; i++)
        {
            while(waiting)
            {
                i -= 1;
                continue;
            }

            Transform currentGhost = _ghostsParent.GetChild(i);
            currentGhost.position = _character.position;
            currentGhost.rotation = _sprite.localRotation;
            currentGhost.GetComponent<SpriteRenderer>().sprite = _tester;
            currentGhost.GetComponent<SpriteRenderer>().material.color = _trailColor;
            // Start Fade;
            StartCoroutine(Wait(ghostInterval));
        }
    }

    private void Update()
    {
        
    }

    private IEnumerator Wait(float time)
    {
        waiting = true;
        yield return new WaitForSeconds(time);
        waiting = false;
    }

    private void FadeSprite(Transform current)
    {
        current.GetComponent<SpriteRenderer>().material.color = _fadeColor;
    }


}