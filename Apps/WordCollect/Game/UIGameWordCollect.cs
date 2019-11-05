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
public class UIGameWordCollect : UIGameBase, ILetterConnectDelegate, IUILetterConnectDelegate, IUIWordContentBaseDelegate
{
    public GameObject objTopbar;
    public GameObject objLayoutBtn;
    public Image imageTopbar;
    public Text textTitle;
    public UILetterConnect uiLetterConnect;
    //prefab 
    public GameWordCollect gamePrefab;

    public GameObject objGoldBar;
    public Image imageGoldBg;
    public Text textGold;


    GameWordCollect game;

    float barHeightCanvas = 160;
    float adBannerHeightCanvas = 0;
    int indexAnswer;

    public UIWordContentBase uiWordContent;

    UIWordFillBox uiWordFillBoxPrefab;
    UIWordAnswer uiWordAnswerPrefab;
    UIWordImageText uiWordImageTextPrefab;

    UIWordAnswer uiWordAnswer;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        InitBg();
        uiLetterConnect.iDelegate = this;

        if (!AppVersion.appCheckHasFinished)
        {
            objGoldBar.SetActive(false);
        }

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

        // PopUpManager.main.Show<UIGameWin>("App/Prefab/Game/UIGameFail");
    }
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIWordFillBox");
            if (obj != null)
            {
                uiWordFillBoxPrefab = obj.GetComponent<UIWordFillBox>();
            }
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIWordImageText");
            if (obj != null)
            {
                uiWordImageTextPrefab = obj.GetComponent<UIWordImageText>();
            }
        }

        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIWordAnswer");
            if (obj != null)
            {
                uiWordAnswerPrefab = obj.GetComponent<UIWordAnswer>();
            }
        }
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

        //letter connect
        if (uiLetterConnect != null)
        {
            ratio = GameWordCollect.RATIO_RECT;
            RectTransform rctan = uiLetterConnect.GetComponent<RectTransform>();
            float oft_bottom = GameManager.main.heightAdCanvas;// + Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetBottom);
            if (Device.isLandscape)
            {
                h = (sizeCanvas.y - oft_bottom * 2) * ratio;
                w = (sizeCanvas.x / 2) * ratio;
                y = 0;
                x = sizeCanvas.x / 4;
            }
            else
            {
                w = sizeCanvas.x * ratio;
                h = (sizeCanvas.y / 2 - oft_bottom) * ratio;
                x = 0;
                y = (-sizeCanvas.y / 2 + oft_bottom) / 2;
            }
            rctan.sizeDelta = new Vector2(w, h);
            rctan.anchoredPosition = new Vector2(x, y);
            uiLetterConnect.LayOut();
            RectTransform rctanBtn = objLayoutBtn.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                x = 0;
            }
            rctanBtn.anchoredPosition = new Vector2(x, y);
        }

        float oft_top = 160f;
        if (uiWordContent != null)
        {
            ratio = GameWordCollect.RATIO_RECT;
            RectTransform rctan = uiWordContent.GetComponent<RectTransform>();

            if (Device.isLandscape)
            {
                h = (sizeCanvas.y - oft_top * 2) * ratio;
                w = (sizeCanvas.x / 2) * ratio;
                y = 0;
                x = -sizeCanvas.x / 4;
            }
            else
            {
                w = sizeCanvas.x * ratio;
                h = (sizeCanvas.y / 2 - oft_top) * ratio;
                x = 0;
                y = (sizeCanvas.y / 2 - oft_top) / 2;
            }

            rctan.sizeDelta = new Vector2(w, h);
            rctan.anchoredPosition = new Vector2(x, y);
            uiWordContent.LayOut();
        }

        if (game != null)
        {
            game.LayOut();
        }
    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        LanguageManager.main.UpdateLanguage(LevelManager.main.placeLevel);
        WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetGuankaItemInfo(level);
        GameGuankaParse.main.ParseItem(info);
        indexAnswer = 0;
        game = (GameWordCollect)GameObject.Instantiate(gamePrefab);
        AppSceneBase.main.AddObjToMainWorld(game.gameObject);
        UIViewController.ClonePrefabRectTransform(gamePrefab.gameObject, game.gameObject);
        game.transform.localPosition = new Vector3(0f, 0f, -1f);
        game.UpdateGuankaLevel(level);
        game.letterConnect.uiLetterConnect = uiLetterConnect;
        game.letterConnect.iDelegate = this;


        //  public const string GAME_TYPE_WORDLIST = "WordList";  
        //  public const string GAME_TYPE_POEM = "Poem"; 


        GameObject objPrefab = null;

        switch (info.gameType)
        {
            case GameRes.GAME_TYPE_CONNECT:
                {
                    objPrefab = uiWordFillBoxPrefab.gameObject;
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordFillBoxPrefab);
                }
                break;
            case GameRes.GAME_TYPE_IMAGE:
            case GameRes.GAME_TYPE_TEXT:
            case GameRes.GAME_TYPE_IMAGE_TEXT:
            case GameRes.GAME_TYPE_POEM:
                {
                    objPrefab = uiWordImageTextPrefab.gameObject;
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordImageTextPrefab);
                }
                break;
            case GameRes.GAME_TYPE_WORDLIST:
                {
                    objPrefab = uiWordAnswerPrefab.gameObject;
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordAnswerPrefab);
                    uiWordAnswer = uiWordContent as UIWordAnswer;
                }
                break;

            default:
                break;
        }
        if (uiWordContent != null)
        {
            uiWordContent.transform.SetParent(this.transform);
            uiWordContent.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(objPrefab, uiWordContent.gameObject);
            uiWordContent.iDelegate = this;
            uiWordContent.infoItem = info;
            uiWordContent.UpdateGuankaLevel(level);
        }

        UpdateLevelTitle();
        //UpdateItem 先layout一次
        LayOut();

        uiLetterConnect.UpdateItem();
        LayOut();
    }



    public void UpdateLevelTitle()
    {
        WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetItemInfo();
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

        if (info.gameType == GameRes.GAME_TYPE_POEM)
        {
            str = info.id;
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
        Debug.Log("Common.gold=" + Common.gold + " str=" + str);
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
    void DoGameWin(bool istips)
    {
        if (uiWordAnswer != null)
        {
            if (!uiWordAnswer.uiWordList.IsGameWin())
            {
                return;
            }
        }

        if (!istips)
        {
            LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;
            // int step_gold = AppRes.GOLD_GUANKA_STEP;//5

            // if ((LevelManager.main.gameLevel >= step_gold) && (LevelManager.main.gameLevel % step_gold == 0))
            {
                Common.gold += AppRes.GOLD_GUANKA;
            }
            UpdateGold();
            ShowAdInsert(GAME_AD_INSERT_SHOW_STEP, false);
        }

        PopUpManager.main.Show<UIGameWin>("App/Prefab/Game/UIGameWin");
    }

    void OnGameWin()
    {
        DoGameWin(false);
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
        WordItemInfo info = GameGuankaParse.main.GetItemInfo();
        if (uiWordAnswer != null)
        {
            UICellWord item = uiWordAnswer.uiWordList.GetItem(idx);
            if (item.GetItem(0).GetStatus() == UILetterItem.Status.LOCK)
            {
                item.SetStatus(UILetterItem.Status.UNLOCK);

                indexAnswer++;
                if (indexAnswer < info.listAnswer.Length)
                {
                    Debug.Log("indexAnswer =" + indexAnswer);
                    uiLetterConnect.RunItemAnimate(lc, item, this, ui =>
                        {
                            if (info.gameType != GameRes.GAME_TYPE_IMAGE)
                            {
                                GameGuankaParse.main.UpdateLetterString(indexAnswer);
                                game.letterConnect.UpdateItem();
                                uiLetterConnect.UpdateItem();
                            }
                            uiWordAnswer.uiWordList.GotoListIndex(indexAnswer);

                        }

                     );


                }

            }
            else if (item.GetItem(0).GetStatus() == UILetterItem.Status.UNLOCK)
            {
                item.SetStatus(UILetterItem.Status.DUPLICATE);
                AudioPlay.main.PlayFile(GameRes.Audio_WordDuplicate);
            }
        }

        Invoke("OnGameWin", uiLetterConnect.durationAnimate);
    }


    public void OnLetterConnectDidUpdateItem(LetterConnect lc, int[] itemIndex)
    {
        if (uiLetterConnect != null)
        {
            uiLetterConnect.OnLetterConnectDidUpdateItem(lc, itemIndex);
        }
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
        Debug.Log("Common.gold=" + Common.gold);
        //UpdateGold();
        OnNotEnoughGold(true);
        if (uiWordAnswer != null)
        {
            UICellWord item = uiWordAnswer.uiWordList.GetFirstLockItem();
            if (item != null)
            {
                item.SetStatus(UILetterItem.Status.UNLOCK);
            }
        }
        DoGameWin(true);
    }


    public void OnClickGold()
    {
        ShowShop();
    }

    public void UIWordContentBaseDidBackWord(UIWordContentBase ui, string word)
    {

    }
    public void UIWordContentBaseDidTipsWord(UIWordContentBase ui, string word)
    {

    }

}

