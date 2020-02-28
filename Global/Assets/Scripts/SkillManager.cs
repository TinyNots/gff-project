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
                // もし残像がカメラ外に出たら、カウンターが増やす
                if (!clone.Find("Sprite").GetComponent<Renderer>().isVisible)
                {
                    counter++;
                }
            }

            // カウンターが残像数と一致したら、残像をけしてクールダウンを開始する
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
            // 現在の残像位置は移動量と位と足す
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
        // 全部の残像をけして、リストと変数を初期化する
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
            // 残像が２個未満だったら、発動しない
            if (_clones.Count < 2)
            {
                return;
            }

            SoundManager.Instance.PlaySe("Evil Attack 04");

            _trigger = true;

            // 中心点を計算するために、全残像の位置をたして残像の数を割る
            Vector3 centerPoint = Vector3.zero;
            foreach (Transform clone in _clones)
            {
                centerPoint += clone.position;
            }
            centerPoint /= _clones.Count;
            _centerPoint = centerPoint;

            // 残像と中心点の角度を計算する
            foreach (Transform clone in _clones)
            {
                float radian = Mathf.Atan2(_centerPoint.y - clone.position.y, _centerPoint.x - clone.position.x);

                Vector2 movement = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

                // 残像を中心点の方向に向く
                Transform cloneSprite = clone.Find("Sprite");
                if (movement.x < 0)
                {
                    cloneSprite.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                }
                else if(movement.x > 0)
                {
                    cloneSprite.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                }

                // 計算したラジアンはラジアン専用のリストに格納する
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
