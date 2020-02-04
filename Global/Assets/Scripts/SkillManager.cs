using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private List<Transform> _clones;
    private Vector3 _centerPoint;
    private List<float> _radians;
    private bool _trigger;
    private bool _wait;

    [Header("General")]
    [SerializeField]
    private float _speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        _clones = new List<Transform>();
        _centerPoint = Vector3.zero;
        _trigger = false;
        _radians = new List<float>();
        _wait = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_trigger)
        {
            int counter = 0;
            foreach (Transform clone in _clones)
            {
                if (!clone.Find("Sprite").GetComponent<Renderer>().isVisible)
                {
                    counter++;
                }
            }

            if (counter == _clones.Count && !_wait)
            {
                ClearClones();
                StartCoroutine("Wait");
            }
        }
    }

    private void FixedUpdate()
    {
        if(_trigger)
        {
            for (int i = 0; i < _clones.Count; i++)
            {
                Vector2 movement = new Vector2(Mathf.Cos(_radians[i]), Mathf.Sin(_radians[i]));
                _clones[i].Translate(movement * _speed * Time.deltaTime);
            }
        }
    }

    public List<Transform> GetClones()
    {
        return _clones;
    }

    public void ClearClones()
    {
        foreach (Transform clone in _clones)
        {
            Destroy(clone.gameObject);
        }
        _clones.Clear();
        _centerPoint = Vector3.zero;
        _radians.Clear();
    }

    public void StartSkill()
    {
        if(!_trigger)
        {
            if (_clones.Count < 2)
            {
                return;
            }

            SoundManager.Instance.PlaySe("Evil Attack 04");

            _trigger = true;

            Vector3 centerPoint = Vector3.zero;
            foreach (Transform clone in _clones)
            {
                centerPoint += clone.position;
            }
            centerPoint /= _clones.Count;
            _centerPoint = centerPoint;

            foreach (Transform clone in _clones)
            {
                float radian = Mathf.Atan2(_centerPoint.y - clone.position.y, _centerPoint.x - clone.position.x);

                Vector2 movement = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
                Transform cloneSprite = clone.Find("Sprite");
                if (movement.x < 0)
                {
                    cloneSprite.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                }
                else if(movement.x > 0)
                {
                    cloneSprite.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                }

                _radians.Add(radian);

                cloneSprite.GetComponent<Animator>().SetBool("Attack", true);
            }
        }
    }

    public bool GetTrigger()
    {
        return _trigger;
    }

    IEnumerator Wait()
    {
        _wait = true;
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Fail");
        _trigger = false;
        _wait = false;
    }
}
