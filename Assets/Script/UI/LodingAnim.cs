using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LodingAnim : MonoBehaviour
{
    public Image[] loding;

    void Awake()
    {
        Hide();
    }
    public void StartLoding()
    {
        Hide();
        StartCoroutine(Loding());
    }

    IEnumerator Loding(bool isWait = false)
    {
        if(isWait)
            yield return new WaitForSeconds(0.1f);
        int i = 0;
        while (i < loding.Length)
        {
            ChangeColor(i);
            i++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ChangeColor(int index)
    {
        loding[index].DOColor(new Color(1,1,1,1),0.3f).SetEase(Ease.InOutQuad).OnComplete(()=> 
        {
            loding[index].DOColor(new Color(1, 1, 1, 0), 0.3f).SetEase(Ease.InOutQuad);
        });
        if (index == loding.Length - 1)
            StartCoroutine(Loding(true));
    }

    public void Hide()
    {
        for (int i = 0; i < loding.Length; i++)
        {
            loding[i].color = new Color(1, 1, 1, 0);
        }
    }
}
