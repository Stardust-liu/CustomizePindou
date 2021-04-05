using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : BaseObject
{
    public static MainManager instance;
    public Transform sceneLayer;

    protected override void OnReadyAwake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }

    protected override void OnAwake()
    {
        LoadMenuPanel();
    }

    void LoadMenuPanel()
    {
        SceneGUIManager.instance.ChangeSceneUI(SceneGUIName.MenuPanel);
    }
}
