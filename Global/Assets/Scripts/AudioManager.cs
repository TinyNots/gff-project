using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    private AudioManager Instance
    {
        get
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
            return instance;
        }
    }

    [SerializeField]
    private AudioSource _audioSource = null;
    [SerializeField]
    private AudioClip _audioClip = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void PlaySE()
    {
        _audioSource.PlayOneShot(_audioClip);
    }

    private void PlayBGM()
    {

    }

    IEnumerator Sound()
    {
        while(true)
        {
            if(_audioSource.isPlaying == false)
            {
                _audioSource.PlayOneShot(_audioClip);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
