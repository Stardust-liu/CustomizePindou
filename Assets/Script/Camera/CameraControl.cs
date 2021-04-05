using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : BaseObject
{
    public Transform mainCamera;
    public Transform target;
    Vector2 offset;
    Vector2 startPoint;
    Vector3 currentCameraPoint;

    public float minHeight;
    public static float maxX, maxY;
    
    float maxHeight;
    float TargetDistance;
    float zoomOffsetX, zoomOffsetY;
    float minEdgeX,maxEdgeX, edgeY;
    float zoomLerp;

    float fovWidth,fovHeight;
    float cameraFov;

    protected override void OnAwake()
    {
        zoomLerp = 1;
        cameraFov = (Camera.main.fieldOfView * 0.5f) * Mathf.Deg2Rad;
    }

    protected override void OnStart()
    {
        GetMaxHeight();
        EdgePointOffset();
    }

    void Update()
    {
        Move();
        Zoom();
        ZoomLerp();
    }

    void Move()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    isEnterUI = true;
        //    return;
        //}
        //else
        //{
        //    if(isEnterUI)
        //        startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(mainCamera.position.z)));
        //    isEnterUI = false;
        //}


        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(mainCamera.position.z)));
        }
        if (Input.GetMouseButton(0))
        {
            offset = mainCamera.position - Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(mainCamera.position.z)));

            float x = Mathf.Clamp(startPoint.x + offset.x, minEdgeX, maxEdgeX);
            float y = Mathf.Clamp(startPoint.y + offset.y, -edgeY, edgeY);

            mainCamera.position = new Vector3(x,y, mainCamera.position.z);
            CheckEdge(x,y);
        }
    }

    void CheckEdge(float x, float y)
    {
        bool isEdge = false;
        if (x == minEdgeX)
            isEdge = true;
        else if (x == maxEdgeX)
            isEdge = true;

        if (y == -edgeY)
            isEdge = true;
        else if (y == edgeY)
            isEdge = true;

        if (isEdge)
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(mainCamera.position.z)));
    }

    void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                TargetDistance = Mathf.Clamp(TargetDistance + 2, maxHeight, minHeight);
            else
                TargetDistance = Mathf.Clamp(TargetDistance - 2, maxHeight, minHeight); ;

            Vector3 currentMousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(mainCamera.position.z)));
            Vector3 forecastMousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(TargetDistance)));

            zoomOffsetX = mainCamera.position.x + (currentMousePoint.x - forecastMousePoint.x);
            zoomOffsetY = mainCamera.position.y + (currentMousePoint.y - forecastMousePoint.y);
            EdgePointOffset();
            zoomOffsetX = Mathf.Clamp(zoomOffsetX, minEdgeX, maxEdgeX);
            zoomOffsetY = Mathf.Clamp(zoomOffsetY, -edgeY, edgeY);
            currentCameraPoint = mainCamera.position;
            zoomLerp = 0;
        }
    }

    void ZoomLerp()
    {
        if (zoomLerp != 1)
        {
            zoomLerp = Mathf.Clamp(zoomLerp + Time.deltaTime*5, 0f, 1f);
            var temporaryPoint = Vector3.Lerp(currentCameraPoint, new Vector3(zoomOffsetX, zoomOffsetY, TargetDistance), zoomLerp);
            mainCamera.position = temporaryPoint;
        }
    }

    /// <summary>
    /// 边缘偏移量
    /// </summary>
    void EdgePointOffset()
    {
        fovHeight =  Mathf.Abs(TargetDistance) * Mathf.Tan(cameraFov);
        fovWidth = fovHeight * Camera.main.aspect;
        edgeY = maxY - fovHeight;
        //由于左边有ui面板遮挡 所以 最左边的边缘要 + 偏移量 - UI面板所占屏幕的长度, 最右边的边缘要 + 偏移量

        minEdgeX = -maxX + fovWidth + (MenuPanel.instance.descPanel.rect.width / 100f) - (fovWidth / MenuPanel.instance.descPanelProportion);
        maxEdgeX = maxX - fovWidth + MenuPanel.instance.descPanel.rect.width / 100f;
    }

    /// <summary>
    /// 获取最远高度
    /// </summary>
    void GetMaxHeight()
    {
        float maxZoomHeight = maxY / Mathf.Tan(cameraFov);
        float maxZoomWidth = maxX / Camera.main.aspect / Mathf.Tan(cameraFov);
        maxHeight = -Mathf.Min(maxZoomHeight, maxZoomWidth);
        mainCamera.position = new Vector3(mainCamera.position.x, mainCamera.position.y, maxHeight);
        TargetDistance = mainCamera.position.z;
    }
}
