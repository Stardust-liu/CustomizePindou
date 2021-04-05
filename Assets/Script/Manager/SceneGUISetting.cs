using System;
using System.Collections;
using UnityEngine;

public class SceneGUISetting : ScriptableObject
{
    private static SceneGUISetting instance;
    public static SceneGUISetting Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<SceneGUISetting>("SceneGUISetting");

            if (instance == null)
                Debug.LogError("未创建SceneGUISetting");

            return instance;
        }
    }
    [Header("场景UI预制体路径")]
    public ScreenCfg[] screenCfgs;
    [System.Serializable]
    public class ScreenCfg
    {
        public SceneGUIName sceneName;
        public string resourcePath;
    }
}
