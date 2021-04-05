using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{
    public delegate void EventHandle();
    public delegate void EventHandle<T>(T value);
    static Dictionary<string, Delegate> eventHandles;


    /// <summary>
    /// 发送事件
    /// </summary>
    public static void PostEvent(string eventName)
    {
        if (eventHandles == null)
            return;
        Delegate d;
        if (eventHandles.TryGetValue(eventName,out d))
        {
            if (d == null)
                return;
            EventHandle call = d as EventHandle;
            if (call != null)
            {
                call.Invoke();
            }
        }
    } 
    public static void PostEvent<T>(string eventName,T value1)
    {
        if (eventHandles == null)
            return;
        Delegate d;
        if (eventHandles.TryGetValue(eventName,out d))
        {
            if (d == null)
                return;
            EventHandle<T> call = d as EventHandle<T>;
            if (call != null)
            {
                call.Invoke(value1);
            }
        }
    }

    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventHandle"></param>
    public static void AddListener(string eventName,EventHandle eventHandle)
    {
        if (eventHandles == null)
            eventHandles = new Dictionary<string, Delegate>();

        if (eventHandles.ContainsKey(eventName))
        {
            Debug.LogError(string.Format("事件名字重复{0}",eventName));
        }
        else
        {
            eventHandles.Add(eventName, eventHandle);
        }
    }
    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventHandle"></param>
    public static void AddListener<T>(string eventName, EventHandle<T> eventHandle)
    {
        if (eventHandles == null)
            eventHandles = new Dictionary<string, Delegate>();

        if (eventHandles.ContainsKey(eventName))
        {
            Debug.LogError(string.Format("事件名字重复{0}", eventName));
        }
        else
        {
            eventHandles.Add(eventName, eventHandle);
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventHandle"></param>
    public static void RemoveEvent(string eventName, EventHandle eventHandle)
    {
        if (eventHandles == null)
            return;
        if (eventHandles.ContainsKey(eventName))
        {
            Debug.LogError(string.Format("事件不存在{0}", eventName));
        }
        else
        {
            eventHandles[eventName] = (EventHandle)eventHandles[eventName] - eventHandle;
        }
    } 
    public static void RemoveEvent<T>(string eventName, EventHandle<T> eventHandle)
    {
        if (eventHandles == null)
            return;
        if (eventHandles.ContainsKey(eventName))
        {
            Debug.LogError(string.Format("事件不存在{0}", eventName));
        }
        else
        {
            eventHandles[eventName] = (EventHandle<T>)eventHandles[eventName] - eventHandle;
            eventHandles.Remove(eventName);
        }
    }
}
