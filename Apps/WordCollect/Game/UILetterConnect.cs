
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILetterConnect : UIView
{

    public Text textTitle;
    public Image imageTitle;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        ShowText(false);
        LayOut();
    }

    void LoadPrefab()
    {

    }
    public override void LayOut()
    {



    }

    public void UpdateText(string str)
    {
        textTitle.text = str;
        RectTransform rctran = imageTitle.GetComponent<RectTransform>();
        float w, h;
        int fontSize = textTitle.fontSize;
        w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontSize) + fontSize;
        h = rctran.sizeDelta.y;
        rctran.sizeDelta = new Vector2(w, h);

    }

    public void ShowText(bool isShow)
    {
        imageTitle.gameObject.SetActive(isShow);
    }
    void OnClickBtnLetter()
    {

    }

    public void OnClickAgain()
    {

    }
    public void OnClickTips()
    {

    }
}
