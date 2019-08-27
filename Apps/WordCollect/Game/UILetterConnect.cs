
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UILetterConnect : UIView
{
    public Image imageBg;
    public Text textTitle;

    UILetterItem uiLetterItemPrefab;
    public List<object> listItem;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
        LoadPrefab();
        UpdateItem();
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

    public Vector2 GetItemPos(int idx)
    {
        Vector2 ret = Vector2.zero;
        RectTransform rctran = this.GetComponent<RectTransform>();
        float r = Mathf.Min(rctran.rect.size.x, rctran.rect.size.y) * 0.5f;
        float angle = 0f;
        int count = listItem.Count;
        angle = (Mathf.PI * 2) * idx / count;
        ret.x = r * Mathf.Cos(angle);
        ret.y = r * Mathf.Sin(angle);
        return ret;
    }

    public void UpdateItem()
    {
        WordItemInfo info = GameGuankaParse.main.GetItemInfo();
        int len = info.listLetter.Length;
        for (int i = 0; i < len; i++)
        {
            UILetterItem item = GameObject.Instantiate(uiLetterItemPrefab);
            item.index = i;
            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            RectTransform rctran = item.GetComponent<RectTransform>();
            rctran.anchoredPosition = GetItemPos(i);
            item.UpdateItem(info.listLetter[i]);
            listItem.Add(item);
        }
    }


    public void OnClickAgain()
    {

    }
    public void OnClickTips()
    {

    }
}
