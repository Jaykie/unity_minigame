using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
参考游戏： Word Collect: Word Games
https://apps.apple.com/cn/app/id1299956969
https://www.taptap.com/app/72589
 */
public class UIGameWordCollect : UIGameBase
{
    public GameObject objTopbar;
    public Image imageTopbar;
    public Text textTitle;


    public UITipsBarMathMaster uiTipsBar;
    public UIGameFinish uiGameFinish;

    //prefab 
    //static public UIGameBoxMathMaster uiGameBoxPrefab;


    float barHeightCanvas = 160;
    float adBannerHeightCanvas = 0;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        InitBg();

        LayOut();

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        uiTipsBar.UpdateGold(Common.gold);

        LayOut();

        //LayoutChild 必须在前面执行
        UpdateGuankaLevel(LevelManager.main.gameLevel);

        float delaytime = 0.1f * 10;
        Invoke("OnUIDidFinish", delaytime);
    }
    void LoadPrefab()
    {
        // {
        //     GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIGameBoxItemMathMaster");
        //     if (obj != null)
        //     {
        //         uiGameBoxItemPrefab = obj.GetComponent<UIGameBoxItemMathMaster>();
        //     }
        // }
        // {
        //     GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIGameBoxMathMaster");
        //     if (obj != null)
        //     {
        //         uiGameBoxPrefab = obj.GetComponent<UIGameBoxMathMaster>();
        //     }
        // }

    }
    public override void LayOut()
    {
        float x, y, w, h;

        Vector2 sizeCanvas = this.frame.size;
        if (sizeCanvas.x <= 0)
        {
            return;
        }
        LayoutChildBase();
        adBannerHeightCanvas = GameManager.main.heightAdCanvas;
        Debug.Log("adBannerHeightCanvas=" + adBannerHeightCanvas);

    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetGuankaItemInfo(level);

    }




    public void InitBg()
    {
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);

    }
    void ShowShop()
    {
        ShopViewController.main.Show(null, null);
    }
    void OnGameWin()
    {
        LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;


        int step_gold = AppRes.GOLD_GUANKA_STEP;//5

        if ((LevelManager.main.gameLevel >= step_gold) && (LevelManager.main.gameLevel % step_gold == 0))

        {
            Common.gold += AppRes.GOLD_GUANKA;
        }
        ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
    }

}

