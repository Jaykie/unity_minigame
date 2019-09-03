
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
        LayOut();
    }

    void LoadPrefab()
    {

    }
    public override void LayOut()
    {



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
