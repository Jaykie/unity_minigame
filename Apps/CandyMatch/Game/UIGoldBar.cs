
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
        LayOutChild();
    }


    void LayOutChild()
    {

    }

    public void UpdateGold()
    {
        textGold.text = Common.gold.ToString();
        {
            RectTransform rctran = textGold.GetComponent<RectTransform>();
            float w = Common.GetStringLength(textGold.text, AppString.STR_FONT_NAME, textGold.fontSize);
            rctran.sizeDelta = new Vector2(w, rctran.sizeDelta.y);
        }
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
