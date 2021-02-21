
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UIGameFinish : UIView
{
    public GameObject objContent;
    public Image imageBg;
    public Image imageBoard;
    public Image imageLogo;
    public Text textTitle;
    public Button btnShare;
    public Button btnComment;
    public Button btnContinue;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        textTitle.text = Language.main.GetString("STRING_GAME_FINISH_TITLE_NORMAL");
        textTitle.color = AppRes.colorTitle;
        Text btnText = Common.GetButtonText(btnShare);
        float oft = btnText.fontSize;
        Common.SetButtonText(btnShare, Language.main.GetString("STRING_GAME_FINISH_BTN_SHARE"), oft);
        Common.SetButtonText(btnComment, Language.main.GetString("STR_BTN_COMMENT"), oft);
        Common.SetButtonText(btnContinue, Language.main.GetString("STRING_GAME_FINISH_BTN_CONTINUE"), oft);


        TextureUtil.UpdateImageTexture(imageLogo, AppRes.IMAGE_LOGO, true);
        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_GUANKA_BG, true);

        btnComment.gameObject.SetActive(AppVersion.appCheckHasFinished);

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        LayOut();
    }


    public override void LayOut()
    {
        float x, y, w, h;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        if((imageBg!=null)&&(imageBg.sprite!=null))
        {

            w = imageBg.sprite.texture.width;//rectTransform.rect.width;
            h = imageBg.sprite.texture.height;
            float scalex = sizeCanvas.x / w;
            float scaley = sizeCanvas.y / h;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }
        {
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            w = Mathf.Min(this.frame.width, this.frame.height-GameManager.main.heightAdCanvas*2) * 0.9f;
            h = w;
            rctran.sizeDelta = new Vector2(w, h);

        }
        RectTransform rctranContent = objContent.GetComponent<RectTransform>();
        {

            RectTransform rctran = imageLogo.GetComponent<RectTransform>();
            w = imageLogo.sprite.texture.width;//rectTransform.rect.width;
            h = imageLogo.sprite.texture.height;//rectTransform.rect.height;
            if (w != 0)
            {
                float scalex = rctranContent.rect.width / w;
                float scaley = rctranContent.rect.height / h;
                float scale = Mathf.Min(scalex, scaley) * 0.9f;
                imageLogo.transform.localScale = new Vector3(scale, scale, 1.0f);
            }

        }

    }

    void ShowShare()
    {
        ShareViewController.main.callBackClick = OnUIShareDidClick;
        ShareViewController.main.Show(null, null);
    }



    public void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIGAME_SHARE_TITLE");
        WordItemInfo infoGuanka = GameLevelParse.main.GetGuankaItemInfo(LevelManager.main.gameLevel) as WordItemInfo;
        string str = " ";
        if (infoGuanka != null)
        {
            str = infoGuanka.title;
        }
        string detail = Language.main.GetReplaceString("UIGAME_SHARE_DETAIL", AppCommon.STR_LANGUAGE_REPLACE, str);
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }
    public void OnClickBtnShare()
    {
        ShowShare();
    }
    public void OnClickBtnComment()
    {
        AppVersion.main.OnComment();
    }
    public void OnClickBtnContinue()
    {
        this.gameObject.SetActive(false);
        LevelManager.main.GotoNextLevel();
    }


}
