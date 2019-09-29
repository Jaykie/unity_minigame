
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUITipsClickDelegate(int idx);
public class UITips : UIView
{
    public Image imageBg;

    public Text textTips;
    public GameObject objGold;

    public Button btnGold;
    public Text textGold;

    public OnUITipsClickDelegate callbackClick { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        textTips.text = Language.main.GetString("STRING_GAME_TISHI");
        imageBg.gameObject.SetActive(true);
        if (Device.isLandscape)
        {
            // imageBg.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        LayOut();
    }

    public override void LayOut()
    {
        {
            RectTransform rctran = textTips.GetComponent<RectTransform>();
            Vector2 pos = rctran.anchoredPosition;
            pos.x = -this.frame.size.x / 4;
            rctran.anchoredPosition = pos;
        }
        if (objGold != null)
        {
            RectTransform rctran = objGold.GetComponent<RectTransform>();
            Vector2 pos = rctran.anchoredPosition;
            pos.x = this.frame.size.x / 4;
            rctran.anchoredPosition = pos;
            rctran.sizeDelta = new Vector2(this.frame.width / 2, this.frame.height);
        }


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

    public void OnClick(int idx)
    {
        if (callbackClick != null)
        {
            callbackClick(idx);
        }
    }

    public void OnClickBtnTips()
    {
        OnClick(1);
    }
    public void OnClickBtnGold()
    {
        OnClick(2);
    }
}

