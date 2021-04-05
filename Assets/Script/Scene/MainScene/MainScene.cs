using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseObject
{
    public static MainScene instance;
    public Texture2D testPicture;
    public SpriteRenderer pixelRenderer;
    public SpriteRenderer pindouRenderer;
    public Transform PindouObjectParent;
    public Transform serialNumberParent;
    public GameObject serialNumberPrefab;
    [HideInInspector]
    public int magnification = 5;
    [HideInInspector]
    public const int pixelCount = 20;
    [HideInInspector]
    public int pictureWidth;
    [HideInInspector]
    public int pictureHeigth;

    Vector2 border;
    Vector3 Pixelscale;
    Color[] PindouColorArray;
    string[] pindpuNumberArray;

 
    protected override void OnReadyAwake()
    {
        instance = this;
    }
    protected override void OnAwake()
    {
        pictureWidth = testPicture.width;
        pictureHeigth = testPicture.height;
        PindouColorArray = new Color[testPicture.GetPixels().Length];
        pindpuNumberArray = new string[testPicture.GetPixels().Length];
        border = new Vector2(-pictureWidth * magnification / 2f / 100f, -pictureHeigth * magnification / 2f / 100f); //边界位置为生成图片的左下角
        Pixelscale = new Vector3(magnification / 100f, magnification / 100f, 1);//每个像素块的大小为 放大像素倍数 / 100
        CameraControl.maxX = pictureWidth * magnification / 100f / 2f;
        CameraControl.maxY = pictureHeigth * magnification / 100f / 2f;
    }

    protected override void OnStart()
    {
        PindouObjectParent.position = new Vector3(MenuPanel.instance.descPanel.rect.width/100,0,0);
        TargetLogic.instance.distance = (magnification / 100f);
        TargetLogic.instance.target.localScale = Pixelscale;
        TargetLogic.instance.targetScale = Pixelscale.x;

        GetPixelsDesc();
        CareatSprite(testPicture.GetPixels(), pixelRenderer);
        CareatPinDouDesc(new Vector2Int(0,0));

        MenuPanel.instance.CareatAreaNumber();
    }

    void GetPixelsDesc()
    {
        Dictionary<string, int> pindouDictonary = new Dictionary<string, int>();
        string pindouTypeCount = "";
      
        for (int i = 0; i < testPicture.GetPixels().Length; i++)
        {
            var nearColorDesc =  ColorManager.instance.GetNearColor(testPicture.GetPixels()[i]);
            PindouColorArray[i] = nearColorDesc.color;
            pindpuNumberArray[i] = nearColorDesc.colorNumber;
            if (!pindouDictonary.ContainsKey(nearColorDesc.colorNumber))
            {
                pindouDictonary.Add(nearColorDesc.colorNumber, 0);
                pindouDictonary[nearColorDesc.colorNumber]++;
            }
            else
            {
                pindouDictonary[nearColorDesc.colorNumber]++;
            }
        }
        foreach (var item in pindouDictonary)
        {
            pindouTypeCount += string.Format("{0}           ×{1}\r\n", item.Key, item.Value.ToString());
            MenuPanel.instance.PixelDesc(pindouTypeCount);
        }
    }


    public void ChangeShowColor(bool isShowPinDouColor)
    {
        if (isShowPinDouColor)
        {
            if (pindouRenderer.sprite == null)
                CareatSprite(PindouColorArray, pindouRenderer);
            else
            {
                pixelRenderer.gameObject.SetActive(false);
                pindouRenderer.gameObject.SetActive(true) ;
            }
        }
        else
        {
            pindouRenderer.gameObject.SetActive(false);
            pixelRenderer.gameObject.SetActive(true);
        }
    }

    public void CareatPinDouDesc(Vector2Int area)
    {
        GameObjectPool.Instance.HidePool(GameObjectPool.Pool.Pixel);
        bool isUpdateArea = false;
        for (int y = 0; y < pixelCount; y++)
        {
            for (int x = 0; x < pixelCount; x++)
            {
                if (area.x * pixelCount + x < pictureWidth &&
                    area.y * pixelCount + y < pictureHeigth)
                {
                    //偏移坐标为: 边界位置 + 放大后像素块间距 * 所在行列数 + 像素间距的一半 + 所在区域偏移量
                    var offsetX = border.x + (magnification / 100f) * (x) + (magnification / 100f / 2f) + (area.x * magnification * pixelCount) / 100f;
                    var offsetY = border.y + (magnification / 100f) * (y) + (magnification / 100f / 2f) + (area.y * magnification * pixelCount) / 100f;

                    //对应颜色的下标为 所在列数 + 所在区域列数 *  单个区域行数 + 所在行数 * 总列数 +所在区域行数 * 单个区域行数 * 总列数
                    int index = x + area.x * pixelCount + y * pictureWidth + area.y * pixelCount * pictureWidth;
                    var colorDesc = pindpuNumberArray[index];
                    GameObject pindouDesc = GameObjectPool.Instance.GetPool(GameObjectPool.Pool.Pixel);

                    if (pindouDesc == null)
                    {
                        pindouDesc = Instantiate(serialNumberPrefab, serialNumberParent);
                        GameObjectPool.Instance.AddPool(GameObjectPool.Pool.Pixel, pindouDesc);
                    }
                    else
                    {
                        pindouDesc.transform.parent = serialNumberParent;
                    }
                    pindouDesc.transform.localScale = Pixelscale;
                    pindouDesc.transform.localPosition = new Vector2(offsetX, offsetY);
                    pindouDesc.GetComponent<PixelDesc>().colorNumberText.text = colorDesc;
                  
                    pindouDesc.SetActive(true);

                    if (!isUpdateArea)
                    {
                        TargetLogic.instance.UpdateArea(area, new Vector2(offsetX - (magnification / 100f / 2f), offsetY - (magnification / 100f / 2f)));
                        isUpdateArea = true;
                    }
                }
            }
        }
    }
    void CareatSprite(Color[] colorArray,SpriteRenderer showRenderer)
    {
        Texture2D PictureTexure = new Texture2D(pictureWidth * magnification, pictureHeigth * magnification);
      
        for (int y = 0 ; y < pictureHeigth * magnification; y++)
        {
            for (int x = 0; x < pictureWidth * magnification; x++)
            {
                int colorIndex = Mathf.CeilToInt(x / magnification) + pictureWidth * Mathf.CeilToInt(y / magnification);
                PictureTexure.SetPixel(x, y, colorArray[colorIndex]);
            }
        }
        PictureTexure.Apply();

        Sprite pixelSprite = Sprite.Create(PictureTexure, new Rect(0, 0, pictureWidth * magnification, pictureHeigth * magnification), new Vector2(0.5f, 0.5f));
        showRenderer.sprite = pixelSprite;
    }
}
