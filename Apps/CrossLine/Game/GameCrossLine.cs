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


    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
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
        CrossItemInfo info = (CrossItemInfo)GameGuankaParse.main.GetGuankaItemInfo(level);
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
        int row = 17;
        int col = 11;
        Texture2D texDot = TextureCache.main.Load(GameRes.Image_GameDot);
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                GameObject obj = new GameObject("dot_" + i + "x" + j);
                SpriteRenderer rd = obj.AddComponent<SpriteRenderer>();
                rd.sprite = LoadTexture.CreateSprieFromTex(texDot);
                obj.transform.SetParent(this.transform);
            }
        }
    }
    void DrawDots()
    {

    }

    void DrawLines()
    {

    }

    string GetLineName(int idx)
    {
        return "line" + idx.ToString();
    }
    LineInfo CreateLine(int start = 0, int end = 0)
    {
        LineInfo linfo = new LineInfo();
        linfo.idxStart = start;
        linfo.idxEnd = end;
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
        Debug.Log("CreateLine start=" + start + " end=" + end);
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

