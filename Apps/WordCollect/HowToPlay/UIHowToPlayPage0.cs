using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHowToPlayPage0 : UIView
{
    public Text textTitle;
    public Text textDetail;  
    public Image imageBg;
    public Image imageGuide;
    public float width;
    public float heigt;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        textTitle.color = AppRes.colorTitle;
        textDetail.color = AppRes.colorTitle; 
        textTitle.text = Language.main.GetString("STRING_HOWTOPLAY_TITLE0");
        textDetail.text = Language.main.GetString("STRING_HOWTOPLAY_DETAIL0");

    }
    void Start()
    {
        LayOut();
        // Vector2 pt0 = imageBg.GetComponent<RectTransform>().anchoredPosition;
        // Vector2 pt1 = image1.GetComponent<RectTransform>().anchoredPosition;
        // Vector2 pt2 = image2.GetComponent<RectTransform>().anchoredPosition;
        // Debug.Log("pt0=" + pt0 + " pt1=" + pt1 + " pt2" + pt2);
        // Sequence seq = DOTween.Sequence();
        // RectTransform rctran = imageGuide.GetComponent<RectTransform>();
        // rctran.anchoredPosition = pt0;
        // float t_animation = 2f;
        // Tweener action0 = rctran.DOLocalMove(pt1, t_animation).SetEase(Ease.InSine);
        // Tweener action1 = rctran.DOLocalMove(pt2, t_animation).SetEase(Ease.InSine);
        // seq.Append(action0).Append(action1).AppendInterval(t_animation / 2).OnComplete(
        //     () =>
        //     {
        //         rctran.anchoredPosition = pt0;
        //     }
        // ).SetLoops(-1);
    }


    public override void LayOut()
    {
        float x, y, w, h;
        {
            RectTransform rctranPage = this.GetComponent<RectTransform>();
            Debug.Log(" page.rect=" + rctranPage.rect + " width=" + width + " heigt=" + heigt);
            x = width / 4;
            y = 0;
           
        }

    }


}
