
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameUtil
{
    public const int ROW_TOTAL = 17;
    public const int COL_TOTAL = 11;
    public int rowTotal
    {
        get
        {
            int ret = Mathf.Max(ROW_TOTAL, COL_TOTAL);
            if (Device.isLandscape)
            {
                ret = Mathf.Min(ROW_TOTAL, COL_TOTAL);
            }
            return ret;
        }
    }

    public int colTotal
    {
        get
        {
            int ret = Mathf.Min(ROW_TOTAL, COL_TOTAL);
            if (Device.isLandscape)
            {
                ret = Mathf.Max(ROW_TOTAL, COL_TOTAL);
            }
            return ret;
        }
    }

    float posZ = -1f;
    static private GameUtil _main = null;
    public static GameUtil main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameUtil();
            }
            return _main;
        }
    }
    public Vector3 GetDotPostion(int r, int c)
    {
        float x, y, w, h;
        Vector2 sizeWorld = Common.GetWorldSize(AppSceneBase.main.mainCamera);
        Vector2 size = new Vector2(sizeWorld.x, sizeWorld.y);
        float oftTop = GameManager.main.heightAdWorld + Common.ScreenToWorldHeight(AppSceneBase.main.mainCamera, Device.offsetTop);
        float oftBottom = GameManager.main.heightAdWorld + Common.ScreenToWorldHeight(AppSceneBase.main.mainCamera, Device.offsetBottom);
        float oftLeft = Common.ScreenToWorldHeight(AppSceneBase.main.mainCamera, Device.offsetLeft);
        float oftRight = Common.ScreenToWorldHeight(AppSceneBase.main.mainCamera, Device.offsetRight);
        size.y = size.y - oftTop - oftBottom;
        size.x = size.x - oftLeft - oftRight;
        //  RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        // w = rctran.rect.width;
        //  h = rctran.rect.height;
        w = size.x;
        h = size.y;

        Vector2 space = Vector2.zero;
        float item_w = (w - (space.x * (colTotal - 1))) / colTotal;
        float item_h = (h - (space.y * (rowTotal - 1))) / rowTotal;

        x = (-sizeWorld.x / 2 + oftLeft) + item_w * c + item_w / 2 + space.x * c;
        y = (-sizeWorld.y / 2 + oftBottom) + item_h * r + item_h / 2 + space.y * r;

        return new Vector3(x, y, posZ);

    }

    public int GetDotCol(Vector3 pt)
    {
        int ret = 0;
        float x, y, w, h;
        Vector2 size = Common.GetWorldSize(AppSceneBase.main.mainCamera);
        w = size.x;
        h = size.y;
        Vector2 space = Vector2.zero;
        float item_w = (w - (space.x * (colTotal - 1))) / colTotal;
        float item_h = (h - (space.y * (rowTotal - 1))) / rowTotal;
        x = pt.x + w / 2;
        float v = x / item_w;
        ret = Mathf.FloorToInt(v);
        if ((v - ret) > 0.5f)
        {
            //ret++;
        }
        return ret;
    }

    public int GetDotRow(Vector3 pt)
    {
        int ret = 0;
        float x, y, w, h;
        Vector2 size = Common.GetWorldSize(AppSceneBase.main.mainCamera);
        w = size.x;
        h = size.y;
        Vector2 space = Vector2.zero;
        float item_w = (w - (space.x * (colTotal - 1))) / colTotal;
        float item_h = (h - (space.y * (rowTotal - 1))) / rowTotal;
        y = pt.y + h / 2;
        float v = y / item_h;
        ret = Mathf.FloorToInt(v);
        if ((v - ret) > 0.5f)
        {
            // ret++;
        }
        return ret;
    }



}
