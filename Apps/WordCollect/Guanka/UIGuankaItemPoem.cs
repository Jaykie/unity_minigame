using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIGuankaItemPoem : UIView
{
    public Text textTitle;
    public Image imageBg;
    public UILetterItem uiLetterItemPrefab;
    public int index;
    public List<object> listItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
    }
    void Update()
    {
        // LayOut();
    }
    void Start()
    {
        LayOut();
        Invoke("LayOut", 0.5f);
    }
    void ClearLetterItem()
    {
        foreach (LetterItem item in listItem)
        {
            DestroyImmediate(item.gameObject);
        }
        listItem.Clear();
    }
    public void UpdateItem(WordItemInfo info)
    {
        textTitle.text = (index + 1).ToString();
        string title = info.id;
        int len = title.Length;
        ClearLetterItem();

        for (int i = 0; i < len; i++)
        {
            UILetterItem item = GameObject.Instantiate(uiLetterItemPrefab);
            item.index = i;
            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            RectTransform rctran = item.GetComponent<RectTransform>();
            item.SetStatus(UILetterItem.Status.UNLOCK);
            item.UpdateItem(title.Substring(i, 1));
            listItem.Add(item);
        }

        LayOut();
        InitAnimate();
    }
    public void InitAnimate()
    {
        float duration = 0.5f;
        for (int i = 0; i < listItem.Count; i++)
        {
            UILetterItem item = listItem[i] as UILetterItem;
            Vector3 posNormal = GetItemPos(i);
            //item.transform.localPosition = new Vector3(0, 0, posNormal.z);
            item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            Sequence seq = DOTween.Sequence();
            //Tweener ani1 = item.transform.DOLocalMove(posNormal, duration);
            float angel = -30 + Random.Range(0, 100) * 60f / 100;
            Tweener aniAngle1 = item.transform.DOLocalRotate(new Vector3(0, 0, angel), duration);
            //seq.Append(ani1).Join(aniAngle1);
            seq.Append(aniAngle1);
        }

    }
    public Vector3 GetItemPos(int idx)
    {
        Vector3 ret = Vector2.zero;
        RectTransform rctran = this.GetComponent<RectTransform>();

        UILetterItem item = listItem[0] as UILetterItem;
        RectTransform rctranItem = item.GetComponent<RectTransform>();
        Debug.Log("GetItemPos rctran=" + rctran.rect.size);
        float ratio = 0.7f;
        float size = Mathf.Min(rctran.rect.size.x, rctran.rect.size.y);

        float r = size / 2 * ratio;
        float right = (size / 2) * 0.9f;
        // if (r + rctranItem.rect.size.x / 2 > right)
        {
            r = right - rctranItem.rect.size.x / 2;
        }

        float angle = 0f;
        int count = listItem.Count;
        //顺时针排列
        angle = Mathf.PI / 2 - (Mathf.PI * 2) * idx / count;

        ret.x = r * Mathf.Cos(angle);
        ret.y = r * Mathf.Sin(angle);

        return ret;
    }
    public override void LayOut()
    {
        RectTransform rctran = this.GetComponent<RectTransform>();

        for (int i = 0; i < listItem.Count; i++)
        {
            UILetterItem item = listItem[i] as UILetterItem;
            rctran = item.GetComponent<RectTransform>();
            float sz = 0f;
            if (Device.isLandscape)
            {
                sz = 128;
            }
            else
            {
                sz = 100;
            }
            rctran.sizeDelta = new Vector2(sz, sz);
            rctran.anchoredPosition = GetItemPos(i);
        }

    }

}
