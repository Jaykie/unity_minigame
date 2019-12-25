
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UIGoldBar : UIView, IPopViewControllerDelegate
{
    public Image imageBg;
    public Image imageGold;
    public Text textGold;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        UpdateGold();
        LayOut();
    }


    public override void LayOut()
    {

    }

    public void UpdateGold()
    {
        string str = Language.main.GetString("STR_GOLD") + ":" + Common.gold.ToString();
        textGold.text = str;
        int fontsize = textGold.fontSize;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        RectTransform rctran = this.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;
        sizeDelta.x = str_w + fontsize;
        rctran.sizeDelta = sizeDelta;
    }

    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        UpdateGold();
    }
    public void OnClickGold()
    {
        ShopViewController.main.Show(null, this);
    }


}
