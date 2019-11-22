using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButton : MonoBehaviour
{
    [SerializeField]
    private SceneCtl.SCENE_ID _sceneID;
    public void ChangeScene()
    {
        if(SceneCtl.instance != null)
        {
            SceneCtl.instance.NextScene(_sceneID);
        }
    }
}
