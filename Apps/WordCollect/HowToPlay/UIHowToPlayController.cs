using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UIHowToPlayController : UIView

{
    public Image imageBoard;
    public Image imageBg;
    public GameObject objContent;
    public GameObject objScrollView;
    public GameObject objScrollViewContent;

    public UIHowToPlayPage0 uiPage0;
    public UIHowToPlayPage1 uiPage1;
    public UIScrollViewDot uiScrollDot;

    public ScrollRect scrollRect;
    int totalPage = 2;
    int indexPage = 0;
    float action_time = 0.5f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        scrollRect = objScrollView.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(ScrollViewValueChanged);
        //bg 
        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_COMMON_BG, true);
        UIScrollViewTouchEvent ev = objScrollView.AddComponent<UIScrollViewTouchEvent>();
        ev.callbackTouch = OnScrollViewDrag;
        RectTransform rctran = objContent.GetComponent<RectTransform>();
        WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetGuankaItemInfo(LevelManager.main.gameLevel);
        if (info.gameType == GameRes.GAME_TYPE_IMAGE)
        {
            totalPage = 1;
            uiPage1.gameObject.SetActive(false);
        }

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        uiScrollDot.SetTotal(totalPage);
        LayOut();
        ShowAction();
    }
    void ShowAction()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        RectTransform rctran = objContent.GetComponent<RectTransform>();
        Vector2 pt = new Vector2(0, sizeCanvas.y / 2 + rctran.rect.height / 2);
        rctran.DOLocalMove(pt, action_time).From().SetEase(Ease.InOutBounce).OnComplete(
            () =>
            {
                OnUIDidFinish();
            }
        );
    }
    public void OnClickBtnBack()
    {
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }

    }

    public override void LayOut()
    {
        float x, y, w, h;
        float ratio = 1f;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
            float w_image = rectTransform.rect.width;
            float h_image = rectTransform.rect.height;
            print(rectTransform.rect);
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);

        }
        {
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                ratio = 0.7f;
            }
            else
            {
                ratio = 0.7f;
            }
            w = this.frame.width * ratio;//Mathf.Min(this.frame.width, this.frame.height) * 0.7f;
            h = this.frame.height * ratio;//w;
            rctran.sizeDelta = new Vector2(w, h);


            // //更新scrollview 内容的长度 
            rctran = objScrollViewContent.GetComponent<RectTransform>();
            Vector2 size = rctran.sizeDelta;
            size.x = w * totalPage;
            rctran.sizeDelta = size;

            //更新page
            uiPage0.width = w;
            uiPage0.heigt = h;
            uiPage0.LayOut();
        }

        {
            uiPage1.LayOut();
        }




    }


    void ScrollViewValueChanged(Vector2 newScrollValue)
    {
        indexPage = GetScrollViewPage();
        Debug.Log("ScrollViewValueChanged:" + " page=" + indexPage + " pos=" + scrollRect.content.anchoredPosition);
        uiScrollDot.UpdateItem(indexPage);

    }

    int GetScrollViewPage()
    {
        int ret = 0;
        RectTransform rctran = objContent.GetComponent<RectTransform>();
        float w = rctran.rect.width;
        float v = Mathf.Abs(scrollRect.content.anchoredPosition.x / w);
        int page = (int)v;
        float dif = v - page;
        ret = dif < 0.5f ? page : (page + 1);
        if (ret >= totalPage)
        {
            ret = totalPage - 1;
        }
        return ret;
    }

    void SetScrollViewPage(int page)
    {
        RectTransform rctran = objContent.GetComponent<RectTransform>();
        float w = rctran.rect.width;
        scrollRect.content.anchoredPosition = new Vector2(-w * page, 0);
    }
    public void OnScrollViewDrag(PointerEventData eventData, int status)
    {

        if (status == UIScrollViewTouchEvent.DRAG_END)
        {
            Debug.Log("OnScrollViewDrag pos=" + eventData.position);
            SetScrollViewPage(indexPage);
        }

    }
}
