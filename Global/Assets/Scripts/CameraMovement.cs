using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private List<GameObject> _players;
    float playerCnt = 1;

    // Start is called before the first frame update
    void Start()
    {
        _players = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCnt < 5)
        {
            var tmpString = "Player " + playerCnt;
            var tmpPlayer = GameObject.Find(tmpString);
            if (tmpPlayer != null)
            {
                _players.Add(tmpPlayer);
                playerCnt++;
            }
        }

     
    }

    private void FixedUpdate()
    {
        if (AveragePos().x >= 2.0f)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0.9f, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z), Time.deltaTime);
        }
        else if (AveragePos().x <= -2.0f)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(-0.9f, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z), Time.deltaTime);

        }
        if (AveragePos().y >= 1f)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(Camera.main.transform.localPosition.x, 0.5f, Camera.main.transform.localPosition.z), Time.deltaTime);
        }
        else if (AveragePos().y <= -3f)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(Camera.main.transform.localPosition.x, -0.5f, Camera.main.transform.localPosition.z), Time.deltaTime);

        }
    }

        Vector2 AveragePos()
    {
        Vector2 tmpPos = new Vector2(0,0);
        foreach (GameObject player in _players)
        {
            if (!player.GetComponent<Character>().IsDie)
            {
                tmpPos += new Vector2(player.transform.position.x, player.transform.position.y);
            }
        }
        tmpPos /= _players.Count;
        return tmpPos;
    }
}
