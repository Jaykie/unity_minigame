using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vectrosity;
/*
参考游戏： 交叉线!
https://www.taptap.com/app/64361

线段交叉算法：
https://www.cnblogs.com/sparkleDai/p/7604895.html
https://blog.csdn.net/rickliuxiao/article/details/6259322

*/


public class LineInfo
{
    public List<Vector3> listPoint;
    public VectorLine line;
    public int idxStart;
    public int idxEnd;

}

public class GameCrossLine : GameBase
{
    public const float RATIO_RECT = 0.9f;

    public List<object> listDot;
    public List<object> listLine;

    float lineWidth = 20f;//屏幕像素
    Material matLine;
    int indexLine;
    int rowTotal;
    int colTotal;
    float posZ;


    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        //11x17
        rowTotal = 8;
        colTotal = 5;
        posZ = -1f;
        listDot = new List<object>();
        listLine = new List<object>();
        DrawBgGrid();
        LayOut();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //  LayOut();
    }

    public Vector3 GetDotPostion(int r, int c)
    {
        float x, y, w, h;
        Vector2 size = Common.GetWorldSize(mainCam);
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

    public override void LayOut()
    {
        float x, y, z, w, h;
        Vector2 sizeWorld = Common.GetWorldSize(mainCam);
        Vector2 sizeCanvas = this.frame.size;
        float ratio = 1f;
        if (sizeCanvas.x <= 0)
        {
            return;
        }

    }
    public void UpdateGuankaLevel(int level)
    {
        CrossItemInfo info = (CrossItemInfo)GameLevelParse.main.GetGuankaItemInfo(level);
        // letterConnect = (LetterConnect)GameObject.Instantiate(letterConnectPrefab);
        // //AppSceneBase.main.AddObjToMainWorld(letterConnect.gameObject); 
        // letterConnect.transform.SetParent(this.transform);
        // UIViewController.ClonePrefabRectTransform(letterConnectPrefab.gameObject, letterConnect.gameObject);
        // letterConnect.transform.localPosition = new Vector3(0f, 0f, -1f);
        DrawDots();
        DrawLines();
        LayOut();
        // LayOut();
        // Invoke("LayOut", 0.6f);
    }


    //背景格子点
    void DrawBgGrid()
    {
        //11x17
        Texture2D texDot = TextureCache.main.Load(GameRes.Image_GameDot);
        for (int i = 0; i < rowTotal; i++)
        {
            for (int j = 0; j < colTotal; j++)
            {
                GameObject obj = new GameObject("dot_" + i + "x" + j);
                SpriteRenderer rd = obj.AddComponent<SpriteRenderer>();
                rd.sprite = TextureUtil.CreateSpriteFromTex(texDot);
                obj.transform.SetParent(this.transform);
                obj.transform.localPosition = GetDotPostion(i, j);
            }
        }
    }
    void DrawDots()
    {

    }

    void DrawLines()
    {
        // {
        //     LineInfo info = CreateLine();
        //     Vector3 pt = GetDotPostion(0, 0);
        //     info.listPoint.Add(pt);
        //     pt = GetDotPostion(2, 3);
        //     info.listPoint.Add(pt);
        //     info.line.Draw();
        // }

        CrossItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();
        for (int i = 0; i < infoGuanka.listLine.Count; i++)
        {
            Vector2 pttmp = infoGuanka.listLine[i];
            LineInfo info = CreateLine();
            Vector3 pt = GetDotPostion((int)pttmp.x, (int)pttmp.y);
            info.listPoint.Add(pt);
            pt = GetDotPostion(2, 3);
            info.listPoint.Add(pt);
            info.line.Draw();
        }

    }

    string GetLineName(int idx)
    {
        return "line" + idx.ToString();
    }
    LineInfo CreateLine()
    {
        LineInfo linfo = new LineInfo();
        float w_line = 20;
        if (Device.isLandscape)
        {
            lineWidth = w_line * Screen.width / 2048;
        }
        else
        {
            lineWidth = w_line * 2 * Screen.width / 1536;
        }

        linfo.listPoint = new List<Vector3>();
        VectorLine lineConnect = new VectorLine(GetLineName(indexLine), linfo.listPoint, lineWidth);
        lineConnect.lineType = LineType.Continuous;
        //圆滑填充画线
        lineConnect.joins = Joins.Fill;
        lineConnect.Draw3D();
        GameObject objLine = lineConnect.GetObj();
        //AppSceneBase.main.AddObjToMainWorld(objLine);
        objLine.transform.parent = this.transform;
        objLine.transform.localScale = new Vector3(1f, 1f, 1f);
        objLine.transform.localPosition = new Vector3(0f, 0f, 0f);
        lineConnect.material = matLine;
        lineConnect.color = Color.red;
        indexLine++;
        linfo.line = lineConnect;

        listLine.Add(linfo);
        return linfo;
    }

    LineInfo GetLineByName(string name)
    {
        foreach (LineInfo info in listLine)
        {
            if (info.line.GetObj().name == name)
            {
                return info;
            }
        }
        return null;
    }
}

