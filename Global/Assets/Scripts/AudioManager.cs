using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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

    private AudioSource _audioSource;
    private Coroutine _coroutine;
    // 最後に再生した音
    private AudioClip _clip;

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

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _clip = _audioSource.clip;
        PlayBGM(_clip);
    }

    public void PlaySE(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(SoundLoop(clip));
        }
    }

    public void StopBGM()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _audioSource.Stop();
            _coroutine = null;
        }
    }

    IEnumerator SoundLoop(AudioClip clip)
    {
        while(true)
        {
            if(_audioSource.isPlaying == false)
            {
                Debug.Log("再生されていないので再生を行います");
                _audioSource.PlayOneShot(clip);
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            PlaySE(_clip);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            PlayBGM(_clip);
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            StopBGM();
        }
    }
}
