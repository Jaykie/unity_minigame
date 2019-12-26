
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameUtil
{
    public int rowTotal = 17;
    public int colTotal = 11;
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
        Vector2 size = Common.GetWorldSize(AppSceneBase.main.mainCamera);
        //  RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        // w = rctran.rect.width;
        //  h = rctran.rect.height;
        w = size.x;
        h = size.y;

        Vector2 space = Vector2.zero;
        float item_w = (w - (space.x * (colTotal - 1))) / colTotal;
        float item_h = (h - (space.y * (rowTotal - 1))) / rowTotal;

        x = -w / 2 + item_w * c + item_w / 2 + space.x * c;
        y = -h / 2 + item_h * r + item_h / 2 + space.y * r;

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
