
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UICellWord : UIView
{


    public List<UILetterItem> listItem;
    public int index;
    UILetterItem uiLetterItemPrefab;
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
        {
            GameObject obj = PrefabCache.main.Load(GameRes.PREFAB_LETTER_ITEM);
            if (obj != null)
            {
                uiLetterItemPrefab = obj.GetComponent<UILetterItem>();
            }
        }
    }
    public override void LayOut()
    {

    }

    public void UpdateItem()
    {
        WordItemInfo info = GameGuankaParse.main.GetItemInfo();
        string word = info.listAnswer[index];
        int len = word.Length;
        for (int i = 0; i < len; i++)
        {
            UILetterItem item = GameObject.Instantiate(uiLetterItemPrefab);
            item.index = i;
            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            item.UpdateItem(word.Substring(i, 1));
            listItem.Add(item);
        }
    }
    public void OnClickGold()
    {

    }


}
