
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUITipsBarMathMasterClickDelegate(int idx);
public class UITipsBarMathMaster : UIView
{
    public Image imageBg;
    public Button btnGold;
    public Text textGold;
    public Button btnTips;
    public OnUITipsBarMathMasterClickDelegate callbackClick { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        textGold.color = AppRes.colorTitle;
        Common.SetButtonText(btnTips, Language.main.GetString("STRING_GAME_TISHI"), 8);
        UpdateGold(Common.gold);
        if ((!AppVersion.appCheckHasFinished) && (!Config.main.isHaveIAP))
        {
            textGold.gameObject.SetActive(false);
            btnGold.gameObject.SetActive(false);
        }
        if (Common.isWinUWP)
        {
            // textGold.gameObject.SetActive(false);
            // btnGold.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        LayOut();
    }

    public override void LayOut()
    {
        imageBg.gameObject.SetActive(true);
        if (Device.isLandscape)
        {
            imageBg.gameObject.SetActive(false);
        }
        // RectTransform rctran = uiTips.GetComponent<RectTransform>();
        // Vector2 size = rctran.sizeDelta;
        // size.x = this.frame.size.x / 2;
        // rctran.sizeDelta = size;
        // rctran.anchoredPosition = new Vector2(this.frame.width / 4 - 48, 0);
        // uiTips.LayOut();

    }
    public void UpdateGold(int gold)
    {
        textGold.text = gold.ToString();
        {
            RectTransform rctran = textGold.GetComponent<RectTransform>();
            float w = Common.GetStringLength(textGold.text, AppString.STR_FONT_NAME, textGold.fontSize);
            rctran.sizeDelta = new Vector2(w, rctran.sizeDelta.y);
        }
    }
    public void OnClickBtnHelp()
    {
        //   HelpViewController.main.Show(null, null);
    }

    public void OnUITipsClick(int idx)
    {
        if (callbackClick != null)
        {
            callbackClick(idx);
        }
    }

    public void OnClickBtnAgain()
    {
        OnUITipsClick(0);
    }

    public void OnClickBtnTips()
    {
        OnUITipsClick(1);
    }

    public void OnClickBtnGold()
    {
        OnUITipsClick(2);
    }
}
