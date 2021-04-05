using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLogic : BaseObject
{
    public static TargetLogic instance;
    public SpriteRenderer targetSprite;
    public Transform target;
    [HideInInspector]
    public float distance;
    [HideInInspector]
    public float targetScale;
    public int scope;

    Vector3 startpoint;
    Vector2Int area;
    int maxMoveHorizontal;
    int maxMoveVertical;
    int centerX;
    int centerY;
    protected override void OnReadyAwake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    void Update()
    {
        if((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && Input.anyKeyDown)
        {
            Move((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        }
    }

    public void UpdateArea(Vector2Int area, Vector2 point)
    {
        centerX = 0;
        centerY = 0;
        this.area = area;
        target.localPosition = startpoint = point;
        targetSprite.size = new Vector2Int(scope, scope);
        maxMoveHorizontal = Mathf.CeilToInt(Mathf.Min(MainScene.pixelCount, MainScene.instance.pictureWidth - area.x * MainScene.pixelCount)/ (float)scope)-1;
        maxMoveVertical = Mathf.CeilToInt(Mathf.Min(MainScene.pixelCount, MainScene.instance.pictureHeigth - area.y * MainScene.pixelCount)/ (float)scope)-1;
    }

    public void Move(int horizontal, int vertical)
    {
        centerX = Mathf.Clamp(centerX + horizontal, 0, maxMoveHorizontal);
        centerY = Mathf.Clamp(centerY + vertical, 0, maxMoveVertical);
        
        int offsetX = Mathf.Min(scope, (MainScene.instance.pictureWidth - area.x * MainScene.pixelCount)- centerX * scope);
        int offsetY = Mathf.Min(scope, (MainScene.instance.pictureHeigth - area.y * MainScene.pixelCount) - centerY * scope);
        targetSprite.size = new Vector2Int(offsetX, offsetY);
        target.localPosition = new Vector3(centerX * targetScale * scope, centerY * targetScale * scope, 0) + startpoint; 
    }
}
