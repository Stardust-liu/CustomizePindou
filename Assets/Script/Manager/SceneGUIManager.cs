using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGUIManager : BaseObject
{
    public static SceneGUIManager instance;
    BaseScene currentPanel;
    BaseScene lastPanel;
    SceneGUIName currentGUIScene;
    Dictionary<SceneGUIName, BaseScene> saveGUIScene = new Dictionary<SceneGUIName, BaseScene>();
    const string generalPath = "Prefab/SceneGUI";
    protected override void OnReadyAwake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// 获取UI预制体路径
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    string GetScenePath(SceneGUIName sceneName)
    {
        return string.Format("{0}/{1}", generalPath, sceneName.ToString()); ;
    }

    /// <summary>
    /// 切换场景UI
    /// </summary>
    public void ChangeSceneUI(SceneGUIName sceneGUIName)
    {
        if(currentGUIScene != sceneGUIName)
        {
            currentGUIScene = sceneGUIName;
        }
        if(sceneGUIName != SceneGUIName.None)
        {
            Show(sceneGUIName);
        }
    }

    /// <summary>
    /// 显示
    /// </summary>
    void Show(SceneGUIName sceneGUIName)
    {
        if(lastPanel != null)
        {
            if (lastPanel == currentPanel)
            {
                Debug.Log("打开了同一个面板");
                return;
            }
        }
        if (!saveGUIScene.ContainsKey(sceneGUIName))
        {
            var prefab = Resources.Load<GameObject>(GetScenePath(sceneGUIName));
            if (prefab != null)
            {
                var item = GameObject.Instantiate(prefab, MainManager.instance.sceneLayer);
                var itemScript = item.GetComponent<BaseScene>();

                if (itemScript != null)
                {
                    saveGUIScene.Add(sceneGUIName, itemScript);
                    ChangePanel();
                }
                else
                {
                    Debug.Log("该场景脚本未继承BaseScene");
                }
            }
            else 
            {
                Debug.Log(string.Format("未找到预制体 {0}", GetScenePath(sceneGUIName)));
            }
        }
        else
        {
            ChangePanel();
        }

        void ChangePanel()
        {
            if (currentPanel != null)
            {
                lastPanel = currentPanel;
                lastPanel.gameObject.SetActive(false);
            }
            saveGUIScene[sceneGUIName].Initialize();
            currentPanel = saveGUIScene[sceneGUIName];
        }
    }
}
