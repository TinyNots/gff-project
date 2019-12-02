using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField]
    private SceneCtl.SCENE_ID _sceneID;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(SceneCtl.instance != null)
            {
                SceneCtl.instance.NextScene(_sceneID);
            }
        }
    }
}
