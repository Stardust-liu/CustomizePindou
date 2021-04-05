using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MenuPanel : BaseScene
{
    public static MenuPanel instance;
    public LodingAnim lodingAnim;
    public RectTransform colorListContent;
    public RectTransform descPanel;
    public RectTransform thumbnailArea;
    public RectTransform thumbnailPanel;

    public RawImage referencePicture;
    public Image menuPanelMask;
    public TextMeshProUGUI lodingText;
    public Text pixelDesc;
    public Text makeAreaText;
    public Toggle showColor;
    string[] AreaName;
    Vector2Int[] AreaNumber;
    int currentAreaIndex;
    [HideInInspector]
    public float descPanelProportion;
    float thumbnailImageRatio;

    protected override void OnReadyAwake()
    {
        instance = this;
    }

    protected override void OnStart()
    {
        AdapterTexture(referencePicture.rectTransform);
        referencePicture.texture = MainScene.instance.testPicture;
        UpdateThumbnailEara(new Vector2Int(0, 0));
    }
    protected override void ShowInitialize()
    {
        Debug.Log("场景初始化");
        currentAreaIndex = 0;
        makeAreaText.text = "A1";
        descPanelProportion = (Screen.width / descPanel.rect.width)/2;
    }
    public void PixelDesc(string Desc)
    {
        pixelDesc.text = Desc;
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return null;
            colorListContent.sizeDelta = new Vector2(colorListContent.rect.width, pixelDesc.GetComponent<RectTransform>().rect.height);
        }
    }

    public void ChangeShowColor()
    {
        MainScene.instance.ChangeShowColor(showColor.isOn);
    }

    public void CareatAreaNumber()
    {
        int areaXCount = Mathf.CeilToInt(MainScene.instance.pictureWidth / (float)MainScene.pixelCount);
        int areaYCount = Mathf.CeilToInt(MainScene.instance.pictureHeigth / (float)MainScene.pixelCount);
        AreaName = new string[areaXCount * areaYCount];
        AreaNumber = new Vector2Int[areaXCount * areaYCount];
        for (int y = 0, i = 0; y < areaYCount; y++)
        {
            for (int x = 0; x < areaXCount; x++,i++)
            {
                AreaName[i] = string.Format("{0}{1}",Tool.Instance.NumberToLetter(y) , (x+1).ToString());
                AreaNumber[i] = new Vector2Int(x,y);
            }
        }
    }
    public void NextMakeArea()
    {
        currentAreaIndex++;
        currentAreaIndex = currentAreaIndex % AreaName.Length;
        UpdateMakeArea(currentAreaIndex);
    }
    public void LastMakeArea()
    {
        if (currentAreaIndex > 0)
            currentAreaIndex--;
        else
            currentAreaIndex = AreaName.Length - 1;

        UpdateMakeArea(currentAreaIndex);
    }

    public void UpdateMakeArea(int areaIndex)
    {
        makeAreaText.text = AreaName[areaIndex];
        MainScene.instance.CareatPinDouDesc(AreaNumber[areaIndex]);
        UpdateThumbnailEara(AreaNumber[areaIndex]);
    }

    public void AdapterTexture(RectTransform rectTransform)
    {
        if (MainScene.instance.pictureWidth > MainScene.instance.pictureHeigth)
        {
            thumbnailImageRatio = thumbnailPanel.rect.width / MainScene.instance.pictureWidth;
            rectTransform.sizeDelta = new Vector2(thumbnailPanel.rect.width, thumbnailImageRatio * MainScene.instance.pictureHeigth);
        }
        else
        {
            thumbnailImageRatio = thumbnailPanel.rect.height / MainScene.instance.pictureHeigth;
            rectTransform.sizeDelta = new Vector2(thumbnailImageRatio * MainScene.instance.pictureWidth, thumbnailPanel.rect.height);
        }
    }

    public void UpdateThumbnailEara(Vector2Int eara)
    {
        float width = Mathf.Clamp(MainScene.instance.pictureWidth - eara.x * MainScene.pixelCount, 0, MainScene.pixelCount)* thumbnailImageRatio;
        float height = Mathf.Clamp(MainScene.instance.pictureHeigth - eara.y * MainScene.pixelCount, 0, MainScene.pixelCount)* thumbnailImageRatio;
        float x = eara.x * MainScene.pixelCount* thumbnailImageRatio;
        float y = eara.y * MainScene.pixelCount * thumbnailImageRatio;
        thumbnailArea.anchoredPosition = new Vector2(x,y);
        thumbnailArea.sizeDelta = new Vector2(width, height);
    }

    public void OpenLodingPanel()
    {
        menuPanelMask.raycastTarget = true;
        menuPanelMask.DOColor(new Color(0,0,0,0.5f),0.5f).OnComplete(()=> 
        {
            lodingText.DOColor(new Color(1, 1, 1, 1), 0.3f);
            lodingAnim.StartLoding();
        });
    }
}
