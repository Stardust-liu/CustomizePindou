using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAsset : Editor
{
    [MenuItem("EasyTool/CreateAsset")]
    static void CarearAsset()
    {
        ScriptableObject sceneSetting = ScriptableObject.CreateInstance<SceneGUISetting>();

        if (!sceneSetting)
            return;

        string path = Application.dataPath + "/BulletAeeet";

        // 如果项目总不包含该路径，创建一个
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //拼接保存自定义资源（.asset） 路径
        path = string.Format("Assets/Resources/{0}.asset", (typeof(SceneGUISetting).ToString()));

        // 生成自定义资源到指定路径
        AssetDatabase.CreateAsset(sceneSetting, path);
    }
}

