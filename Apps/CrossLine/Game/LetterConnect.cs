
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vectrosity;


public interface ILetterConnectDelegate
{
    void OnLetterConnectDidRightAnswer(LetterConnect lc, int idx);
    void OnLetterConnectDidUpdateItem(LetterConnect lc, int[] itemIndex);
}


public class LineInfo
{
    public List<Vector3> listPoint;
    public VectorLine line;
    public int idxStart;
    public int idxEnd;

}

public class LetterConnect : UIView
{
    public const int INDEX_END_LAST = -1;
    public GameObject objSpriteBg;
    public BoxCollider boxCollider;
    public LetterItem letterItemPrefab;

    public List<object> listItem;
    public List<object> listLine;
    public List<int> listIndexClick;
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
    bool isOutOfMaxItemCount;//超出范围
    int[] rdmItemIndex;

    private ILetterConnectDelegate _delegate;
    public ILetterConnectDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
        listLine = new List<object>();
        listIndexClick = new List<int>();

        matLine = new Material(Shader.Find("Custom/LineConnect"));
        UITouchEventWithMove ev = this.gameObject.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;


    }

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //UpdateItem();
        // InitAnimate();
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
            float sz = 0f;
            if (Device.isLandscape)
            {
                sz = 1f;
            }
            else
            {
                sz = 0.8f;
            }
            if (Common.appKeyName == GameRes.GAME_IDIOM)
            {
                sz = sz * 0.8f;
            }
            rctran.sizeDelta = new Vector2(sz, sz);
            // rctran.anchoredPosition = GetItemPos(i);
            item.transform.localPosition = GetItemPos(rdmItemIndex[i]);
        }

    }

    public void InitAnimate()
    {
        float duration = 0.5f;
        for (int i = 0; i < listItem.Count; i++)
        {
            LetterItem item = listItem[i] as LetterItem;
            Vector3 posNormal = GetItemPos(rdmItemIndex[i]);
            item.transform.localPosition = new Vector3(0, 0, posNormal.z);
            item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            Sequence seq = DOTween.Sequence();
            //Tweener ani1 = item.transform.DOLocalMove(posNormal, duration);
            float angel = -30 + Random.Range(0, 100) * 60f / 100;
            Tweener aniAngle1 = item.transform.DOLocalRotate(new Vector3(0, 0, angel), duration);
            //seq.Append(ani1).Join(aniAngle1);
            seq.Append(aniAngle1);
        }
        if (iDelegate != null)
        {
            iDelegate.OnLetterConnectDidUpdateItem(this, rdmItemIndex);
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


        rdmItemIndex = Common.RandomIndex(listItem.Count, listItem.Count);

        if (iDelegate != null)
        {
            iDelegate.OnLetterConnectDidUpdateItem(this, rdmItemIndex);
        }
        LayOut();
        InitAnimate();
    }
    public Vector3 GetItemPos(int idx)
    {
        Vector3 ret = Vector2.zero;
        RectTransform rctran = this.GetComponent<RectTransform>();

        LetterItem item = listItem[0] as LetterItem;
        RectTransform rctranItem = item.GetComponent<RectTransform>();
        Debug.Log("GetItemPos rctran=" + rctran.rect.size);
        float ratio = 0.8f;
        float size = Mathf.Min(rctran.rect.size.x, rctran.rect.size.y);
        float r = size / 2 * ratio;
        float right = (size / 2) * 0.9f;
        // if (r + rctranItem.rect.size.x / 2 > right)
        {
            r = right - rctranItem.rect.size.x / 2;
        }

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
        listIndexClick.Add(idx);
        UpdateConnectWord();
        LetterItem item = GetItem(idx);
        item.OnItemDidSelect();
    }


    public void UpdateConnectWord()
    {
        strLetter = "";
        foreach (int idx in listIndexClick)
        {
            LetterItem item = GetItem(idx);
            strLetter += item.textTitle.text;
        }
        uiLetterConnect.UpdateText(strLetter);
        uiLetterConnect.ShowText(true);
    }
    int GetIndexRightAnswer(string str)
    {
        int ret = -1;
        WordItemInfo info = GameGuankaParse.main.GetItemInfo();
        for (int i = 0; i < info.listAnswer.Length; i++)
        {
            string word = info.listAnswer[i];
            if (word.ToLower() == str.ToLower())
            {
                ret = i;
                break;
            }
        }
        return ret;
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
        objLine = lineConnect.GetObj();
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

    LineInfo GetLine(int start, int end)
    {
        foreach (LineInfo info in listLine)
        {
            if ((info.idxStart == start) && (info.idxEnd == end))
            {
                return info;
            }
        }
        return null;
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

    bool IsLineCreated(int start, int end)
    {
        bool ret = false;
        foreach (LineInfo info in listLine)
        {
            if ((info.idxStart == start) && (info.idxEnd == end))
            {
                ret = true;
            }
        }
        return ret;
    }
    void DestroyLastLine()
    {

        LineInfo linfo = GetLastLineInfo();
        if (linfo != null)
        {
            GameObject obj = linfo.line.GetObj();
            Debug.Log("DestroyLastLine name = " + obj.name);
            DestroyImmediate(obj);
        }
        if (listLine.Count > 0)
        {
            listLine.RemoveAt(listLine.Count - 1);
        }
        indexLine = listLine.Count;

        if (listIndexClick.Count > 0)
        {
            listIndexClick.RemoveAt(listIndexClick.Count - 1);
        }
        if (listIndexClick.Count > 0)
        {
            indexClickPre = listIndexClick[listIndexClick.Count - 1];
        }
        UpdateConnectWord();
    }

    public void DestroyAllLine()
    {
        foreach (LineInfo info in listLine)
        {
            DestroyImmediate(info.line.GetObj());
        }
        listLine.Clear();
        indexLine = 0;
        listIndexClick.Clear();
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

    void OnCheckAnswer()
    {
        int idx = GetIndexRightAnswer(strLetter);
        if (idx >= 0)
        {
            //right
            WordItemInfo info = GameGuankaParse.main.GetItemInfo();
            if (iDelegate != null)
            {
                iDelegate.OnLetterConnectDidRightAnswer(this, idx);
            }
            AudioPlay.main.PlayFile(GameRes.Audio_WordRight);
        }
        else
        {
            //error
            AudioPlay.main.PlayFile(GameRes.Audio_WordError);
        }
    }

    public void OnClickAgain()
    {
        float duration = 0.5f;
        rdmItemIndex = Common.RandomIndex(listItem.Count, listItem.Count);
        for (int i = 0; i < listItem.Count; i++)
        {
            LetterItem item = listItem[i] as LetterItem;
            Vector3 posNormal = GetItemPos(rdmItemIndex[i]);
            Sequence seq = DOTween.Sequence();
            Tweener ani0 = item.transform.DOLocalMove(new Vector3(0, 0, posNormal.z), duration);
            Tweener aniAngle0 = item.transform.DOLocalRotate(new Vector3(0, 0, 0), duration);

            Tweener ani1 = item.transform.DOLocalMove(posNormal, duration);

            float angel = -30 + Random.Range(0, 100) * 60f / 100;
            Tweener aniAngle1 = item.transform.DOLocalRotate(new Vector3(0, 0, angel), duration);

            seq.Append(ani0).Join(aniAngle0).AppendInterval(duration / 10).Append(ani1).Join(aniAngle1);
        }

        if (iDelegate != null)
        {
            iDelegate.OnLetterConnectDidUpdateItem(this, rdmItemIndex);
        }

    }
    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                OnTouchDown();
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                OnTouchMove();
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                OnTouchUp();
                break;
        }
    }

    public void Clear(LineInfo info = null)
    {
        // if (listPoint != null)
        // {
        //     listPoint.Clear();
        // }
        // if (lineConnect != null)
        // {
        //     lineConnect.Draw3D();
        // }
        LineInfo linfo = info;
        if (linfo == null)
        {
            linfo = GetLastLineInfo();
        }
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
    public void AddPoint(Vector3 pos, LineInfo info = null)
    {
        pos.z = 0;
        Debug.Log("AddPoint pos=" + pos);
        LineInfo linfo = info;
        if (linfo == null)
        {
            linfo = GetLastLineInfo();
        }
        if (linfo != null)
        {
            linfo.listPoint.Add(GetTouchLocalPosition(pos));
            linfo.line.Draw3D();
        }


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

    void DrawLine(LineInfo info = null)
    {
        Clear(info);
        // Debug.Log("onTouchMove ptStart=" + ptStart + " ptEnd=" + ptEnd + " idx=" + idx);
        AddPoint(ptStart, info);
        AddPoint(ptEnd, info);
    }

    void OnDrawLine(int idx)
    {
        for (int i = 0; i < listIndexClick.Count; i++)
        {
            int idxStart = listIndexClick[i];
            int idxEnd = 0;
            if (i < listIndexClick.Count - 1)
            {
                idxEnd = listIndexClick[i + 1];
                ptStart = GetItemWorldPos(idxStart);
                ptEnd = GetItemWorldPos(idxEnd);

                //重绘最后一条line,矫正位置
                LineInfo infotmp = GetLine(idxStart, INDEX_END_LAST);
                if (infotmp != null)
                {
                    infotmp.idxEnd = idxEnd;
                    DrawLine(infotmp);
                }


                if (!IsLineCreated(idxStart, idxEnd))
                {
                    LineInfo info = CreateLine(idxStart, idxEnd);
                    DrawLine(info);
                }

            }
            else
            {
                if (idx >= 0)
                {

                }
                else
                {
                    //最后line 动态变化
                    ptStart = GetItemWorldPos(idxStart);
                    ptEnd = Common.GetInputPositionWorld(mainCam);
                    idxEnd = INDEX_END_LAST;//listIndexClick[i];
                                            // if (!IsLineCreated(idxStart, idxEnd))


                    // if (GetLineByName(GetLineName(listIndexClick.Count - 1)) == null)
                    {
                        if (!IsLineCreated(idxStart, idxEnd))
                        {
                            LineInfo info = CreateLine(idxStart, idxEnd);
                        }
                    }
                    LineInfo last = GetLastLineInfo();
                    DrawLine(last);
                }

            }


        }
    }
    void OnTouchDown()
    {
        int idx = GetTouchItem();
        isStartLine = false;
        isEndLine = false;
        isOutOfMaxItemCount = false;
        if (idx >= 0)
        {
            indexClickPre = idx;
            indexClickCur = idx;
            ptStart = GetItemWorldPos(idx);
            Debug.Log("onTouchDown idx=" + idx + " ptStart=" + ptStart);
            isStartLine = true;
            //CreateLine();
            strLetter = "";
            uiLetterConnect.UpdateText(strLetter);
            OnLetterDidClick(idx);
        }

    }
    void OnTouchMove()
    {
        int idx = GetTouchItem();
        if (idx >= 0)
        {
            indexClickCur = idx;
            if (indexClickPre != indexClickCur)
            {
                OnLetterDidClick(idx);
                indexClickPre = indexClickCur;
            }
        }
        OnDrawLine(idx);
        //Debug.Log("DestroyLastLineout idx= " + idx + "listIndexClick.Count=" + listIndexClick.Count + " listItem.Count=" + listItem.Count);
        if (idx >= 0)
        {

            //往回画线 擦除功能
            if (listIndexClick.Count >= 3)
            {
                int idxLast2 = listIndexClick[listIndexClick.Count - 3];
                // Debug.Log("onTouchMove DestroyLastLine 2 idxLast2=" + idxLast2 + " idx=" + idx + " count=" + listIndexClick.Count);
                if (idxLast2 == idx)
                {
                    //Debug.Log("onTouchMove DestroyLastLine 2=");
                    LetterItem item = GetItem(listIndexClick[listIndexClick.Count - 2]);
                    item.OnItemDidUnSelect();
                    //删除最后两根线
                    DestroyLastLine();
                    DestroyLastLine();

                }
            }


            if (listIndexClick.Count > listItem.Count)
            {
                //超出最大个数 
                // Debug.Log("DestroyLastLineout of max");
                DestroyLastLine();

            }

        }


    }

    void OnTouchUp()
    {
        int idx = GetTouchItem();
        uiLetterConnect.ShowText(false);
        if (idx >= 0)
        {

        }
        else
        {
            //  DestroyLastLine();
        }
        OnCheckAnswer();
        DestroyAllLine();

        for (int i = 0; i < listItem.Count; i++)
        {
            LetterItem item = listItem[i] as LetterItem;
            item.OnItemDidUnSelect();
        }
    }
}
