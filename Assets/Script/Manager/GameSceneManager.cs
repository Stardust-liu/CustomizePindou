using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : BaseObject
{
    public static GameSceneManager instance;
    protected override void OnReadyAwake()
    {
        if(instance == null)
        {
            instance = this;
        }
        SceneManager.sceneLoaded += LoadSceneEnd;
    }

    private void OnEnable()
    {
        EventCenter.AddListener(EventName.LoadLevelEndEvent, (string sceneName)=> 
        {
            Debug.Log(string.Format("进入到{0}场景", sceneName));
        });
    }
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        LoadSceneBegin(sceneName);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
    /// <summary>
    /// 开始加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneBegin(string sceneName)
    {
        Debug.Log(string.Format("开始加载{0}场景", sceneName));
    }
    /// <summary>
    /// 加载场景结束
    /// </summary>
    public void LoadSceneEnd(Scene scene, LoadSceneMode loadMode)
    {
        EventCenter.PostEvent(EventName.LoadLevelEndEvent, scene.name);
    }
}
