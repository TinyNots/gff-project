using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomTitle : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _spriteList;
    private SpriteRenderer _sprite;
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        SpriteChange();
    }

    private void SpriteChange()
    {
        int ID = Random.Range(0, _spriteList.Count);
        _sprite.sprite = _spriteList[ID];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
