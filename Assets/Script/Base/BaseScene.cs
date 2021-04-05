using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : BaseObject
{
    /// <summary>
    /// 面板初始化
    /// </summary>
    public void Initialize()
    {
        ShowInitialize();
    }
  
    protected virtual void ShowInitialize() { }
}
