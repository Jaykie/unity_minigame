
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;

public interface IUILetterConnectDelegate
{
    void OnUILetterConnectDidAgain(UILetterConnect ui);
    void OnUILetterConnectDidTips(UILetterConnect ui);
}

public class UILetterConnect : UIView
{
    public List<UILetterItem> listItem;
    public UILetterItem uiLetterItemPrefab;
    public Text textTitle;
    public Image imageTitle;
    public Button btnTips;
    public GameObject objLetterAnimate;
    public float durationAnimate;
    int[] rdmItemIndex;

    private IUILetterConnectDelegate _delegate;
    public IUILetterConnectDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        btnTips.gameObject.SetActive(AppVersion.appCheckHasFinished);
        ShowText(false);
        listItem = new List<UILetterItem>();
        durationAnimate = 1f;
        LayOut();
    }

    void LoadPrefab()
    {

    }
    public override void LayOut()
    {
        if (rdmItemIndex == null)
        {
            return;
        }
        for (int i = 0; i < listItem.Count; i++)
        {
            UILetterItem item = listItem[i];
            RectTransform rctran = item.GetComponent<RectTransform>();
            if (i >= rdmItemIndex.Length)
            {
                continue;
            }
            Debug.Log("rdmItemIndex=" + rdmItemIndex.Length + " i=" + i + " listItem.Count=" + listItem.Count);
            Debug.Log("rdmItemIndex[i] = " + rdmItemIndex[i]);

            rctran.anchoredPosition = GetItemPos(rdmItemIndex[i]);
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

    public void UpdateItem()
    {
        UpdateUILetterItem();
    }

    void ClearLetterItem()
    {
        foreach (UILetterItem item in listItem)
        {
            DestroyImmediate(item.gameObject);
        }
        listItem.Clear();
    }

    void UpdateUILetterItem()
    {
        ClearLetterItem();
        WordItemInfo info = GameGuankaParse.main.GetItemInfo();
        int len = info.listLetter.Length;
        for (int i = 0; i < len; i++)
        {
            UILetterItem item = GameObject.Instantiate(uiLetterItemPrefab);
            item.index = i;
            item.transform.SetParent(objLetterAnimate.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            item.UpdateItem(info.listLetter[i]);
            item.SetStatus(UILetterItem.Status.UNLOCK);
            item.gameObject.SetActive(false);
            listItem.Add(item);
        }
        rdmItemIndex = Common.RandomIndex(listItem.Count, listItem.Count);

        LayOut();
        Debug.Log("rdmItemIndex UILetterConnect =" + " listItem.Count=" + listItem.Count);

    }

    public void UpdateText(string str)
    {
        textTitle.text = str;
        RectTransform rctran = imageTitle.GetComponent<RectTransform>();
        float w, h;
        int fontSize = textTitle.fontSize;
        w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontSize) + fontSize;
        h = rctran.sizeDelta.y;
        rctran.sizeDelta = new Vector2(w, h);
    }

    public void ShowText(bool isShow)
    {
        imageTitle.gameObject.SetActive(isShow);
    }

    public void RunItemAnimate(LetterConnect lc, UICellWord cellword, UIGameBase game, Action<UILetterConnect> onComplete = null)
    {
        WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetGuankaItemInfo(LevelManager.main.gameLevel);
        for (int i = 0; i < lc.listIndexClick.Count; i++)
        {
            UILetterItem itemAnser = cellword.GetItem(i);
            UILetterItem item = listItem[lc.listIndexClick[i]];
            item.gameObject.SetActive(true);
            Vector2 posOrigin = item.transform.position;
            Vector2 posEnd = itemAnser.transform.position;
            if (info.gameType == GameRes.GAME_TYPE_IMAGE)
            {
                UIGameWordCollect uiGame = game as UIGameWordCollect;
                posEnd = uiGame.uiWordContent.transform.position;
            }

            item.transform.DOMove(posEnd, durationAnimate).OnComplete(() =>
              {
                  item.gameObject.SetActive(false);
                  item.transform.position = posOrigin;

                  if (onComplete != null)
                  {
                      onComplete(this);
                  }
              });
        }
    }
    public void OnLetterConnectDidUpdateItem(LetterConnect lc, int[] itemIndex)
    {
        rdmItemIndex = itemIndex;
        for (int i = 0; i < listItem.Count; i++)
        {
            UILetterItem item = listItem[i];
            RectTransform rctran = item.GetComponent<RectTransform>();
            if (i >= rdmItemIndex.Length)
            {
                continue;
            }
            Debug.Log("rdmItemIndex=" + rdmItemIndex.Length + " i=" + i + " listItem.Count=" + listItem.Count);
            Debug.Log("rdmItemIndex[i] = " + rdmItemIndex[i]);

            rctran.anchoredPosition = GetItemPos(rdmItemIndex[i]);
        }

    }
    public void OnClickBtnHelp()
    {
        HowToPlayViewController.main.Show(null, null);
    }

    public void OnClickAgain()
    {
        if (iDelegate != null)
        {
            iDelegate.OnUILetterConnectDidAgain(this);
        }
    }
    public void OnClickTips()
    {
        if (iDelegate != null)
        {
            iDelegate.OnUILetterConnectDidTips(this);
        }
    }
}
