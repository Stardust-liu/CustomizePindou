using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public struct HSV
{
    //public int H;
    //public int S;
    //public int V; 
    public float H;
    public float S;
    public float V;
}
public class Tool : BaseInstance<Tool>
{
    public Camera mainCamera;

public Tool()
    {
        mainCamera = Camera.main;
    }
    /// <summary>
    /// 数字转字母
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
   public string NumberToLetter(int number)
   {
        var letter = "";
        if (number < 26)
        {
            byte[] numberb = new byte[] { (byte)(number+65) };
            letter = Encoding.ASCII.GetString(numberb);
        }
        else
            Debug.Log("数字不在转换范围内");
        return letter;
   }

    /// <summary>
    /// 世界坐标转屏幕坐标
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 WorldToScreenPos(Vector3 point)
    {
        return mainCamera.WorldToScreenPoint(point);
    }
    /// <summary>
    /// 屏幕坐标转世界坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 ScreenToWorldPos(Vector3 point)
    {
        return mainCamera.ScreenToWorldPoint(point);
    }

    public static HSV RGBTOHSV(Color color)
    {
        float  h, s, v;
        HSV hsv = new HSV();
        Color.RGBToHSV(color, out h, out s, out v);

        //hsv.H = Mathf.FloorToInt(h*360);
        //hsv.S = Mathf.FloorToInt(s*100);
        //hsv.V = Mathf.FloorToInt(v*100);
        hsv.H = h*360;
        hsv.S = s*100;
        hsv.V = v*100;

        return hsv;
    }
}
