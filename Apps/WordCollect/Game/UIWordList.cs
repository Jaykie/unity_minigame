
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
    int colTotal;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
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
        LayOutGrid ly = objScrollContent.GetComponent<LayOutGrid>();
        if (ly != null)
        {
            ly.LayOut();
        }

        RectTransform rctran = this.GetComponent<RectTransform>();
        SetContentHeight(rctran.rect.height);

        foreach (UICellWord item in listItem)
        {

            RectTransform rctranCell = item.GetComponent<RectTransform>();
            w = rctran.rect.width / colTotal;
            h = rctranCell.rect.height;
            rctranCell.sizeDelta = new Vector2(w, h);

            item.LayOut();
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

        LayOutGrid ly = objScrollContent.GetComponent<LayOutGrid>();
        colTotal = 1;
        ly.row = len;
        Debug.Log("len = " + len + " row_display=" + row_display + " rctran.rect.height=" + rctran.rect.height + " h_item=" + h_item);

        if (len > row_display)
        {
            colTotal = 2;
            ly.row = row_display;
        }


        ly.col = colTotal;


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

    public bool IsGameWin()
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
