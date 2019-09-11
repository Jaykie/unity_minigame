
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
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<UICellWord>();
        LoadPrefab();
        //UpdateItem();
        LayOut();
    }

    void LoadPrefab()
    {

    }
    public override void LayOut()
    {
        LayOutVertical ly = objScrollContent.GetComponent<LayOutVertical>();
        if (ly != null)
        {
            ly.LayOut();
        }
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
        float w_item = 128f;
        float h_item = 128f;
        for (int i = 0; i < len; i++)
        {
            UICellWord item = GameObject.Instantiate(uiCellWordPrefab);
            RectTransform rctran = this.GetComponent<RectTransform>();
            string word = info.listAnswer[i];
            w = w_item * word.Length;
            h = h_item;
            rctran.sizeDelta = new Vector2(w, h);
            item.index = i;
            item.transform.SetParent(objScrollContent.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            item.UpdateItem();
            listItem.Add(item);
        }
        LayOutVertical ly = objScrollContent.GetComponent<LayOutVertical>();
        if (ly != null)
        {
            ly.LayOut();
        }
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
