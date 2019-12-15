using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UIText : UIView
{
    public Text textTitle;
    public string text
    {
        get
        {
            return textTitle.text;
        }

        set
        {
            textTitle.text = value;
            LayOut();
        }

    }

    //



    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        UpdateLanguage();
    }
    // Use this for initialization
    void Start()
    {

    }

    public void SetFontSize(int sz)
    {
        textTitle.fontSize = sz;
    }

    public void SetTextColor(Color cr)
    {
        textTitle.color = cr;
    }
    public override void LayOut()
    {
        base.LayOut();
    }
    public override void UpdateLanguage()
    {
        string str = GetKeyText();
        if (!Common.isBlankString(str))
        {
            this.text = GetKeyText();
        }
    }
}
