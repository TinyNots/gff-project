using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPoint : MonoBehaviour
{
    private GameObject playerText;
    private GameObject player;
    public int playerNo = 1;
    private GameObject prefab = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            var tmpString = "Player " + playerNo;
            player = GameObject.Find(tmpString);
            if (player != null)
            {
                playerText.GetComponent<Text>().text = "P" + playerNo;
            }
        }
        gameObject.transform.position = Camera.main.WorldToScreenPoint(new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z));
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0));
    }
}
