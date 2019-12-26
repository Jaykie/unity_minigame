
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public delegate void OnUIGameDotDelegate(UIGameDot ui, int status);
public class UIGameDot : UIView
{
    public GameObject objSprite;
    public bool isBg;
    public bool isSel;
    public int index;
    public int row;
    public int col;

    public int rowOrigin;
    public int colOrigin;
    public UITouchEventWithMove uiTouchEvent;
    public BoxCollider boxCollider;
    public bool enableMove;
    public OnUIGameDotDelegate callBackTouch { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        isSel = false;
        enableMove = true;
        uiTouchEvent.callBackTouch = OnUITouchEvent;
    }
    public void Start()
    {
        base.Start();

        Texture2D texDot;
        float scale = 1f;
        if (isBg)
        {
            scale = 0.3f;
            texDot = TextureCache.main.Load(GameRes.Image_GameDotBg);
        }
        else
        {
            scale = 1.5f;
            texDot = TextureCache.main.Load(GameRes.Image_GameDot);
        }
        SpriteRenderer rd = objSprite.GetComponent<SpriteRenderer>();
        rd.sprite = TextureUtil.CreateSpriteFromTex(texDot);
        objSprite.transform.SetParent(this.transform);
        objSprite.transform.localPosition = new Vector3(0, 0, 0);
        objSprite.transform.localScale = new Vector3(scale, scale, 1f);
        boxCollider.size = new Vector3(rd.bounds.size.x, rd.bounds.size.y, 1f);
    }

    public override void LayOut()
    {
        base.LayOut();
    }
    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        if (isBg)
        {
            return;
        }
        if (!enableMove)
        {
            return;
        }
        isSel = false;

        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {

                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {
                    Vector3 pt = Common.GetInputPositionWorld(mainCam);
                    float z = this.transform.position.z;
                    int r = GameUtil.main.GetDotRow(pt);
                    int c = GameUtil.main.GetDotCol(pt);
                    this.row = r;
                    this.col = c;
                    pt = GameUtil.main.GetDotPostion(r, c);
                    pt.z = z;
                    this.transform.position = pt;
                    if (callBackTouch != null)
                    {
                        isSel = true;
                        callBackTouch(this, status);
                    }
                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {
                    if (callBackTouch != null)
                    {
                        isSel = true;
                        callBackTouch(this, status);
                    }

                }
                break;

        }
    }


}
