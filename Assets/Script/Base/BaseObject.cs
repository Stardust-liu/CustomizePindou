using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    void Awake()
    {
        OnReadyAwake();
        OnAwake();
    }
    void Start()
    {
        OnReadyStart();
        OnStart();
    }

    protected virtual void OnReadyAwake() { }
    protected virtual void OnAwake() { }
    protected virtual void OnReadyStart() { }
    protected virtual void OnStart() { }}
