
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UICellWord : UIView
{
    public List<UILetterItem> listItem;
    public int index;
    public UILetterItem uiLetterItemPrefab;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        listItem = new List<UILetterItem>();
        LayOut();
    }

    void LoadPrefab()
    {
        //     {
        //         GameObject obj = PrefabCache.main.Load(GameRes.PREFAB_LETTER_ITEM);
        //         if (obj != null)
        //         {
        //             uiLetterItemPrefab = obj.GetComponent<UILetterItem>();
        //         }
        //     }
    }
    public override void LayOut()
    {
        LayOutBase lay = this.GetComponent<LayOutBase>();
        if (lay != null)
        {
            lay.LayOut();
        }

    }

    public void UpdateItem(bool isHowToPlay = false)
    {
        WordItemInfo info = GameGuankaParse.main.GetItemInfo();
        string word = info.listAnswer[index];
        int len = word.Length;
        if (isHowToPlay)
        {
            if (index == 0)
            {
                word = "SO";
            }
            if (index == 1)
            {
                word = "SOL";
            }
        }
        for (int i = 0; i < len; i++)
        {
            UILetterItem item = GameObject.Instantiate(uiLetterItemPrefab);
            item.index = i;
            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            item.UpdateItem(word.Substring(i, 1));
            if (isHowToPlay)
            {
                if (index == 0)
                {
                    item.SetStatus(UILetterItem.Status.UNLOCK);
                }
                if (index == 1)
                {
                    item.SetStatus(UILetterItem.Status.LOCK);
                }
            }

            listItem.Add(item);
        }
    }

    public UILetterItem GetItem(int idx)
    {
        UILetterItem item = listItem[idx] as UILetterItem;
        return item;
    }
    public void SetStatus(UILetterItem.Status st)
    {
        foreach (UILetterItem item in listItem)
        {
            item.SetStatus(st);
        }
    }
    public void OnClickGold()
    {

    }


}
