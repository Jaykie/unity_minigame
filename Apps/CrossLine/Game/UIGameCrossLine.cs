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
public class UIGameCrossLine : UIGameBase, ILetterConnectDelegate, IUILetterConnectDelegate
{
    public GameObject objTopbar;
    public GameObject objLayoutBtn;
    public Image imageTopbar;
    public Text textTitle;

    //prefab 
    public GameCrossLine gamePrefab;

    public GameObject objGoldBar;
    public Image imageGoldBg;
    public Text textGold;


    GameCrossLine game;

    float barHeightCanvas = 160;
    float adBannerHeightCanvas = 0;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        InitBg();
        UpdateGold();
        LayOut();

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //LayoutChild 必须在前面执行
        UpdateGuankaLevel(LevelManager.main.gameLevel);
        float delaytime = 0.1f * 10;
        Invoke("OnUIDidFinish", delaytime);
    }
    void LoadPrefab()
    {



    }


    public override void LayOut()
    {
        float x, y, z = 0, w = 0, h = 0;
        Vector2 sizeWorld = Common.GetWorldSize(mainCam);
        Vector2 sizeCanvas = this.frame.size;
        float ratio = 1f;
        if (sizeCanvas.x <= 0)
        {
            return;
        }
        LayoutChildBase();
        adBannerHeightCanvas = GameManager.main.heightAdCanvas;
        Debug.Log("adBannerHeightCanvas=" + adBannerHeightCanvas);



        if (game != null)
        {
            game.LayOut();
        }
    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetGuankaItemInfo(level);

        game = (GameCrossLine)GameObject.Instantiate(gamePrefab);
        AppSceneBase.main.AddObjToMainWorld(game.gameObject);
        UIViewController.ClonePrefabRectTransform(gamePrefab.gameObject, game.gameObject);
        game.transform.localPosition = new Vector3(0f, 0f, -1f);
        game.UpdateGuankaLevel(level);

        game.letterConnect.iDelegate = this;

        UpdateLevelTitle();
        //UpdateItem 先layout一次
        LayOut();

        //  uiLetterConnect.UpdateItem();
        //   uiWordAnswer.UpdateItem();
        LayOut();
    }



    public void UpdateLevelTitle()
    {
        string str = Language.main.GetString("GAME_LEVEL");
        int level = LevelManager.main.gameLevel + 1;
        if (str.Contains("xxx"))
        {
            str = str.Replace("xxx", level.ToString());
        }
        else
        {
            str += level.ToString();
        }
        textTitle.text = str;

        {
            RectTransform rctran = imageTopbar.GetComponent<RectTransform>();
            float w = Common.GetStringLength(textTitle.text, AppString.STR_FONT_NAME, textTitle.fontSize) + textTitle.fontSize;
            rctran.sizeDelta = new Vector2(w, rctran.sizeDelta.y);
        }
    }



    public void InitBg()
    {
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);

    }
    void ShowShop()
    {
        ShopViewController.main.Show(null, null);
    }


    void UpdateGold()
    {
        string str = Language.main.GetString("STR_GOLD") + ":" + Common.gold.ToString();
        textGold.text = str;
        int fontsize = textGold.fontSize;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        RectTransform rctran = objGoldBar.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;
        sizeDelta.x = str_w + fontsize;
        rctran.sizeDelta = sizeDelta;
    }
    public void OnNotEnoughGold(bool isUpdate)
    {
        if (isUpdate)
        {
            UpdateGold();
        }
        else
        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_NOT_ENOUGH_GOLD);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_NOT_ENOUGH_GOLD);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_NOT_ENOUGH_GOLD);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_NOT_ENOUGH_GOLD);

            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GOLD, OnUIViewAlertFinished);
        }

    }
    void OnGameWin()
    {
        LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;

        Common.gold += AppRes.GOLD_GUANKA;
        UpdateGold();

        ShowAdInsert(GAME_AD_INSERT_SHOW_STEP, false);
        PopUpManager.main.Show<UIGameWin>("App/Prefab/Game/UIGameWin");
    }

    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {


        if (STR_KEYNAME_VIEWALERT_GOLD == alert.keyName)
        {
            if (isYes)
            {
                ShowShop();
            }
        }



    }
    //
    public void OnLetterConnectDidRightAnswer(LetterConnect lc, int idx)
    {

    }


    public void OnLetterConnectDidUpdateItem(LetterConnect lc, int[] itemIndex)
    {

    }
    //

    public void OnUILetterConnectDidAgain(UILetterConnect ui)
    {
        if (game != null)
        {
            game.letterConnect.OnClickAgain();
        }
    }
    public void OnUILetterConnectDidTips(UILetterConnect ui)
    {
        if (Common.gold <= 0)
        {
            OnNotEnoughGold(false);
            return;
        }

        Common.gold--;
        if (Common.gold < 0)
        {
            Common.gold = 0;
        }
        OnNotEnoughGold(true);


        OnGameWin();
    }


    public void OnClickGold()
    {
        ShowShop();
    }

}

