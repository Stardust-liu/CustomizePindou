using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

[Serializable]
public class Click : UnityEvent { }
[Serializable]
public class Down : UnityEvent { }
[Serializable]

[AddComponentMenu("Event/ButtonEventBase")]
public class ButtonEventBase : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public ClickSpecies clickSpecies;
    public float zoomRatio = 0.9f;
    public float zoomTime = 0.2f;
    Vector2 startScale;
    RectTransform rectTransform;
    public enum ClickSpecies
    {
        zoomm,
        nothing,
    }
    public Click onClickEvent;
    public Down onDownEvent;

    void Start()
    {
        rectTransform = (this.transform as RectTransform);
        startScale = rectTransform.localScale;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (onClickEvent != null) onClickEvent.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (clickSpecies == ClickSpecies.zoomm)
        {
            this.rectTransform.DOScale(new Vector2(startScale.x*zoomRatio, startScale.y* zoomRatio), zoomTime);
        }
        if (onDownEvent != null) onDownEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (clickSpecies == ClickSpecies.zoomm)
        {
            this.rectTransform.DOScale(startScale, zoomTime);
        }
    }
}
