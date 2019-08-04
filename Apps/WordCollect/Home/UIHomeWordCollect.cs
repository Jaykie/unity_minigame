using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHomeWordCollect : UIHomeBase, IPopViewControllerDelegate
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    //f88816
    public GameObject objLayoutBtn;
    public UIGoldBar uiGoldBar;
    public Text textTitle;
    public Button btnNoAd;
    public Button btnShare;
    public Button btnSetting;
    public Button btnMore;

    void Awake()
    {
        //TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_GAME_BG, true);
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_HOME_BG);

        if (Common.isWinUWP)
        {
            // uiGoldBar.gameObject.SetActive(false);
        }

        if (Common.isAndroid)
        {
            btnNoAd.gameObject.SetActive(false);
        }
        if (Common.isWinUWP)
        {
            btnMore.gameObject.SetActive(false);
            btnNoAd.gameObject.SetActive(false);
        }

    }
    // Use this for initialization
    void Start()
    {
        LayOut();

        Vector2 pt = new Vector2(0, 40);

        UpdateTitle();

        OnUIDidFinish();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }
    void UpdateTitle()
    {
        //textTitle.text = title; 

    }
    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        UpdateTitle();
    }


    public void OnClickBtnPlay()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(GuankaViewController.main);//  
        }
    }
    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        float x = 0, y = 0, w = 0, h = 0;
        // {
        //     RectTransform rctran = imageBg.GetComponent<RectTransform>();
        //     w = imageBg.texture.width;//rectTransform.rect.width;
        //     h = imageBg.texture.height;//rectTransform.rect.height;
        //     if (w != 0)
        //     {
        //         float scalex = sizeCanvas.x / w;
        //         float scaley = sizeCanvas.y / h;
        //         float scale = Mathf.Max(scalex, scaley);
        //         imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

        //         //屏幕坐标 现在在屏幕中央
        //         imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);

        //     }

        // }

        //image name 

        {
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            x = 0;
            if (uiHomeAppCenter != null)
            {
                GridLayoutGroup gridLayout = uiHomeAppCenter.GetComponent<GridLayoutGroup>();
                Vector2 cellSize = gridLayout.cellSize;

                if (Device.isLandscape)
                {
                    h = 0;
                }
                else
                {
                    h = cellSize.y;
                }
            }
            Debug.Log("homemathmaster:sizeCanvas=" + sizeCanvas + " this.frame=" + this.frame + " h=" + h);
            y = -(this.frame.size.y / 2 - h) / 2;
            rctran.anchoredPosition = new Vector2(x, y);
        }


        //LayoutChildBase();
    }
}
