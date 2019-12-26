using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWin : UIViewPop 
{
    public const string KEY_GAMEWIN_INFO_INTRO = "KEY_GAMEWIN_INFO_INTRO";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";

    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";


    public const string KEY_GAMEWIN_INFO_ALBUM = "KEY_GAMEWIN_INFO_ALBUM";

 
    public UITextView textView;
    public Text textTitle;
    public Image imageBg;
    public RawImage imageHead;
    public Button btnClose;

    public Button btnFriend;
    public Button btnNext;
    public Button btnAddLove;
    public GameObject objLayoutBtn;
    Color colorTitle = new Color32(255, 255, 255, 255);

    int indexSegment;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CrossItemInfo info = GameLevelParse.main.GetItemInfo();

        //Common.SetButtonText(btnFriend, Language.main.GetString("STR_GameWin_BtnFriend"));
        //Common.SetButtonText(btnNext, Language.main.GetString("STR_GameWin_BtnNext"), 0, false);
        //Common.SetButtonText(btnAddLove, Language.main.GetString("STR_GameWin_BtnAddLove"));

        string str = info.title;
        str = Language.main.GetString("STR_UIVIEWALERT_TITLE_GAME_FINISH");
        textTitle.text = str;


        textView.SetFontSize(80);
        textView.SetTextColor(new Color32(59, 219, 178, 255));

        textTitle.color = colorTitle;

        indexSegment = 0; 

        UpdateText(null);

    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0;
        float ratio = 0.8f;
        if (Device.isLandscape)
        {
            ratio = 0.8f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctranRoot.sizeDelta = new Vector2(w, h);

        }
        float w_btns_landscape = 420;
        float space = 32f;
        //textView
        {
            RectTransform rctran = textView.GetComponent<RectTransform>();
            float oftTop = 0;
            float oftBottom = 0;
            float oftLeft = 0;
            float oftRight = 0;
            if (Device.isLandscape)
            {
                oftLeft = space;
                oftRight = w_btns_landscape + space;
                oftTop = 300;
                oftBottom = space;
            }
            else
            {
                oftLeft = space;
                oftRight = space;
                oftTop = 300;
                oftBottom = 200;
            }
            w = rctranRoot.rect.width - oftLeft - oftRight;
            h = rctranRoot.rect.height - oftTop - oftBottom;
            x = ((-rctranRoot.rect.width / 2 + oftLeft) + (rctranRoot.rect.width / 2 - oftRight)) / 2;
            y = ((-rctranRoot.rect.height / 2 + oftBottom) + (rctranRoot.rect.height / 2 - oftTop)) / 2;
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
            textView.LayOut();
        }

        //objLayoutBtn
        {
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                w = w_btns_landscape;
                h = rctranRoot.rect.height;
                y = 0;
                x = rctranRoot.rect.width / 2 - w / 2 - space;
            }
            else
            {
                w = rctranRoot.rect.width;
                h = 160;
                x = 0;
                y = -rctranRoot.rect.height / 2 + h / 2 + space;
            }
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);


            LayOutGrid lg = objLayoutBtn.GetComponent<LayOutGrid>();
            lg.enableHide = false;
            int btn_count = lg.GetChildCount(false);
            if (Device.isLandscape)
            {
                lg.row = btn_count;
                lg.col = 1;
            }
            else
            {
                lg.row = 1;
                lg.col = btn_count;
            }
            lg.LayOut();
        }
    }

    public void UpdateText(ItemInfo info)
    {
        CrossItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();
        string str = "";

        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        textView.text = str;
    }
 
    public void OnClickBtnClose()
    {
        Close();
        GameManager.main.GotoPlayAgain();
    }
    public void OnClickBtnFriend()
    {
    }
    public void OnClickBtnNext()
    {
        Close();
        LevelManager.main.GotoNextLevel();
    }
    public void OnClickBtnAddLove()
    {
    }
}
