
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Assertions;

using GameVanilla.Core;
using GameVanilla.Game.Common;
using GameVanilla.Game.Popups;
using GameVanilla.Game.UI;



public class UIHomeCandyMatch : UIHomeBase, IPopViewControllerDelegate
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

    public Button btnMore;

    public Button btnSetting;

    public BaseScene baseScene;

    public void Awake()
    {
        base.Awake();
        InitGame();
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
    public void Start()
    {
        base.Start();
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
    void InitGame()
    {
        float w, h;
        //candy mathch 设置为10
        mainCam.orthographicSize = 10f;
        {
            GameObject obj = GameObject.Find("Canvas");
            if (obj != null)
            {
                CanvasScaler canvasScaler = obj.GetComponent<CanvasScaler>();
                canvasScaler.matchWidthOrHeight = 0.5f;
                w = 1280;
                h = 1920;
                if (!Device.isLandscape)
                {
                    //
                    canvasScaler.referenceResolution = new Vector2(Mathf.Min(w, h), Mathf.Max(w, h));
                }
                else
                {
                    canvasScaler.referenceResolution = new Vector2(Mathf.Max(w, h), Mathf.Min(w, h));
                }
            }
        }

        AdKitCommon.main.enableBanner = false;


        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Home/SoundManager");
            obj = GameObject.Instantiate(obj);
            obj.name = "SoundManager";
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Home/BackgroundMusic");
            obj = GameObject.Instantiate(obj);
            obj.name = "BackgroundMusic";
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Home/Loader");
            obj = GameObject.Instantiate(obj);
            obj.name = "Loader";
        }


    }
    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        UpdateTitle();
    }

    public void OnStartGamePopupDidPlay()
    {

        Invoke("GotoGame", 0.6f);
    }
    public void OnClickBtnPlay()
    {
        // int numLevel = 1;
        // var numLives = PlayerPrefs.GetInt("num_lives");
        StartGamePopup.main.callbackPlay = OnStartGamePopupDidPlay;

        // if (numLives > 0)
        // {
        //     if (!FileUtils.FileExists("Levels/" + numLevel))
        //     {
        //         baseScene.OpenPopup<AlertPopup>("Popups/AlertPopup",
        //             popup => popup.SetText("This level does not exist."));
        //     }
        //     else
        //     {
        //         baseScene.OpenPopup<StartGamePopup>("Popups/StartGamePopup", popup =>
        //         {
        //             popup.LoadLevelData(numLevel);
        //         });
        //     }
        // }
        GotoGame();

    }

    public void GotoGame()
    {
        PuzzleMatchManager.instance.lastSelectedLevel = 1;
        // SceneManager.LoadScene("GameScene");
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(GuankaViewController.main);//  GuankaViewController GameViewController
        }
    }

    public void OnClickBtnPlay2()
    {
        PuzzleMatchManager.instance.lastSelectedLevel = 1;
        SceneManager.LoadScene("GameScene");

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

        //objLayoutBtn

        {
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            LayOutGrid lyg = objLayoutBtn.GetComponent<LayOutGrid>();
            float size_ly = 160f;
            if (Device.isLandscape)
            {
                lyg.row = 1;
                lyg.col = 3;
                h = size_ly;
                w = lyg.col * h;
            }
            else
            {
                lyg.row = 3;
                lyg.col = 1;
                w = size_ly;
                h = lyg.row * w;
            }

            x = 0;
            float ofty = 0;
            if (uiHomeAppCenter != null)
            {
                GridLayoutGroup gridLayout = uiHomeAppCenter.GetComponent<GridLayoutGroup>();
                Vector2 cellSize = gridLayout.cellSize;
                if (!Device.isLandscape)
                {
                    ofty = cellSize.y;
                }
            }
            Debug.Log("homemathmaster:sizeCanvas=" + sizeCanvas + " this.frame=" + this.frame + " h=" + h);
            y = -this.frame.size.y / 2 + ofty + h / 2;
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
            lyg.LayOut();
        }


        //LayoutChildBase();
    }

    public void OnClickBtnSettingCandy()
    {
        baseScene.OpenPopup<SettingsPopup>("Popups/SettingsPopup");
    }
    public void OnClickBtnMusicCandy()
    {
        SoundManager.instance.ToggleMusic();
        OnClickBtnMusic();
         
    }
    public void OnClickBtnSoundCandy()
    {
        SoundManager.instance.ToggleSound();
        OnClickBtnSound();
    }

}
