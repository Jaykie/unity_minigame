using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIHomePlay : UIView
{


    public Button btnPlay;
    public Button btnLearn;
    public Button btnAdVideo;

    public UIViewController controllerHome;
    void Awake()
    {
        controllerHome = HomeViewController.main;
        if (!Config.main.APP_FOR_KIDS)
        {
            btnLearn.gameObject.SetActive(false);
        }
    }
    // Use this for initialization
    void Start()
    {
        LayOut();

    }



    public override void LayOut()
    {

    }


    public void OnClickBtnPlay()
    {

        Debug.Log("OnClickBtnPlay");
        if (controllerHome != null)
        {
            NaviViewController navi = controllerHome.naviController;
            int total = LevelManager.main.placeTotal;
            if (total > 1)
            {
                navi.Push(PlaceViewController.main);
            }
            else
            {
                navi.Push(GuankaViewController.main);
            }
        }
    }

    public void OnClickBtnLearn()
    {

        if (controllerHome != null)
        {
            NaviViewController navi = controllerHome.naviController;
            //  navi.Push(LearnViewController.main);

        }
    }

    public void OnClickBtnAdVideo()
    {
    }

}
