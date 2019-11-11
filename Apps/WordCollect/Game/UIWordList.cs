
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UIWordList : UIView
{
    public ScrollRect scrollRect;
    public GameObject objScrollContent;

    public UICellWord uiCellWordPrefab;

    public List<UICellWord> listItem;
    int totalPage;
    int cellOnePage;
    int colTotal;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        totalPage = 2;
        listItem = new List<UICellWord>();
        colTotal = 1;
        LoadPrefab();
        //UpdateItem();
        LayOut();
    }

    void LoadPrefab()
    {

    }
    public override void LayOut()
    {
        float x, y, w, h;

        RectTransform rctran = this.GetComponent<RectTransform>();
        //SetContentHeight(rctran.rect.height);

        foreach (UICellWord item in listItem)
        {
            RectTransform rctranCell = item.GetComponent<RectTransform>();
            w = rctran.rect.width / colTotal;
            h = rctranCell.rect.height;
            rctranCell.sizeDelta = new Vector2(w, h);
            item.LayOut();
        }

        LayOutGrid ly = objScrollContent.GetComponent<LayOutGrid>();
        if (ly != null)
        {
            ly.LayOut();
        }
    }
    public void SetContentHeight(float h)
    {
        RectTransform rctran = objScrollContent.GetComponent<RectTransform>();
        rctran.sizeDelta = new Vector2(rctran.sizeDelta.x, h);
    }
    public void Clear()
    {
        foreach (UICellWord item in listItem)
        {
            DestroyImmediate(item.gameObject);
        }
        listItem.Clear();
    }

    public void UpdateItem()
    {
        float x, y, w, h;
        Clear();
        WordItemInfo info = GameGuankaParse.main.GetItemInfo();
        int len = info.listAnswer.Length;
        RectTransform rctran = this.GetComponent<RectTransform>();
        RectTransform rctranCell = uiCellWordPrefab.GetComponent<RectTransform>();
        float w_item = 128f;
        float h_item = rctranCell.rect.height;
        int row_display = (int)(rctran.rect.height / h_item);
        if ((int)(rctran.rect.height) % (int)h_item != 0)
        {
            row_display++;
        }
        totalPage = (int)(h_item * len / rctran.rect.height);
        if ((int)(h_item * len) % (int)rctran.rect.height != 0)
        {
            totalPage++;
        }
        if (info.gameType == GameRes.GAME_TYPE_WORDLIST)
        {
            totalPage = 1;
        }
        cellOnePage = row_display;

        LayOutGrid ly = objScrollContent.GetComponent<LayOutGrid>();
        colTotal = 1;
        ly.row = len;
        Debug.Log("len = " + len + " row_display=" + row_display + " rctran.rect.height=" + rctran.rect.height + " h_item=" + h_item);

        if (len > row_display)
        {
            colTotal = 2;
            ly.row = row_display;
        }

        if (info.gameType == GameRes.GAME_TYPE_POEM)
        {
            colTotal = 1;
        }

        ly.col = colTotal;


        // //更新scrollview 内容的长度  
        SetContentHeight(rctran.rect.height * totalPage);

        Debug.Log(" totalPage=" + totalPage + " len=" + len);

        for (int i = 0; i < len; i++)
        {
            UICellWord item = GameObject.Instantiate(uiCellWordPrefab);
            string word = info.listAnswer[i];
            w = w_item * word.Length;
            h = h_item;
            // rctran.sizeDelta = new Vector2(w, h);
            item.index = i;
            item.transform.SetParent(objScrollContent.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            item.UpdateItem();
            listItem.Add(item);
        }

        LayOut();
    }

    public UICellWord GetItem(int idx)
    {
        UICellWord item = listItem[idx] as UICellWord;
        return item;
    }

    //
    public UICellWord GetFirstLockItem()
    {
        UICellWord ret = null;
        foreach (UICellWord item in listItem)
        {
            UILetterItem ltitem = item.GetItem(0);
            if (ltitem.GetStatus() == UILetterItem.Status.LOCK)
            {
                ret = item;
                break;
            }

        }
        return ret;
    }
    public void SetStatus(int idx, UILetterItem.Status st)
    {
        UICellWord item = GetItem(idx);
        item.SetStatus(st);
    }

    public void OnClickGold()
    {

    }

    public void GotoListIndex(int idx)
    {
        UICellWord item = GetItem(0);
        RectTransform rctranCell = item.GetComponent<RectTransform>();
        RectTransform rctran = this.GetComponent<RectTransform>();
        float h = rctranCell.rect.height;
        float y = scrollRect.content.anchoredPosition.y;
        float y_to = h * idx;

        int page = (int)(rctranCell.rect.height * idx / rctran.rect.height);
        if (y_to >= ((page + 1) * rctran.rect.height - h))
        {
            //下一页
            scrollRect.content.anchoredPosition = new Vector2(0, y_to - h);
        }

        // SetScrollViewPage(page);
        y = scrollRect.content.anchoredPosition.y;
        Debug.Log("GotoListIndex y=" + y + " h=" + h);
    }

    void SetScrollViewPage(int page)
    {
        RectTransform rctran = this.GetComponent<RectTransform>();
        float h = rctran.rect.height;
        scrollRect.content.anchoredPosition = new Vector2(0, h * page);
    }


    public bool CheckAllAnswerFinish()
    {

        bool ret = true;
        foreach (UICellWord item in listItem)
        {
            UILetterItem.Status st = item.GetItem(0).GetStatus();
            if (st == UILetterItem.Status.LOCK)
            {
                ret = false;
            }
        }
        return ret;
    }
}
