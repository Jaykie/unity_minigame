
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vectrosity;

public class LineInfo
{
    public List<Vector3> listPoint;
    public VectorLine line;
}

public class LetterConnect : UIView
{
    public GameObject objSpriteBg;
    public BoxCollider boxCollider;
    public LetterItem letterItemPrefab;

    public List<object> listItem;
    public List<object> listLine;
    public UILetterConnect uiLetterConnect;

    GameObject objLine;
    float lineWidth = 20f;//屏幕像素
    Material matLine;
    int indexLine;

    Vector3 ptStart;
    Vector3 ptEnd;
    bool isStartLine;
    bool isEndLine;
    string strLetter;
    int indexClickPre;
    int indexClickCur;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
        listLine = new List<object>();
        matLine = new Material(Shader.Find("Custom/LineConnect"));
        UITouchEventWithMove ev = this.gameObject.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;

        UpdateItem();
        LayOut();
    }

    public override void LayOut()
    {
        Vector2 size = Common.GetWorldSize(mainCam);
        boxCollider.size = size;
        RectTransform rctran = this.GetComponent<RectTransform>();
        SpriteRenderer rd = objSpriteBg.GetComponent<SpriteRenderer>();
        rd.size = rctran.rect.size;

        for (int i = 0; i < listItem.Count; i++)
        {
            LetterItem item = listItem[i] as LetterItem;
            rctran = item.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(1f, 1f);
            // rctran.anchoredPosition = GetItemPos(i);
            item.transform.localPosition = GetItemPos(i);
        }

    }

    public void UpdateItem()
    {
        strLetter = "";
        WordItemInfo info = GameGuankaParse.main.GetItemInfo();
        int len = info.listLetter.Length;
        for (int i = 0; i < len; i++)
        {
            LetterItem item = GameObject.Instantiate(letterItemPrefab);
            item.index = i;
            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            RectTransform rctran = item.GetComponent<RectTransform>();
            //rctran.anchoredPosition = GetItemPos(i);
            item.UpdateItem(info.listLetter[i]);
            listItem.Add(item);
        }
    }
    public Vector3 GetItemPos(int idx)
    {
        Vector3 ret = Vector2.zero;
        RectTransform rctran = this.GetComponent<RectTransform>();
        float r = Mathf.Min(rctran.rect.size.x, rctran.rect.size.y) * 0.25f;
        float angle = 0f;
        int count = listItem.Count;
        angle = (Mathf.PI * 2) * idx / count;
        ret.x = r * Mathf.Cos(angle);
        ret.y = r * Mathf.Sin(angle);
        return ret;
    }



    public LetterItem GetItem(int idx)
    {
        LetterItem item = listItem[idx] as LetterItem;
        return item;
    }
    public int GetTouchItem()
    {
        int idx = -1;
        for (int i = 0; i < listItem.Count; i++)
        {
            if (IsTouchInItemRect(i))
            {
                idx = i;
                break;
            }
        }
        return idx;
    }


    public Vector3 GetItemWorldPos(int idx)
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        LetterItem item = GetItem(idx);
        //Vector3 pos = Common.CanvasToScreenPoint(sizeCanvas, item.transform.position);
        Vector3 posWorld = item.transform.position;//mainCam.ScreenToWorldPoint(pos);
        return posWorld;
    }
    public bool IsTouchInItemRect(int idx)
    {
        LetterItem item = GetItem(idx);
        bool ret = false;
        Vector2 inputPos = Common.GetInputPositionWorld(mainCam);
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        RectTransform rctran = item.GetComponent<RectTransform>();
        Vector3 pt = item.transform.position;//Common.CanvasToScreenPoint(sizeCanvas, item.transform.position);
        float x, y, w, h;
        w = rctran.rect.width;
        h = rctran.rect.height;
        x = pt.x - w / 2;
        y = pt.y - h / 2;
        Rect rc = new Rect(x, y, w, h);
        Debug.Log("inputPos =" + inputPos + " rc=" + rc + " pt=" + pt);
        if (rc.Contains(inputPos))
        {
            ret = true;
        }
        return ret;
    }

    public void OnLetterDidClick(int idx)
    {
        LetterItem item = GetItem(idx);
        strLetter += item.textTitle.text;
        uiLetterConnect.textTitle.text = strLetter;
        uiLetterConnect.ShowText(true);
    }

    void CreateLine()
    {
        LineInfo linfo = new LineInfo();

        linfo.listPoint = new List<Vector3>();
        VectorLine lineConnect = new VectorLine("line" + indexLine.ToString(), linfo.listPoint, lineWidth);
        lineConnect.lineType = LineType.Continuous;
        //圆滑填充画线
        lineConnect.joins = Joins.Fill;
        lineConnect.Draw3D();
        objLine = lineConnect.GetObj();
        //AppSceneBase.main.AddObjToMainWorld(objLine);
        objLine.transform.parent = this.transform;
        objLine.transform.localScale = new Vector3(1f, 1f, 1f);
        objLine.transform.localPosition = Vector3.zero;
        lineConnect.material = matLine;
        lineConnect.color = Color.red;
        indexLine++;
        linfo.line = lineConnect;

        listLine.Add(linfo);

    }

    void DestroyLastLine()
    {
        LineInfo linfo = GetLastLineInfo();
        if (linfo != null)
        {
            DestroyImmediate(linfo.line.GetObj());
        }
    }

    LineInfo GetLastLineInfo()
    {
        int idx = listLine.Count - 1;
        if (idx < 0)
        {
            return null;
        }
        return listLine[idx] as LineInfo;
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                onTouchDown();
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                onTouchMove();
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                onTouchUp();
                break;
        }
    }

    public void Clear()
    {
        // if (listPoint != null)
        // {
        //     listPoint.Clear();
        // }
        // if (lineConnect != null)
        // {
        //     lineConnect.Draw3D();
        // }
        LineInfo linfo = GetLastLineInfo();
        if (linfo != null)
        {
            linfo.listPoint.Clear();
            linfo.line.Draw3D();
        }
    }

    //是否在字母区域
    public bool IsPointInLetterArea(Vector3 pos)
    {
        bool ret = false;

        return ret;
    }

    //添加世界坐标
    public void AddPoint(Vector3 pos)
    {
        pos.z = 0;
        Debug.Log("PaintLine AddPoint pos=" + pos);
        LineInfo linfo = GetLastLineInfo();
        linfo.listPoint.Add(GetTouchLocalPosition(pos));
        linfo.line.Draw3D();
    }
    Vector3 GetTouchLocalPosition(Vector3 pos)
    {
        // Vector2 inputPos = Common.GetInputPosition();
        // Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
        Vector3 loaclPos = this.transform.InverseTransformPoint(pos);
        loaclPos.z = -2;
        // posTouchWorld.z = 0;
        return loaclPos;
    }
    void onTouchDown()
    {
        int idx = GetTouchItem();
        isStartLine = false;
        isEndLine = false;
        indexClickPre = -1;
        indexClickCur = -1;

        Debug.Log("onTouchDown idx=" + idx);
        if (idx >= 0)
        {
            ptStart = GetItemWorldPos(idx);
            isStartLine = true;
            CreateLine();
            //OnLetterDidClick(idx);
        }

    }
    void onTouchMove()
    {
        int idx = GetTouchItem();
        if (isStartLine)
        {
            if (idx >= 0)
            {
                ptEnd = GetItemWorldPos(idx);
                isStartLine = false;
                isEndLine = true;
                indexClickCur = idx;
                if (indexClickPre != indexClickCur)
                {
                    OnLetterDidClick(idx);
                }

                indexClickPre = indexClickCur;
            }
            else
            {
                ptEnd = Common.GetInputPositionWorld(mainCam);

            }
            Clear();
            AddPoint(ptStart);
            AddPoint(ptEnd);
        }

        if (isEndLine)
        {
            if (idx >= 0)
            {
                isEndLine = false;
                isStartLine = true;
                ptStart = GetItemWorldPos(idx);
                CreateLine();
            }

        }
    }
    void onTouchUp()
    {
        int idx = GetTouchItem();
        uiLetterConnect.ShowText(false);
        if (idx >= 0)
        {

        }
        else
        {
            DestroyLastLine();
        }
    }
}
